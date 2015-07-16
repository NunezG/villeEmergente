using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC : MonoBehaviour{

    public float timer = 0,minEndTimer=10, maxEndTimer = 15, endTimer;

    public Material material;
    public AudioSource audioSource;
    public AudioClip defaultClip;
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
            audioSource.Play();
        }
	}
    
    public void OnAddingFragment(Fragment fragment)
    {
        print("NPC:OnAddingFragment");
        this.fragment = fragment;
        
        this.renderer.material = fragment.material;
        this.audioSource.clip = fragment.GetClip();
    }
}
