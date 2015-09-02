using UnityEngine;
using System.Collections;

public enum FragmentType { Liquid, Urban, Electric };


public struct soundsFamilies{
	public static string[] electric = {"electricBuzz"};
	public static string[] liquid = {"waterSplash","softRain", "dropsCardboard"};
	public static string[] urban = {"trafficBy","busyStreet"};
}


public class Fragment : MonoBehaviour{

    public Material material;
    //public  AudioSource audioSource;
    //public AudioClip defaultClip;
    //public string audioEventName;
	// Use this for initialization
	//public FragmentType family;
	public FragmentType family;
	public bool justSpawn = true;

    public void Awake()
    {
        this.tag = "Fragment";
    }

    public void Start()
    {
        print("FragmentStart");
		randomSoundFromFamily ();
		WwiseAudioManager.instance.PlayLoopEvent ("fragment_call", this.gameObject, false);
        //audioSource.clip = defaultClip;
       // this.renderer.material = material;
	}
	
	// Update is called once per frame
    public void Update()
    {
	
	}

	public void Drop()
	{
		rigidbody.isKinematic = false;
		transform.parent = null;

		WwiseAudioManager.instance.StopLoopEvent(GetComponent<InteractibleObject>().soundEvent, gameObject);


		//AkSoundEngine.SetRTPCValue ("binaural_to_convolver", 0);
		//AkSoundEngine.SetSwitch("Elements_decor", "Batiment_4", gameObject);
		//WwiseAudioManager.instance.PlayFiniteEvent("switch_bat1", this.gameObject);
	}
		
	public void AnswerTheCall()
    {
        print("Fragment:AnswerTheCall");
       // Play();
    }

	public GameObject OnTouch()
    {
		print ("Fragment:OnPickUp");
		this.gameObject.SetActive (true);

		if (justSpawn) 
		{
			WwiseAudioManager.instance.StopLoopEvent ("fragment_call", this.gameObject, false);
			justSpawn = false;
		}
	
		//WwiseAudioManager.instance.PlayFiniteEvent("busy_street_stop", gameObject);	
		WwiseAudioManager.instance.PlayFiniteEvent("prendre_morceau", gameObject);
		WwiseAudioManager.instance.PlayLoopEvent(GetComponent<InteractibleObject>().soundEvent, gameObject);	


		return this.gameObject;
    }



	void randomSoundFromFamily()
	{
		switch (family) 
		{
		case (FragmentType.Urban):
			GetComponent<InteractibleObject>().soundEvent = soundsFamilies.urban[Random.Range(0,soundsFamilies.urban.Length)];
			break;
		case (FragmentType.Liquid):
			GetComponent<InteractibleObject>().soundEvent = soundsFamilies.liquid[Random.Range(0,soundsFamilies.liquid.Length)];
			break;
		case (FragmentType.Electric):
			GetComponent<InteractibleObject>().soundEvent = soundsFamilies.electric[Random.Range(0,soundsFamilies.electric.Length)];
			break;
		default:
			break;
		}
	}

}
