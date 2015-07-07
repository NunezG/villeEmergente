using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class EntityRolesParameters 
{
	public LifeSceneRoleNames[] Entities;
    //public LifeSceneRoleNames[] SelectEntities;
    public GameObject[] EntitiesAff;
   // public GameObject[] SelectEntitiesAff;
    public LifeSceneRoleNames newEntity = null; 
	public int entitySize = -1;
    public string buttonText = "Lock Inspector and Select Characters";



}


