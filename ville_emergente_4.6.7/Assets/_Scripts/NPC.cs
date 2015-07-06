using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC : InteractibleObject {

    public float timer = 0, maxTimer = 15, endTimer;


    public List<InteractibleObject> fragments = new List<InteractibleObject>();

	// Use this for initialization
    public void Start()
    {
        endTimer = (int)Random.Range(0, 15);
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
            endTimer = (int)Random.Range(0, 15);
            //audioSource.Play();
        }
	}
    
    public void OnAddingFragment(GameObject fragment)
    {
        fragments.Add(fragment.GetComponent<InteractibleObject>());
    }

}
