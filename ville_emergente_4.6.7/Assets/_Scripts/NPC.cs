using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using RAIN.Core;
using RAIN.Navigation.Targets;
using RAIN.Minds;
using RAIN.Serialization;
using RAIN.Motion;

public class NPC : MonoBehaviour{


    public RAIN.Memory.BasicMemory tMemory;



    public float timer = 0,minEndTimer=10, maxEndTimer = 15, endTimer;

    public Material material;
    //public AudioSource audioSource;
    //public AudioClip defaultClip;
    private string audioEventName = "";
    public string defaultAudioEventName="";
    //public List<Fragment> fragments = new List<Fragment>();
    public Fragment fragment = null;

    public static GameObject[] targets=null;
    public void Awake()
    {
       tag = "NPC";
        if(targets==null)
            targets = GameObject.FindGameObjectsWithTag("NavigationTarget");
    }

	// Use this for initialization
    public void Start()
    {
        audioEventName = defaultAudioEventName;

        AIRig aiRig = GetComponentInChildren<AIRig>();
        tMemory = aiRig.AI.WorkingMemory as RAIN.Memory.BasicMemory;


        endTimer = (int)Random.Range(minEndTimer, maxEndTimer);

        this.renderer.material = material;
	}
	
	// Update is called once per frame
    public void Update()
    {
        if (timer < endTimer)
        {
            timer = timer + Time.deltaTime;
        }
        else
        {
            timer = 0;
            endTimer = (int)Random.Range(minEndTimer, maxEndTimer);
            EmitSound();
        }
	}
    
    public void OnAddingFragment(Fragment fragment)
    {
        print("NPC:OnAddingFragment");
        this.fragment = fragment;
        
        this.renderer.material = fragment.material;
        //this.audioSource.clip = fragment.GetClip();
        this.audioEventName = fragment.audioEventName;
    }

    public void EmitSound()
    {
        WwiseAudioManager.instance.PlayFiniteEvent(audioEventName, this.gameObject);
        print("Emit Sound");
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
