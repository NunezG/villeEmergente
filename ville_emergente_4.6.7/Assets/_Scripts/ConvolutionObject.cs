using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConvolutionObject : MonoBehaviour {

   // public AudioSource audioSource;
   // public InteractibleType type; // par defaut un element du decor

	//public string soundEevent;
	public string switchNumber;
	public Fragment fragment;
	public Material defaultMaterial, activatedMaterial;
	//public string playingSound;

	// Use this for initialization
	public void Start () {
		this.renderer.material = defaultMaterial;
		this.gameObject.layer = 8; // mis dans la layer InteractibleObject pour limiter les raycast avec un layermask
	}
	
	// Update is called once per frame
    public void Update()
    {

	}


	public GameObject OnTouch()
	{
		if (!HasFragment ()) {


			WwiseAudioManager.instance.PlayFiniteEvent ("toucher_element", this.gameObject);
			WwiseAudioManager.instance.PlayFiniteEvent ("Sfx_RI", this.gameObject);

			return null;

		} else 
			return OnPickUp ();
		//soundEevent


	}


	public GameObject OnPickUp()
	{
	
			print("SettingPiece:OnPickUp");
			
			GameObject fragPicked =fragment.OnTouch();
			
			SoundUniverseManager.removeSoundEvent(fragment.gameObject);
			
			//WwiseAudioManager.instance.StopLoopEvent(fragment.GetComponent<InteractibleObject>().soundEevent, fragment.gameObject, true);
			
			fragment = null;
			this.renderer.material = activatedMaterial;
			//audioSource.clip = defaultClip;
			//audioEventName = defaultAudioEventName;
			return fragPicked;
	
	}

	/*
    public void OnInteract(){
        print("Parent OnInteract");
        this.GetComponent<SettingPiece>().OnInteract();
    }
*/

    public void OnAddingFragment(Fragment fragment)
    {
		GetComponent<InteractibleObject> ().soundEvent = fragment.GetComponent<InteractibleObject> ().soundEvent;

		//fragment.transform.parent = this.transform;

		print("fragment.transform.parentfragment.transform.parent: "+ this.name);

		print("SettingPiece:OnAddingFragment");
		this.fragment = fragment;
		//activatedMaterial = fragment.material;
		this.renderer.material = fragment.material;
		//this.audioSource.clip = fragment.GetClip();
		// this.audioEventName = fragment.audioEventName;
		
		//AkSoundEngine.SetRTPCValue ("binaural_to_convolver", 100);
		//fragment.Drop ();
		//AkSoundEngine.SetSwitch("Elements_decor", "Batiment_1", inHandObject.gameObject);
		
		SoundUniverseManager.addSoundEvent (this.gameObject); 
		
		WwiseAudioManager.instance.PlayFiniteEvent("linker_morceau", fragment.gameObject);
		WwiseAudioManager.instance.PlayFiniteEvent(SoundUniverseManager.switchType+switchNumber, fragment.gameObject);
		WwiseAudioManager.instance.PlayLoopEvent (fragment.GetComponent<InteractibleObject>().soundEvent, fragment.gameObject, true);

		
		
		
		if (GetComponent<InteractibleObject>().type == InteractibleType.NPC)
        {
			this.GetComponent<Musicien>().OnAddingFragment(fragment);
        }
       
    }

    public bool HasFragment()
    {
        bool value =false;
        if (fragment != null) { 
            value = true; 
        }
        return value;
    }

}
