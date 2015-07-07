using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class PlaceGeneration 
{
	public Transform Position;
	public int PlaceNumber = 0;
	public float Alignment = 0;
	public float LeaderDistance = 0;
	public float AgentDistance = 0;
	public float AgentOrientation = 0;
    public int alignNumber = 0;
	
}

[System.Serializable]
public class PlaceParameters
{
	public LifeScenePlace[] UnityPlaces;
    public int Number;
	
}

[System.Serializable]
public class FormationParameters
{

    public PositioningParameters[] Formations;
    public float distanceAmbient = 1;

}

[System.Serializable]
public class PositioningParameters
{
	public enum TypeFormation
	{
		None = 0,
		Queue = 1,
		Circle = 2,
		
	};

	public TypeFormation TypeForm = 0;
	//public string Behavior = "none";
	public string ConcernedRole = "none";
	public bool GenerationOfPlace = false;
	public PlaceGeneration PlaceGeneration;
	public bool UnityPlace =  true;
	public LifeScenePlace [] UnityPlaces;
    public string fullBehavior = "none";
    public int number;

}
