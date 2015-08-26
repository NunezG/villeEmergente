using UnityEngine;
using System.Collections;

public class Fragment : MonoBehaviour{


    public Material material;
    //public  AudioSource audioSource;
    //public AudioClip defaultClip;
    public string audioEventName;
	// Use this for initialization
    public void Awake()
    {
        this.tag = "Fragment";
    }

    public void Start()
    {
        print("FragmentStart");
        //audioSource.clip = defaultClip;
        this.renderer.material = material;
	}
	
	// Update is called once per frame
    public void Update()
    {
	
	}

	public void Drop()
	{
		rigidbody.isKinematic = false;
		transform.parent = null;

		WwiseAudioManager.instance.PlayFiniteEvent("busy_street_stop", gameObject);
		WwiseAudioManager.instance.PlayFiniteEvent("lacher_morceau", gameObject);
		
		//AkSoundEngine.SetRTPCValue ("binaural_to_convolver", 0);
		//AkSoundEngine.SetSwitch("Elements_decor", "Batiment_4", gameObject);
		//WwiseAudioManager.instance.PlayFiniteEvent("switch_bat1", this.gameObject);
	}
		
	public void AnswerTheCall()
    {
        print("Fragment:AnswerTheCall");
       // Play();
    }

    public GameObject OnPickUp()
    {
        print("Fragment:OnPickUp");
        this.gameObject.SetActive(true);

		//WwiseAudioManager.instance.PlayFiniteEvent("busy_street_stop", gameObject);	
		WwiseAudioManager.instance.PlayFiniteEvent("prendre_morceau", gameObject);
		WwiseAudioManager.instance.PlayFiniteEvent("busy_street_play", gameObject);	

		return this.gameObject;
    }

}
