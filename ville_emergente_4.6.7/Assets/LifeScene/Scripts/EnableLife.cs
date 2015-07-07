using UnityEngine;
using System.Collections;
using mlv;
using System;
using System.Collections.Generic;


public class EnableLife : MonoBehaviour {

    public float wait = 0.5f;
    private Entity entity;
	private bool init; 

	void Awake () 
	{
		init = false;
        entity = GetComponent<Entity>();

	}

	void OnEnable()
	{
		Invoke("StartLife", 0.5f);

	}
	
	void StartLife()
	{
		//zone = GetComponent< ZoneDetection >();
        //new WaitForSeconds(wait);
		GetComponent< mlv.Entity >().enabled = true;
	}

	void Update() 
	{
        if (entity.enabled && init == false)
        {
            DynamicObject entityKnowledge = entity.getWorkingKnowledge();

            UInt32 EventProp;
            if (!entityKnowledge.hasProperty("EnableLife"))
                EventProp = entityKnowledge.addProperty("EnableLife");
            else
                EventProp = entityKnowledge.getProperty("EnableLife");


            UInt32 param;
            if (!entityKnowledge.hasChild(EventProp, "Delay"))
            {
                param = entityKnowledge.addChild(EventProp, "Delay");
                entityKnowledge.setReal(param, wait);
            }
            if (!entityKnowledge.hasChild(EventProp, "Enabled"))
            {
                param = entityKnowledge.addChild(EventProp, "Enabled");
                entityKnowledge.setBool(param, false);
            }
            init = true;

        }
    }
}