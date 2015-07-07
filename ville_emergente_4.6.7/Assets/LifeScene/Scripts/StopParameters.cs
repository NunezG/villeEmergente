using UnityEngine;
using System;

[Serializable]
public class StopParameters
{
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
	
	
	public ConditionBool[] conditionsBool;
	public ConditionInt[] conditionsInt;
	public LifeScenePlace[] Places;
    public GameObject[] SO;

	public float duration = -1;

}


