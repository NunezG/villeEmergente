using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundUniverseManager : MonoBehaviour {
	
	private string switchAtmo = "switch_atmo";
	public static string switchType;
	private string switchDark = "switch_dark";

	public static float timer;
	public static float moveTimer;
	public static float stopTimer;
	public static int stopCounter;

	public static List<GameObject> playingObjects;
	
	// Use this for initialization
	void Start () {
		playingObjects = new List<GameObject>();
		switchType = switchAtmo;
	}
	
	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;

		if (timer >= 120.0f) {
			//stopCounter = 0;
			//timer = 0.0f;
			resetTimers();
		}

		if (Input.GetKeyDown (KeyCode.Space))
		{
			//if (switchType != switchDark)
			//{
				//switchType = switchDark;
				//switchSounds();
			//}	
		} else
		if (Input.GetKey (KeyCode.Z) || Input.GetKey (KeyCode.Q) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.D)) 
		{
			moveTimer += Time.deltaTime;
			stopTimer = 0;

			if((switchType == switchAtmo) && moveTimer >=6.0f)
			{
				switchType = switchDark;
				switchSounds();
			}

			//resetTimer ();
			
		} else 
		{
			moveTimer = 0;
			stopTimer += Time.deltaTime;


			if ((switchType == switchDark) && (stopTimer >= 8.0f || stopCounter >= 5))
			{
				switchType = switchAtmo;
				switchSounds();
			}

			if (stopTimer >= 3.0f)
			{
				stopCounter++;
				stopTimer = 0;
			}
		}
	}

	static void resetTimers()
	{
		timer = 0;
		stopTimer = 0;
		stopCounter = 0;
		moveTimer = 0;
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
		resetTimers ();
		//Tous les sons a switcher sont avec convolver
		for (int i = 0; i < playingObjects.Count; i++)
		{
			Debug.Log( " stop: "+ playingObjects[i].GetComponent<InteractibleObject>().soundEevent);
			//Stoppe son du convolver avec switch précédent
			WwiseAudioManager.instance.StopLoopEvent (playingObjects[i].GetComponent<InteractibleObject>().soundEevent, playingObjects[i], true);

			Debug.Log( " one: " + switchType);

			Debug.Log( " yoyoyoyo: " + playingObjects[i].name);

			Debug.Log( " two: " + playingObjects[i].GetComponent<SettingPiece>().switchNumber);

			Debug.Log( " finite: " + switchType+playingObjects[i].GetComponent<SettingPiece>().switchNumber);
			//Lance switch du convolver
			WwiseAudioManager.instance.PlayFiniteEvent(switchType+playingObjects[i].GetComponent<SettingPiece>().switchNumber, playingObjects[i]);

			Debug.Log( " play loop: " + playingObjects[i].GetComponent<InteractibleObject>().soundEevent);
			//Lance son du convolver
			WwiseAudioManager.instance.PlayLoopEvent (playingObjects[i].GetComponent<InteractibleObject>().soundEevent, playingObjects[i], true);
		}
	}
}
