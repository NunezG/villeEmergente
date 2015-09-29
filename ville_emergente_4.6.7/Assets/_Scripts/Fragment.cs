using UnityEngine;
using System.Collections;

public class Fragment : MonoBehaviour{

    public Material material;
	public FragmentType family;
	private bool justSpawn = true;
    public bool desactivateOnStart = false;

	public string[][] soundString = new string[5][] ;

    public void Awake()
    {
		//sons de chaque famille
		soundString [0] = new string[]{"waterSplash","paddle", "dropsCardboard"}; // Liquid
		soundString [1] = new string[]{"hammer", "logCrack", "woodFall"}; // Wood
		soundString [2] = new string[]{"thunder", "electricityStatic", "electricityArcing"};// Electricity
		soundString [3] = new string[]{"metalCreak","metalRattle", "anvil"}; // Metal
		soundString [4] = new string[]{"buildingSite", "train", "brewing"}; // Urban

        this.tag = "Fragment";
        int familyInt = (int)family;

        //Choose random family
        GetComponent<InteractibleObject>().soundEvent = soundString[familyInt][Random.Range(0, soundString[familyInt].Length)];
    }

    public void Start()
    {
		//appel du fragment
		WwiseAudioManager.PlayLoopEvent ("fragment_call_"+family.ToString(), this.gameObject, false);

        if (desactivateOnStart)
        {
            this.gameObject.SetActive(false);
        }
	}

	//Quand le fragment est posé au sol
	public void Ground()
	{
		WwiseAudioManager.PlayFiniteEvent("lacher_morceau", gameObject);
		//relance appel du fragment
		WwiseAudioManager.PlayLoopEvent ("fragment_call_"+family.ToString(), gameObject, false);
	}

	//Quand le fragment est laché
	public void Drop()
	{
		rigidbody.isKinematic = false;
		transform.parent = null;

		//stoppe la convolution
		WwiseAudioManager.StopLoopEvent(GetComponent<InteractibleObject>().soundEvent, gameObject);

        GetComponent<levitation>().IsHeld(false);
	}

	//Ramassage du fragment
	public GameObject OnTouch()
    {		
		this.gameObject.SetActive (true);

        GetComponent<levitation>().enabled = true;
        GetComponent<levitation>().IsHeld(true);

		//stoppe appel
		WwiseAudioManager.StopLoopEvent ("fragment_call_"+family.ToString(), this.gameObject, false);
	
		WwiseAudioManager.PlayFiniteEvent("prendre_morceau", gameObject);

		//lance convolution du fragment
		WwiseAudioManager.PlayLoopEvent(GetComponent<InteractibleObject>().soundEvent, gameObject);	
		
		return this.gameObject;
    }


}
