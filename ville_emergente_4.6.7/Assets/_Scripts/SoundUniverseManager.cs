using UnityEngine;
using System.Collections;

public class SoundUniverseManager : MonoBehaviour {
	
	private string switchAtmo = "switch_atmo";
	public static string switchType;
	private string switchDark = "switch_dark";

	private float timer;

	private float moveTimer;


	private float stopTimer;
	private int stopCounter;


	// Use this for initialization
	void Start () {
		switchType = switchAtmo;
	}
	
	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;

		if (timer >= 120.0f) {
			stopCounter = 0;
		}

		if (Input.GetKeyDown (KeyCode.Space))
		{
			if (switchType != switchDark)
				switchType = switchDark;
			 	
		} else
		if (Input.GetKey (KeyCode.Z) || Input.GetKey (KeyCode.Q) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.D)) {
			moveTimer += Time.deltaTime;

			stopTimer = 0;

			if(moveTimer >=6.0f)
			{
				switchType = switchDark;
			}

			//resetTimer ();
			
		} else 
		{
			moveTimer = 0;
			stopTimer += Time.deltaTime;

			if (stopTimer >= 3.0f)
			{
				stopCounter++;
				stopTimer = 0;
			}

			if (stopTimer >= 8.0f || stopCounter == 5) {
				switchType = switchAtmo;
			}
		}
	}

	void resetTimer()
	{
		stopTimer = 0;
		timer = 0;
	}

}
