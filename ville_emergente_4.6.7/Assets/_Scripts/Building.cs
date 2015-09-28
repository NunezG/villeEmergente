using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {

	private float lastPos = 0;

	public float collapsingSpeed = 10.0f;

	public void Start()
	{
	}

    /*
	void OnMouseDown()
	{
		if (lastPos == 0)
			Down ();
		else
			Up ();
	}*/


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

        GetComponent<Blend>().inverse = true;
        GetComponent<Blend>().startTime = Time.time;
        GetComponent<Blend>().endTime = Time.time + collapsingSpeed;
        GetComponent<Blend>().enabled = true;


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
