using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SettingPiece : MonoBehaviour{

    public static List<Fragment> fragmentsOfZeWorld = new List<Fragment>(); // tout les fragments 
    public float fragmentsCallRadius = 25;

    
    //public AudioSource audioSource;
   // public AudioClip defaultClip;
   // private string audioEventName = "";
    //public string defaultAudioEventName = "";
    public bool isPlayingInteract = false;


	//public string switchT;


	// Use this for initialization
    public void Awake()
    {
        this.tag = "SettingPiece";
    }
    public void Start()
	{
        //audioEventName = defaultAudioEventName;
    
        if (fragmentsOfZeWorld.Count == 0)
        {
            GameObject[] fragsTab = GameObject.FindGameObjectsWithTag("Fragment");
            for (int i = 0; i < fragsTab.Length; i++)
            {
                fragmentsOfZeWorld.Add(fragsTab[i].GetComponent<Fragment>());
            }
        }
	}

	public GameObject OnTouch()
	{
		
		return GetComponent<ConvolutionObject> ().OnTouch ();
		
	}

	// Update is called once per frame
    public void Update()
    {
		//switchT = SoundUniverseManager.switchType;

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
   /* public void OnInteract()
    {
        print("SettingPiece:OnInteract ");
        isPlayingInteract = true;
		WwiseAudioManager.instance.PlayFiniteEvent("prendre_morceau",this.gameObject, OnInteractCallBack);
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
*/
   



}
