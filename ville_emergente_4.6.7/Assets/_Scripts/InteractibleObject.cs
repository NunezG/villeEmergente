using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Classe pour généraliser les interactions du joueur et de certains PNJ entre eux et avec le décor
public class InteractibleObject : MonoBehaviour {

    public InteractibleType type; //type de l'objet interactible par defaut un element du decor
	public string soundEvent = "";



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

}
