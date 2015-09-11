using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractibleObject : MonoBehaviour {

   // public AudioSource audioSource;
    public InteractibleType type; // par defaut un element du decor
	public string soundEvent = "";


	//public string playingSound;

	// Use this for initialization
	public void Start () {

        this.gameObject.layer = 8; // mis dans la layer InteractibleObject pour limiter les raycast avec un layermask
	}
	

	public GameObject OnTouch()
	{
		if (type == InteractibleType.NPC // si on a un objet en main et qu'on vise un NPC
			|| (type == InteractibleType.SettingPiece)) { // ou un batiment vide
			return GetComponent<ConvolutionObject>().OnTouch();


		} else if (type == InteractibleType.Fragment) { 
			//GetComponent<Fragment>().randomSoundFromFamily ();

			return GetComponent<Fragment>().OnTouch();

		}

		return null;
	}
	

		
		/*
    public void OnInteract(){
        print("Parent OnInteract");
        this.GetComponent<SettingPiece>().OnInteract();
    }
*/

}
