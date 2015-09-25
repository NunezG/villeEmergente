using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConvolutionObject : MonoBehaviour {

   // public AudioSource audioSource;
   // public InteractibleType type; // par defaut un element du decor

	//public string soundEevent;
	public Fragment fragment;
	private Material defaultMaterial;//, activatedMaterial;
	//public string playingSound;
	public string switchName;


	// Use this for initialization
	public void Start () {
		//this.renderer.material = defaultMaterial;
		this.gameObject.layer = 8; // mis dans la layer InteractibleObject pour limiter les raycast avec un layermask
		defaultMaterial = this.GetComponentInChildren<Renderer> ().material;
        if (fragment != null)
        {
            OnAddingFragment(fragment);
        }
	}
	
	// Update is called once per frame
    public void Update()
    {

	}


	public GameObject OnTouch()
	{
		if (!HasFragment ()) {

			WwiseAudioManager.PlayFiniteEvent ("toucher_element", this.gameObject);

			if (GetComponent<InteractibleObject>().type == InteractibleType.SettingPiece) {

				WwiseAudioManager.PlayFiniteEvent(SoundUniverseManager.switchType+"RI"+switchName, this.gameObject);

				WwiseAudioManager.PlayFiniteEvent ("Sfx_RI", this.gameObject);
			}

			return null;

		} else 
			return OnPickUp ();
		//soundEevent


	}

	public GameObject OnPickUp()
	{
		print("SettingPiece:OnPickUp");
			
		WwiseAudioManager.StopLoopEvent(this.GetComponent<InteractibleObject>().soundEvent, this.gameObject, true);
		WwiseAudioManager.PlayFiniteEvent ("convolution_wet_to0", this.gameObject);

		GameObject fragPicked =fragment.OnTouch();
			
		SoundUniverseManager.removeSoundEvent(this.gameObject);
			
		fragment = null;
		this.GetComponentInChildren<Renderer>().material = defaultMaterial;
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
        this.GetComponent<Blend>().enabled = false;
		this.GetComponent<InteractibleObject> ().soundEvent = fragment.GetComponent<InteractibleObject> ().soundEvent;

		//fragment.transform.parent = this.transform;
		
		//print("SettingPiece:OnAddingFragment:" + GetComponent<InteractibleObject>().type);
		this.fragment = fragment;
		//activatedMaterial = fragment.material;
		this.GetComponentInChildren<Renderer>().material = fragment.material;
		//this.audioSource.clip = fragment.GetClip();
		// this.audioEventName = fragment.audioEventName;

		//AkSoundEngine.SetRTPCValue ("binaural_to_convolver", 100);
		//fragment.Drop ();
		//AkSoundEngine.SetSwitch("Elements_decor", "Batiment_1", inHandObject.gameObject);
	
		SoundUniverseManager.addSoundEvent (this.gameObject); 
		
		WwiseAudioManager.PlayFiniteEvent("linker_morceau", this.gameObject);
	
		WwiseAudioManager.PlayFiniteEvent(SoundUniverseManager.switchType+switchName, this.gameObject);

		WwiseAudioManager.PlayLoopEvent (fragment.GetComponent<InteractibleObject>().soundEvent, this.gameObject, true);
		WwiseAudioManager.PlayFiniteEvent ("convolution_wet_to100", this.gameObject);
		
		
		
		if (GetComponent<InteractibleObject>().type == InteractibleType.NPC)
        {
			this.GetComponent<Musicien>().OnAddingFragment(fragment);
			GetComponentInChildren<AudioEventManager>().SounStopdIdle();
			GetComponentInChildren<AudioEventManager>().idleSound = false; 
			
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
