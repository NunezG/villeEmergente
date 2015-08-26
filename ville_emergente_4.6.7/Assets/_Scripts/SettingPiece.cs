using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SettingPiece : MonoBehaviour{

    public static List<Fragment> fragmentsOfZeWorld = new List<Fragment>(); // tout les fragments 
    public float fragmentsCallRadius = 25;

    public Material defaultMaterial, activatedMaterial;
    //public AudioSource audioSource;
   // public AudioClip defaultClip;
    private string audioEventName = "";
    public string defaultAudioEventName = "";
    public Fragment fragment;
    public bool isPlayingInteract = false;

	private string switchAtmo = "switch_atmo";
	public string switchType;
	private string switchDark = "switch_dark";
	public string switchNumber;


	// Use this for initialization
    public void Awake()
    {
        this.tag = "SettingPiece";
    }
    public void Start()
    {
		switchType = switchAtmo;

        audioEventName = defaultAudioEventName;
        this.renderer.material = defaultMaterial;
        if (fragmentsOfZeWorld.Count == 0)
        {
            GameObject[] fragsTab = GameObject.FindGameObjectsWithTag("Fragment");
            for (int i = 0; i < fragsTab.Length; i++)
            {
                fragmentsOfZeWorld.Add(fragsTab[i].GetComponent<Fragment>());
            }
        }
	}

	// Update is called once per frame
    public void Update()
    {

		if (Input.GetKeyDown(KeyCode.Space)) {

			if (switchType != switchDark)
			switchType = switchDark;
			else switchType = switchAtmo;

		}

        if (hasBeenActivated && !isPlayingInteract) // si le decor a ete active, et que son son n'est plus en en train de jouer, 
        {

            print("has been activated and finished playing");
            for (int i = 0; i < fragmentsOfZeWorld.Count; i++)
            {
                float distance = (fragmentsOfZeWorld[i].transform.position - this.transform.position).magnitude;
                if (distance < fragmentsCallRadius)
                {
                    fragmentsOfZeWorld[i].AnswerTheCall(); // appelle le premier fragment a portee
                    break;
                }
            }
            hasBeenActivated = false;
        }
	}

    bool hasBeenActivated = false;
    public void OnInteract()
    {
        print("SettingPiece:OnInteract ");
        isPlayingInteract = true;
        WwiseAudioManager.instance.PlayFiniteEvent(audioEventName,this.gameObject, OnInteractCallBack);
        hasBeenActivated = true;

        this.renderer.material = activatedMaterial;
    }

    void OnInteractCallBack(object in_cookie, AkCallbackType in_type, object in_info)
    {

        if (in_type == AkCallbackType.AK_EndOfEvent)
        {
            isPlayingInteract = false;
            print("OnInteractCallBack");
        }
    }

    public void OnAddingFragment(Fragment fragment)
    {
        print("SettingPiece:OnAddingFragment");
        this.fragment = fragment;
        //activatedMaterial = fragment.material;
        this.renderer.material = fragment.material;
        //this.audioSource.clip = fragment.GetClip();
        this.audioEventName = fragment.audioEventName;

		//AkSoundEngine.SetRTPCValue ("binaural_to_convolver", 100);
		fragment.Drop ();
		//AkSoundEngine.SetSwitch("Elements_decor", "Batiment_1", inHandObject.gameObject);

		WwiseAudioManager.instance.PlayFiniteEvent(switchType+switchNumber, fragment.gameObject);
		WwiseAudioManager.instance.PlayFiniteEvent("busy_street_convolver_play", fragment.gameObject);
		
    }

    public GameObject OnPickUp()
    {
		print("SettingPiece:OnPickUp");

		GameObject fragPicked =fragment.OnPickUp();

		WwiseAudioManager.instance.PlayFiniteEvent("busy_street_convolver_stop", fragPicked);

		fragment = null;
        this.renderer.material = activatedMaterial;
        //audioSource.clip = defaultClip;
        audioEventName = defaultAudioEventName;
        return fragPicked;
    }
}
