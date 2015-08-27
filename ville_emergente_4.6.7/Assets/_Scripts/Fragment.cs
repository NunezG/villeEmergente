using UnityEngine;
using System.Collections;

public enum FragmentType { Liquid, Urban, Electric };


public struct soundsFamilies{
	public static string[] electric = {"electricBuzz"};
	public static string[] urban = {"waterSplash","softRain", "dropsCardboard"};
	public static string[] liquid = {"trafficBy","busyStreet"};
}



public class Fragment : MonoBehaviour{

    public Material material;
    //public  AudioSource audioSource;
    //public AudioClip defaultClip;
    //public string audioEventName;
	// Use this for initialization
	//public FragmentType family;
	public FragmentType family;
	public string soundEevent;

    public void Awake()
    {
        this.tag = "Fragment";
    }

    public void Start()
    {
        print("FragmentStart");
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

		WwiseAudioManager.instance.StopLoopEvent(soundEevent, gameObject);


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
		randomSoundFromFamily ();

		//WwiseAudioManager.instance.PlayFiniteEvent("busy_street_stop", gameObject);	
		WwiseAudioManager.instance.PlayFiniteEvent("prendre_morceau", gameObject);
		WwiseAudioManager.instance.PlayLoopEvent(soundEevent, gameObject);	


		return this.gameObject;
    }


	void randomSoundFromFamily()
	{
		switch (family) 
		{
		case (FragmentType.Urban):
			soundEevent = soundsFamilies.urban[Random.Range(0,soundsFamilies.urban.Length-1)];
			break;
		case (FragmentType.Liquid):
			soundEevent = soundsFamilies.liquid[Random.Range(0,soundsFamilies.liquid.Length-1)];
			break;
		case (FragmentType.Electric):
			soundEevent = soundsFamilies.electric[Random.Range(0,soundsFamilies.electric.Length-1)];
			break;
		default:
			break;
		}
	}

}
