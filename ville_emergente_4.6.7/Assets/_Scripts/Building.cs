using UnityEngine;
using System.Collections;

public class Building : InteractibleObject {

	private float lastPos = 0;

	public float collapsingSpeed = 10.0f;
	 


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
			Down ();
		else
			Up ();
	}


	public void Down()
	{
		StopAllCoroutines ();
		StartCoroutine ("BuildingDown");
	}

	public void Up()
	{
		StopAllCoroutines ();
		StartCoroutine ("BuildingUp");
	}

	private IEnumerator BuildingDown()
	{
		
		while (lastPos < GetComponent<Collider> ().bounds.size.y) {
			transform.Translate (new Vector3 (0, 0, -collapsingSpeed*Time.deltaTime));
			//transform.position.z
			lastPos += collapsingSpeed*Time.deltaTime;
			
			yield return true;
		}



	}


	private IEnumerator BuildingUp()
	{
		
		
		while (lastPos > 0) {
			transform.Translate (new Vector3 (0, 0, collapsingSpeed*Time.deltaTime));
			//transform.position.z
			lastPos -= collapsingSpeed*Time.deltaTime;
			yield return true;
			
		}

		lastPos = 0;
		
	}
}
