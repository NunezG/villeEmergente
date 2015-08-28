using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using RAIN.Core;
using RAIN.Navigation.Targets;
using RAIN.Minds;
using RAIN.Serialization;
using RAIN.Motion;

public class Musicien : MonoBehaviour{


    public RAIN.Memory.BasicMemory tMemory;



    public float soundTimer = 0,minEndSoundTimer=10, maxEndSoundTimer = 15, endSoundTimer,
                waitTimer=0,minEndWaitTimer=10, maxEndWaitTimer = 15, endWaitTimer,
                
                danceTimer=0,endDanceTimer=10;

    public Material material;
    //public AudioSource audioSource;
    //public AudioClip defaultClip;
    private string audioEventName = "";
    public string defaultAudioEventName="";
    //public List<Fragment> fragments = new List<Fragment>();
    public Fragment fragment = null;

    public static GameObject[] allNavTargets = null;
    public List<GameObject> targets = new List<GameObject>();
	public GameObject previousTarget = null;

	public GameObject[] buildings;
    public SceneRange scene;

	public bool isFragmentComplete=false;
	
	public void Awake()
    {
       tag = "NPC";
       // if(targets==null)
       //Debug.Log ("START MUSICIENN");
       int matriculeMusicien = int.Parse(this.gameObject.name.Substring(8));


        if (allNavTargets==null)
            allNavTargets = GameObject.FindGameObjectsWithTag("NavigationTarget");
        foreach (GameObject gObject in allNavTargets)
       {
           if (gObject.name == "NT_Musicien_" + matriculeMusicien)
           {
               targets.Add(gObject);
           }
        }






       //targets = GameObject.FindGame

		//Debug.Log ("FINISH MUSICIENN");

	}

	// Use this for initialization
    public void Start()
    {
        audioEventName = defaultAudioEventName;

        //print("number : " + this.gameObject.name.Substring(8));
        AIRig aiRig = GetComponentInChildren<AIRig>();
        tMemory = aiRig.AI.WorkingMemory as RAIN.Memory.BasicMemory;


        endSoundTimer = (int)Random.Range(minEndSoundTimer, maxEndSoundTimer);
        endWaitTimer = (int)Random.Range(minEndWaitTimer, maxEndWaitTimer);

        this.renderer.material = material;
	}
	
	// Update is called once per frame
    public void Update()
    {
        
        if (soundTimer < endSoundTimer && !tMemory.GetItem<bool>("soundTimerHasEnded"))
        {
            soundTimer = soundTimer + Time.deltaTime;
        }
        else if (!tMemory.GetItem<bool>("soundTimerHasEnded"))
        {
            tMemory.SetItem<bool>("soundTimerHasEnded", true);
        }
        if (soundTimer != 0 && tMemory.GetItem<bool>("soundTimerHasEnded"))
        {
            soundTimer = 0;
            endSoundTimer = (int)Random.Range(minEndSoundTimer, maxEndSoundTimer);
        }
        //----------------------
        if (tMemory.GetItem<bool>("destinationReached"))
        {
            if (waitTimer < endWaitTimer && !tMemory.GetItem<bool>("waitTimerHasEnded"))
            {
                waitTimer = waitTimer + Time.deltaTime;
            }
            else if (!tMemory.GetItem<bool>("waitTimerHasEnded"))
            {
                tMemory.SetItem<bool>("waitTimerHasEnded", true);
            }
            if (waitTimer != 0 && tMemory.GetItem<bool>("waitTimerHasEnded"))
            {
                waitTimer = 0;
                endWaitTimer = (int)Random.Range(minEndWaitTimer, maxEndWaitTimer);
            }
        }
        //---------------------------------
        if (tMemory.GetItem<bool>("isDancing"))
        {
            if (danceTimer < endDanceTimer )
            {
                danceTimer = danceTimer + Time.deltaTime;
            }
            else 
            {
                danceTimer = 0;
                tMemory.SetItem<bool>("isDancing", false);
            }
        }


        /*
        if (isFragmentComplete)
        {
            isFragmentComplete = false;

            for (int i = 0; i < buildings.Length; i++)
            {
                buildings[i].GetComponent<Building>().Down();
            }
		}*/
	}

    public void OpenTheWay()
    {
        for (int i = 0; i < buildings.Length; i++)
        {
            buildings[i].GetComponent<Building>().Down();
        }
    }
    public void ActiveScene()
    {
        scene.gameObject.SetActive(true);
    }

    public void OnAddingFragment(Fragment fragment)
    {
        print("NPC:OnAddingFragment");

        //SetIsFragmentComplete(true);
        SetJustReceivedFragmentComplete(true);
        this.fragment = fragment;
        
        this.renderer.material = fragment.material;
        //this.audioSource.clip = fragment.GetClip();
		this.audioEventName = fragment.GetComponent<InteractibleObject>().soundEevent;


    }

    public void EmitSound()
    {
        //WwiseAudioManager.instance.PlayFiniteEvent(audioEventName, this.gameObject);
        print("Emit Sound");
    }


    public void SetJustReceivedFragmentComplete(bool boolean)
    {
        isFragmentComplete = boolean;
        tMemory.SetItem<bool>("justReceivedFragment", boolean);
    }

    public void SetIsFragmentComplete(bool boolean)
    {
        isFragmentComplete = boolean;
        tMemory.SetItem<bool>("isFragmentComplete", boolean);
    }

    public void SetPlayerIsInRange(bool boolean)
    {
        tMemory.SetItem<bool>("playerIsInRange", boolean);
    }

    public void SetTargetLookAt(GameObject lookAtTarget)
    {
        tMemory.SetItem<GameObject>("lookAtTarget", lookAtTarget);
    }
}
