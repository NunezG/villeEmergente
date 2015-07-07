using UnityEngine;
using System.Collections;

public class LifeScenePlace 
	: MonoBehaviour 
{
	//public LifeSceneRoleNames lifeSceneRoleNames;
	
	public string allowedRole = "none";
	private LifeScenePlace[] otherPlaces;
	
	void OnDrawGizmosSelected()
	{
		if(gameObject.transform.parent != null && gameObject.transform.parent.GetComponentsInChildren<LifeScenePlace>() != null){
			otherPlaces = gameObject.transform.parent.GetComponentsInChildren<LifeScenePlace>();
			for(int i = 0; i < otherPlaces.Length; i++){
				Gizmos.matrix = otherPlaces[i].transform.localToWorldMatrix;
				Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.75f, 0.75f, 0.75f));
			}
		} else
		{
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
		}
	}
	
}
