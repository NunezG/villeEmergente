using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractibleObject : MonoBehaviour {

   // public AudioSource audioSource;
    public InteractibleType type; // par defaut un element du decor

	public string soundEevent;

	//public string playingSound;

	// Use this for initialization
	public void Start () {

        this.gameObject.layer = 8; // mis dans la layer InteractibleObject pour limiter les raycast avec un layermask
	}
	
	// Update is called once per frame
    public void Update()
    {

	}


    public GameObject OnPickUp()
    {
        print("Parent OnPickUp");
        if (type == InteractibleType.Fragment)
        {
            return this.GetComponent<Fragment>().OnPickUp();
        }
        else if (type == InteractibleType.SettingPiece)
        {
            return this.GetComponent<SettingPiece>().OnPickUp();
        }

        return null;
    }
	/*
    public void OnInteract(){
        print("Parent OnInteract");
        this.GetComponent<SettingPiece>().OnInteract();
    }
*/

    public void OnAddingFragment(Fragment fragment)
    {
		soundEevent = fragment.GetComponent<InteractibleObject> ().soundEevent;

		//fragment.transform.parent = this.transform;

		print("fragment.transform.parentfragment.transform.parent: "+ this.name);


        if (type == InteractibleType.NPC)
        {
			this.GetComponent<Musicien>().OnAddingFragment(fragment);
        }
        else if (type == InteractibleType.SettingPiece)
        {
            this.GetComponent<SettingPiece>().OnAddingFragment(fragment);
        }
    }

    public bool HasFragment()
    {
        bool value =false;
        if (this.GetComponent<SettingPiece>().fragment != null) { 
            value = true; 
        }
        return value;
    }

}
