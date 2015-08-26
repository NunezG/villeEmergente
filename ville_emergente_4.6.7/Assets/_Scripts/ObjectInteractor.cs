using UnityEngine;
using System.Collections;

public class ObjectInteractor : MonoBehaviour {

    public GameObject hitObject,inHandPosition,inHandObject;
    public bool handsFull = false;
    public float range;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown ("Action"))
		{
            RaycastHit hit;
            //if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 2, 1 << 8))
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out hit, range, 1 << 8))
            {
                //print("Input.mousePosition : " + Input.mousePosition);
                hitObject = hit.collider.gameObject; // on recupere l'objet vise
                InteractibleObject interacObj = hitObject.GetComponent<InteractibleObject>(); // on recupere sa composante InteractibleObject

                if (handsFull && (interacObj.type == InteractibleType.NPC // si on a un objet en main et qu'on vise un NPC
                    || ( interacObj.type == InteractibleType.SettingPiece && !interacObj.HasFragment() ) ) ) // ou un batiment vide
                {
                    //ajout de l'objet en main a l'objet vise
                    print("ajout de l'objet en main a l'objet vise");
                   // WwiseAudioManager.instance.PlayFiniteEvent("linker_morceau", this.gameObject);
                    interacObj.OnAddingFragment(inHandObject.GetComponent<Fragment>());
                    AddingFragment();
					//
					interacObj.GetComponent<Building>().Down();
                }
                else if (!handsFull && (interacObj.type == InteractibleType.Fragment // si on a rien en main et qu'on vise un objet rammassable
                    || (interacObj.type == InteractibleType.SettingPiece && interacObj.HasFragment()))) // ou un batiment avec un fragment
                {
                    print("on ramasse l'objet");
                    //on ramasse l'objet
                    PickUpObject(interacObj.OnPickUp());
                }
                else if (!handsFull && (interacObj.type == InteractibleType.SettingPiece && !interacObj.HasFragment())) // si on a rien en main et qu'on vise un element de decor vide
                {
					WwiseAudioManager.instance.PlayFiniteEvent("toucher_element", this.gameObject);
                    //print("(!handsFull && interacObj.type == InteractibleType.SettingPiece)");
                    //on declenche l'interaction avec cet objet
                   // interacObj.OnInteract();
                }
                else if (handsFull) // sinon, si on a juste un objet en main et qu'on ne vise pas un NPC
                {
                    //on laisse tomber l'objet en main
                    DropInHandObject();
                }
            }
            else
            {
                if (inHandObject != null)
                {
                    // on laisse tomber l'objet en main
                    DropInHandObject();
                }
            }
        }
    }


    public void PickUpObject(GameObject toPickUp)
    {
        print("Interactor:PickUpObject");
		inHandObject = toPickUp;
        inHandObject.transform.parent = inHandPosition.transform;
        inHandObject.transform.position = inHandPosition.transform.position;
        inHandObject.transform.rotation = inHandPosition.transform.rotation;
        inHandObject.rigidbody.isKinematic = true;
        handsFull = true;    
	}

    public void DropInHandObject()
    {
        print("Interactor:DropInHandObject");
		if (inHandObject != null)
        {
            inHandObject.GetComponent<Fragment>().Drop();
			        
            handsFull = false;
            inHandObject = null;

        }
	}

    public void AddingFragment()
    {
		print("Interactor:AddingFragment");
		inHandObject.rigidbody.isKinematic = false;
        handsFull = false;
        inHandObject.transform.parent = null;
        SettingPiece.fragmentsOfZeWorld.Remove(inHandObject.GetComponent<Fragment>());
        //Destroy(inHandObject);
        inHandObject.SetActive(false);
    }
}