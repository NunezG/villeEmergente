using UnityEngine;
using System.Collections;

public class ObjectInteractor : MonoBehaviour {

    public GameObject hitObject,inHandPosition,inHandObject;
    public bool handsFull = false;
    public float range;


	private CursorManager Cm;

	// Use this for initialization
	void Start () 
	{
		Cm = GameObject.Find ("Canvas").GetComponentInChildren<CursorManager> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		hitObject = null;
		
		RaycastHit hit;

		Cm.setNormalCursor();

		InteractibleObject interacObj = null;

		if (Physics.Raycast (Camera.main.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2, 0)), out hit, range, 1 << 8)) {
			hitObject = hit.collider.gameObject; // on recupere l'objet vise
			interacObj = hitObject.GetComponent<InteractibleObject> (); // on recupere sa composante InteractibleObject

			if (handsFull
				&& (interacObj.type == InteractibleType.SettingPiece 
				|| interacObj.type == InteractibleType.NPC)
				&& !interacObj.GetComponent<ConvolutionObject> ().HasFragment ()) {
				Cm.setFragmentCursor ();
			} else if 
				(!handsFull
				&& ((interacObj.type == InteractibleType.Fragment 
				&& (interacObj.transform.parent == null || interacObj.transform.parent.name != "inHandPosition"))
				|| (interacObj.type == InteractibleType.SettingPiece 
				&& interacObj.GetComponent<ConvolutionObject> ().HasFragment ()))) { //µRamassable
				Cm.setInteractibleCursor ();
			}
		}

			//Bouton action
			if (Input.GetButtonDown ("Action")) 
			{
				if (Cm.cursorT == CursorType.interactible) { // si on a rien en main et qu'on vise un objet rammassable

						print ("try PickUpObject");
						PickUpObject (interacObj.OnTouch ());
				}
				else if (Cm.cursorT == CursorType.fragment) { // si on a un objet en main et qu'on vise un NPC
					 // ou un batiment vide
					ConvolutionObject convolObj = interacObj.GetComponent<ConvolutionObject> ();

					//ajout de l'objet en main a l'objet vise
					print ("ajout de l'objet en main a l'objet vise");
					// WwiseAudioManager.instance.PlayFiniteEvent("linker_morceau", this.gameObject);
					Fragment fragment = inHandObject.GetComponent<Fragment> ();
					AddingFragment ();
					convolObj.OnAddingFragment (fragment);
					fragment.gameObject.SetActive (false);
						 
				} else if (handsFull){  // sinon, si on a juste un objet en main et qu'on ne vise pas un NPC
					
					//on laisse tomber l'objet en main
					DropInHandObject ();
				}else {

					Cm.setFailCursor ();
				}
			}
		


	/*	else
		{
			if (handsFull)
			{
				Cm.setNormalCursor();

				if (Input.GetButtonDown("Action")) 
				{
					DropInHandObject();
				}
			}
		}*/
    }

    public void PickUpObject(GameObject toPickUp)
    {
		if (toPickUp != null) {
			print("Interactor:PickUpObject");
			inHandObject = toPickUp;
			inHandObject.transform.parent = inHandPosition.transform;
			inHandObject.transform.position = inHandPosition.transform.position;
			inHandObject.transform.rotation = inHandPosition.transform.rotation;
			inHandObject.rigidbody.isKinematic = true;
			handsFull = true;   
			WwiseAudioManager.PlayFiniteEvent("interactions_simples_motion", this.gameObject);
		}	
	}

    public void DropInHandObject()
    { 
		if (inHandObject != null)
        {
			print("Interactor:DropInHandObject");
			WwiseAudioManager.PlayFiniteEvent("interactions_simples_motion", this.gameObject);
			inHandObject.GetComponent<Fragment>().Drop();
			     
			if (hitObject == null){
				inHandObject.GetComponent<Fragment>().Ground();
			}

            handsFull = false;
            inHandObject = null;
        }
	}

    public void AddingFragment()
    {
		print("Interactor:AddingFragment");
		inHandObject.rigidbody.isKinematic = false;
       // handsFull = false;
        inHandObject.transform.parent = null;
        SettingPiece.fragmentsOfZeWorld.Remove(inHandObject.GetComponent<Fragment>());
        //Destroy(inHandObject);
		DropInHandObject ();
    }
}