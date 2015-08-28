using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundUniverseManager : MonoBehaviour {
	
	private string switchAtmo = "switch_atmo";
	public static string switchType;
	private string switchDark = "switch_dark";

	private float timer;
	private float moveTimer;
	private float stopTimer;
	private int stopCounter;

	private static List<GameObject> playingObjects;
	
	// Use this for initialization
	void Start () {
		playingObjects = new List<GameObject>();
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
			{
				switchType = switchDark;
				switchSounds();
			}	
		} else
		if (Input.GetKey (KeyCode.Z) || Input.GetKey (KeyCode.Q) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.D)) {
			moveTimer += Time.deltaTime;

			stopTimer = 0;

			if(moveTimer >=6.0f)
			{
				switchType = switchDark;
				switchSounds();
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

			if (stopTimer >= 8.0f || stopCounter == 5)
			{
				switchType = switchAtmo;
				switchSounds();
			}
		}
	}

	void resetTimer()
	{
		stopTimer = 0;
		timer = 0;
	}

	public static void addSoundEvent(GameObject target)
	{
		playingObjects.Add (target);

	}
	
	public static void removeSoundEvent(GameObject target)
	{
		playingObjects.Remove (target);
		
	}

	//void getNextEvent()
	//{
		//soundsList.Remove (name, target);
		
	//}

	static void switchSounds()
	{
	//	bool convolve = true;
		//if (switchType == switchAtmo)
	//		convolve = false;

		//Tous les sons a switcher sont avec convolver
		for (int i = 0; i < playingObjects.Count; i++)
		{
			//Stoppe son du convolver avec switch précédent
			WwiseAudioManager.instance.StopLoopEvent (playingObjects[i].GetComponent<Fragment>().soundEevent, playingObjects[i], true);

			//Lance switch du convolver
			WwiseAudioManager.instance.PlayFiniteEvent(switchType+playingObjects[i].GetComponent<SettingPiece>().switchNumber, playingObjects[i]);

			//Lance son du convolver
			WwiseAudioManager.instance.PlayLoopEvent (playingObjects[i].GetComponent<Fragment>().soundEevent, playingObjects[i], true);
		}
	}
}
