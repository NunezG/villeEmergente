
using UnityEngine;
using System.Collections;

public class PlacePosition 
	: MonoBehaviour 
{
	//public LifeSceneRoleNames lifeSceneRoleNames;
	
	private PlacePosition[] otherPositions;
	private LifeScenePlace[] otherPlaces;
	private PlacePosition[] otherPos;


	void OnDrawGizmosSelected()
	{
		if(gameObject.transform.parent != null && gameObject.transform.parent.GetComponentsInChildren<PlacePosition>() != null){
			otherPositions = gameObject.transform.parent.GetComponentsInChildren<PlacePosition>();
			for(int i = 0; i < otherPositions.Length; i++)
				Gizmos.DrawWireCube(otherPositions[i].transform.position, new Vector3(0.25f, 0.25f, 0.25f));

			if(gameObject.transform.parent.transform.parent.GetComponentsInChildren<LifeScenePlace>() != null){
				otherPlaces = gameObject.transform.parent.transform.parent.GetComponentsInChildren<LifeScenePlace>();
				for(int i = 0; i < otherPlaces.Length; i++)
					Gizmos.DrawWireCube(otherPlaces[i].transform.position, new Vector3(0.75f, 0.75f, 0.75f));
			}

			if(gameObject.transform.parent.transform.parent.GetComponentsInChildren<PlacePosition>() != null){
				otherPos = gameObject.transform.parent.transform.parent.GetComponentsInChildren<PlacePosition>();
				for(int i = 0; i < otherPos.Length; i++)
					Gizmos.DrawWireCube(otherPos[i].transform.position, new Vector3(0.25f, 0.25f, 0.25f));
			}
		} else
			Gizmos.DrawWireCube(transform.position, Vector3.one);
	}
	
}