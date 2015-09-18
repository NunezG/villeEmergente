using UnityEngine;
using System.Collections;

public class Fragment : MonoBehaviour{

    public Material material;
    //public  AudioSource audioSource;
    //public AudioClip defaultClip;
    //public string audioEventName;
	// Use this for initialization
	//public FragmentType family;
	public FragmentType family;
	private bool justSpawn = true; 

	public string[][] soundString = new string[5][] ;

	
	/*
	public struct soundsFamilies{
		public static string[] electricity = {"electricBuzz"};
		public static string[] liquid = {"waterSplash","softRain", "dropsCardboard"};
		public static string[] urban = {"trafficBy","busyStreet"};
		public static string[] wood = {"trafficBy","busyStreet"};
		public static string[] metal = {"trafficBy","busyStreet"};
	}
*/

    public void Awake()
    {
		soundString [0] = new string[]{"waterSplash","paddle", "dropsCardboard"}; // Liquid
		soundString [1] = new string[]{"waterSplash", "dropsCardboard"}; // Wood
		soundString [2] = new string[]{"thunder", "electricityStatic", "electricityArcing"};						  		// Electricity
		soundString [3] = new string[]{"waterSplash","softRain", "dropsCardboard"}; // Metal
		soundString [4] = new string[]{"trafficBy"}; // Urban

        this.tag = "Fragment";
    }

    public void Start()
    {
		print("FragmentStart: "+ family.ToString()+ " /// "+ family.GetHashCode() +  " /// " + (int)family);
		//randomSoundFromFamily ();
		
		print ("THELENGHT "+ soundString[(int)family].Length);

	//	print ("THELENGHT "+ soundString[0,]);

		
		int familyInt = (int)family;

		//Choose random family
		GetComponent<InteractibleObject>().soundEvent =soundString [familyInt][Random.Range (0,soundString[familyInt].Length)];


		WwiseAudioManager.PlayLoopEvent ("fragment_call_"+family.ToString(), this.gameObject, false);
        //audioSource.clip = defaultClip;
       // this.renderer.material = material;
	}
	
	// Update is called once per frame
    public void Update()
    {
	
	}

	public void Ground()
	{
		WwiseAudioManager.PlayFiniteEvent("lacher_morceau", gameObject);
		WwiseAudioManager.PlayLoopEvent ("fragment_call_"+family.ToString(), gameObject, false);
	}


	public void Drop()
	{
		rigidbody.isKinematic = false;
		transform.parent = null;

		WwiseAudioManager.StopLoopEvent(GetComponent<InteractibleObject>().soundEvent, gameObject);


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

		//if (justSpawn) 
		//{
		WwiseAudioManager.StopLoopEvent ("fragment_call_"+family.ToString(), this.gameObject, false);
		//	justSpawn = false;
		//}
	
		//WwiseAudioManager.instance.PlayFiniteEvent("busy_street_stop", gameObject);	
		WwiseAudioManager.PlayFiniteEvent("prendre_morceau", gameObject);
		WwiseAudioManager.PlayLoopEvent(GetComponent<InteractibleObject>().soundEvent, gameObject);	


		return this.gameObject;
    }



	void randomSoundFromFamily()
	{
		//switch (family) 
	//	{
	//	case (FragmentType.liquid):
	

		//GetComponent<InteractibleObject>().soundEvent = soundString[family.GetHashCode()][Random.Range(0,soundString[family].GetLength())];
			


		//soundsFamilies.urban[Random.Range(0,soundsFamilies.urban.Length)];
		//	break;
		//case (FragmentType.liquid):
		//	GetComponent<InteractibleObject>().soundEvent = mystring[0][Random.Range(0,mystring[0].GetLength())];
			//break;
	//	case (FragmentType.electricity):
	//		GetComponent<InteractibleObject>().soundEvent = somystring[0][Random.Range(0,mystring[0].GetLength())];
		//	break;
	//	default:
		//	break;
		//}
	}

}
