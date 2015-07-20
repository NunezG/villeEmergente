using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class navigationScript : MonoBehaviour {

	public List<GameObject> targets=null;



	// Use this for initialization
	void Start () {


		GameObject[] targetList = GameObject.FindGameObjectsWithTag("NavigationTarget");
		int i = 0;

		while (i< targetList.Length)
		{
			int rand = Random.Range(0,targetList.Length-1);


			if (!targetList[rand].Equals(this.gameObject) && !targets.Contains(targetList[rand]))
			{
				targets.Add (targetList[rand]);
				i++;
			}

		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
