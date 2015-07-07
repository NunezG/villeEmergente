using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TriggerParameters
{
	public enum TypeTriger
	{
		None = 0,
		Entity = 1,
		Object = 2,
		Time = 3,
		
	};

	[System.Serializable]
	public class ConditionBool
	{
		public string nameBool;
		public bool triggerBool;
	}

	[System.Serializable]
	public class ConditionInt
	{
		public string nameInt;
		public float triggerInt;
	}


	public TypeTriger type ;
	public ConditionBool[] conditionsBool;
	public ConditionInt[] conditionsInt;
	public LifeScenePlace[] Places;
    public GameObject[] SO;
	public float begining = 0;
	public string behavior = "none";
	public GameObject entity = null;
	public bool repetition = false;

	public string ExcluedRole = "none";
	public float repetitionTime = 0;
	public LifeScenePlace Place;
    public float duration = -1;

	

		
	
}

