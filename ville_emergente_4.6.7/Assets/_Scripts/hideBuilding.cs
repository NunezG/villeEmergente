using UnityEngine;
using System.Collections;

public class hideBuilding : MonoBehaviour {

	private float lastPos = 0;

	 


	// Use this for initialization
	void Start () {
		//lastPos = transform.position.y + 500.0f;
	}
	
	// Update is called once per frame
	void Update () {
	
		//transform.position = Vector3.Lerp(transform.position,new Vector3(transform.position.x, lastPos , transform.position.z), 888888.0f);



		//transform.Translate(spawn.transform.position.x, GetComponent<Collider>().bounds.extents.y, spawn.transform.position.z);

		//transform.localScale.

	}

	void OnMouseDown()
	{
		if (lastPos == 0)
			down ();
		else
			up ();
	}


	public void down()
	{
		StopAllCoroutines ();
		StartCoroutine ("buildingDown");
	}

	public void up()
	{
		StopAllCoroutines ();
		StartCoroutine ("buildingUp");
	}

	private IEnumerator buildingDown()
	{
		
		
		while (lastPos < GetComponent<Collider> ().bounds.size.y) {
			transform.Translate (new Vector3 (0, -0.015f, 0));
			//transform.position.z
			lastPos += 0.015f;
			
			yield return true;
		}



	}


	private IEnumerator buildingUp()
	{
		
		
		while (lastPos > 0) {
			transform.Translate (new Vector3 (0, +0.015f, 0));
			//transform.position.z
			lastPos -= 0.015f;
			yield return true;
			
		}

		lastPos = 0;
		
	}
}
