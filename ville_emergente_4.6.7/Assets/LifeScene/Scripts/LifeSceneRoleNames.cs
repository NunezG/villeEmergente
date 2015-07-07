using UnityEngine;
using mlv;
using System;
using System.Collections;
using System.Collections.Generic;

public class LifeSceneRoleNames 
	: MonoBehaviour 
{
	public string[] Roles;
	public string[] LifeScenes;

	public Entity entity;
	private bool init; 

	public string allowedRole = "none";

	void Awake () 
	{
		init = false;
        entity = GetComponent<Entity>();

	}

	void Update() 
	{
		if (entity.enabled && init == false){
			DynamicObject entityKnowledge = entity.getWorkingKnowledge();
			
			UInt32 EventProp;
            if (!entityKnowledge.hasProperty("LifeScenes.Assigned"))
            {
				EventProp = entityKnowledge.addProperty("LifeScenes.Assigned");
			} else {
                EventProp = entityKnowledge.getProperty("LifeScenes.Assigned");
			}

			UInt32 param;
			entityKnowledge.resize(EventProp, Roles.Length);
			for(int j = 0; j < Roles.Length; j++){
				if(!entityKnowledge.hasChild(EventProp, "Name")){
					param = entityKnowledge.addChild(entityKnowledge.getChild(EventProp, j), "Name");
					entityKnowledge.setString(param, LifeScenes[j]);
				}
				if(!entityKnowledge.hasChild(EventProp, "Role")){
					param = entityKnowledge.addChild(entityKnowledge.getChild(EventProp, j), "Role");
					entityKnowledge.setString(param, Roles[j]);
				}
			}
			init = true;
		}
	}
}
