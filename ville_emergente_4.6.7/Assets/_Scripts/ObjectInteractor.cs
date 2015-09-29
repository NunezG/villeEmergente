using UnityEngine;
using System.Collections;

//Classe permettant au joueur d'intéragir avec l'environnement et les PNJs
public class ObjectInteractor : MonoBehaviour {

    public GameObject hitObject, // référence pour l'objet touché par le raycast
                    inHandPosition,inHandObject; // position pour l'objet ramassé, référence pour l'objet ramassé 
    public bool handsFull = false; // variable pour status main vide/pleine
    public float range; // portée de l'interaction 


	private CursorManager Cm; // référence au gestionnaire de changement d'état du curseur

	// Use this for initialization
	void Start () 
	{
		Cm = GameObject.Find ("Canvas").GetComponentInChildren<CursorManager> (); // récupération du composant
	}
	
	// Update is called once per frame
	void Update () 
	{
		hitObject = null;
		
		RaycastHit hit;

		Cm.setNormalCursor();

		InteractibleObject interacObj = null;  

		if (Physics.Raycast (Camera.main.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2, 0)), out hit, range, 1 << 8)) { // Raycast sur la layer des 
			hitObject = hit.collider.gameObject; // on recupere l'objet vise
			interacObj = hitObject.GetComponent<InteractibleObject> (); // on recupere sa composante InteractibleObject

			if (handsFull
				&& (interacObj.type == InteractibleType.SettingPiece 
				|| interacObj.type == InteractibleType.NPC)
				&& !interacObj.GetComponent<ConvolutionObject> ().HasFragment ()) { // si on a un fragment et qu'on clique sur un batiment ou un NPC qui n'en a pas
				Cm.setFragmentCursor (); // on met le fragment correspondant
			} else if 
				(!handsFull
				&& ((interacObj.type == InteractibleType.Fragment 
				&& (interacObj.transform.parent == null || interacObj.transform.parent.name != "inHandPosition"))
				|| (interacObj.type == InteractibleType.SettingPiece ))) { // Si on a les mains vides et qu'on vise un fragment ou un batiment
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
    }
    // Ramasse l'objet toPickUp
    public void PickUpObject(GameObject toPickUp)
    {
		if (toPickUp != null) { 
			print("Interactor:PickUpObject");
            // on racrroche toPickUp a inHandPosition dans la hiérarchie
			inHandObject = toPickUp; 
			inHandObject.transform.parent = inHandPosition.transform;
			inHandObject.transform.position = inHandPosition.transform.position;
			inHandObject.transform.rotation = inHandPosition.transform.rotation;
			inHandObject.rigidbody.isKinematic = true;
			handsFull = true;   
			WwiseAudioManager.PlayFiniteEvent("interactions_simples_motion", this.gameObject);// on joue le son correspondant
		}	
	}


    public void AddingFragment()
    {
        print("Interactor:AddingFragment");
        inHandObject.rigidbody.isKinematic = false;
        inHandObject.transform.parent = null;
        DropInHandObject();
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

}