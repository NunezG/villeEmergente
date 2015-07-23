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


					WwiseAudioManager.instance.PlayFiniteEvent("switch_bat3", this.gameObject);
					print("wat?????????????????????????");

					
					AkSoundEngine.SetRTPCValue ("binaural_to_convolver", 100);
					print("endddddddddddddd");

					
					
                }
                else if (!handsFull && (interacObj.type == InteractibleType.Fragment // si on a rien en main et qu'on vise un objet rammassable
                    || (interacObj.type == InteractibleType.SettingPiece && interacObj.HasFragment()))) // ou un batiment avec un fragment
                {
                    print("on ramasse l'objet");
                    //on ramasse l'objet
                    PickUpObject(interacObj.OnPickUp());
					print("on ramasse UNE SECONDE FOIS");

					//WwiseAudioManager.instance.PlayFiniteEvent("busy_street_convolver", this.gameObject);

					//WwiseAudioManager.instance.PlayFiniteEvent("busy_street_play", this.gameObject);
					print("AFTERPARTYYYYYYYYYY");
					//WwiseAudioManager.instance.PlayFiniteEvent("fuck le_morceau", this.gameObject);

					WwiseAudioManager.instance.PlayFiniteEvent("prendre_morceau", this.gameObject);

					WwiseAudioManager.instance.PlayFiniteEvent("switch_bat1", this.gameObject);
					AkSoundEngine.SetRTPCValue ("binaural_to_convolver", 0);
					print("PLAY LACHE MORCEAU");
					WwiseAudioManager.instance.PlayFiniteEvent("busy_street_convolver", this.gameObject);


                }
                else if (!handsFull && (interacObj.type == InteractibleType.SettingPiece && !interacObj.HasFragment())) // si on a rien en main et qu'on vise un element de decor vide
                {
                    print("on declenche l'interaction avec cet objet");
                    WwiseAudioManager.instance.PlayFiniteEvent("toucher_element", this.gameObject);
                    //print("(!handsFull && interacObj.type == InteractibleType.SettingPiece)");
                    //on declenche l'interaction avec cet objet
                    interacObj.OnInteract();
                }
                else if (handsFull) // sinon, si on a juste un objet en main et qu'on ne vise pas un NPC
                {
                    print("on laisse tomber l'objet en main");
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
					//AkSoundEngine.SetSwitch("Elements_decor", "Batiment_4", gameObject);

					
					//AkSoundEngine.SetSwitch("Elements_decor", "Batiment_1", gameObject);

					//WwiseAudioManager.instance.PlayFiniteEvent("lacher_morceau", this.gameObject);
                //    WwiseAudioManager.instance.PlayFiniteEvent("lacher_morceau", this.gameObject);
					print("PLAY LACHE MORCEAU enddd");
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

			print(" PLAAAYY Interactor:DropInHandObject");

            inHandObject.GetComponent<Fragment>().Play();
			print(" end  PLAAAYY Interactor:DropInHandObject");

            inHandObject.rigidbody.isKinematic = false;
            handsFull = false;
            inHandObject.transform.parent = null;
            inHandObject = null;

        }
		WwiseAudioManager.instance.PlayFiniteEvent("busy_street_stop", this.gameObject);

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