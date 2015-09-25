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

	public string switchTypeTest;


	public static List<GameObject> playingObjects;

	public Color ambientLight;
	public Color upLightLight;
	public Color downLightLight;
	public Color skyBoxLight;

	public Color ambientDark;
	public Color upLightDark;
	public Color downLightDark;
	public Color skyBoxDark;


	//public Color ambientTest;
	//public Color upLightTest;
	//public Color downLighTest;
	//public Color skyBoxTest;


	public float speed = 5f;

	void Awake () {
		playingObjects = new List<GameObject>();
		switchType = switchAtmo;
	}



	// Use this for initialization
	void Start () {
		WwiseAudioManager.PlayFiniteEvent("ville_calme", this.gameObject);


		ambientLight = RenderSettings.ambientLight;
		upLightLight = GameObject.Find ("Lumières").transform.FindChild ("Directional_light_up").GetComponent<Light> ().color;
		downLightLight = GameObject.Find("Lumières").transform.FindChild("Directional_light_down").GetComponent<Light>().color;
		skyBoxLight = RenderSettings.skybox.GetColor ("_Tint");
		
		ambientDark = new Color(64.0f/255.0f, 64.0F/255.0f, 64.0F/255.0f, 0.5F);
		upLightDark = new Color(16.0F/255.0f, 30.0F/255.0f, 98.0F/255.0f, 0.5F);
		downLightDark = new Color(141.0f/255.0f, 189.0F/255.0f, 254.0F/255.0f, 0.5F);
		skyBoxDark = new Color(91.0F/255.0f, 77.0F/255.0f, 105.0F/255.0f, 0.5F);

	}
	
	// Update is called once per frame
	void Update () {

		//ambientTest = RenderSettings.ambientLight;
		//upLightTest = GameObject.Find ("Lumières").transform.FindChild ("Directional_light_up").GetComponent<Light> ().color;
		//downLighTest = GameObject.Find("Lumières").transform.FindChild("Directional_light_down").GetComponent<Light>().color;
		//skyBoxTest = RenderSettings.skybox.GetColor ("_Tint");

		switchTypeTest = switchType;

		timer += Time.deltaTime;

		if (timer >= 120.0f) {
			//stopCounter = 0;
			//timer = 0.0f;
			resetTimers();
		}

		if (Input.GetKeyDown (KeyCode.Space))
		{
		//	AkSoundEngine.StopAll ();

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
				StartCoroutine("changeColors");
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
				StartCoroutine("changeColors");
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


	IEnumerator changeColors()
	{

		if (switchType != switchDark) {

			while (GameObject.Find ("Lumières").transform.FindChild ("Directional_light_up").GetComponent<Light> ().color != upLightLight) {
				
				RenderSettings.ambientLight = Color.Lerp (RenderSettings.ambientLight, ambientLight, Time.deltaTime * speed);
				GameObject.Find ("Lumières").transform.FindChild ("Directional_light_up").GetComponent<Light> ().color = Color.Lerp (GameObject.Find ("Lumières").transform.FindChild ("Directional_light_up").GetComponent<Light> ().color, upLightLight, Time.deltaTime * speed);
				GameObject.Find ("Lumières").transform.FindChild ("Directional_light_down").GetComponent<Light> ().color = Color.Lerp (GameObject.Find ("Lumières").transform.FindChild ("Directional_light_down").GetComponent<Light> ().color, downLightLight, Time.deltaTime * speed);
				RenderSettings.skybox.SetColor ("_Tint", Color.Lerp (RenderSettings.skybox.GetColor ("_Tint"), skyBoxLight, Time.deltaTime * speed));
				
				yield return new WaitForSeconds (.1f);
			}



		} else {


			while (GameObject.Find ("Lumières").transform.FindChild ("Directional_light_up").GetComponent<Light> ().color != upLightDark) {

				RenderSettings.ambientLight = Color.Lerp (RenderSettings.ambientLight, ambientDark, Time.deltaTime * speed);
				GameObject.Find ("Lumières").transform.FindChild ("Directional_light_up").GetComponent<Light> ().color = Color.Lerp (GameObject.Find ("Lumières").transform.FindChild ("Directional_light_up").GetComponent<Light> ().color, upLightDark, Time.deltaTime * speed);
				GameObject.Find ("Lumières").transform.FindChild ("Directional_light_down").GetComponent<Light> ().color = Color.Lerp (GameObject.Find ("Lumières").transform.FindChild ("Directional_light_down").GetComponent<Light> ().color, downLightDark, Time.deltaTime * speed);
				RenderSettings.skybox.SetColor ("_Tint", Color.Lerp (RenderSettings.skybox.GetColor ("_Tint"), skyBoxDark, Time.deltaTime * speed));
			
				yield return new WaitForSeconds (.1f);
			}
		}
	}

	
	static void switchSounds()
	{

	//	bool convolve = true;
		//if (switchType == switchAtmo)
	//		convolve = false;
		resetTimers ();
		//Tous les sons a switcher sont avec convolver
		for (int i = 0; i < playingObjects.Count; i++)
		{

			if (playingObjects[i].GetComponent<ConvolutionObject>() != null && playingObjects[i].GetComponent<InteractibleObject>().soundEvent != "")
			{
				          //Stoppe son du convolver avec switch précédent
				WwiseAudioManager.StopLoopEvent (playingObjects[i].GetComponent<InteractibleObject>().soundEvent, playingObjects[i], true);

				//Lance switch du convolver
				WwiseAudioManager.PlayFiniteEvent(switchType+playingObjects[i].GetComponent<ConvolutionObject>().switchName, playingObjects[i]);

				//Lance son du convolver
				WwiseAudioManager.PlayLoopEvent (playingObjects[i].GetComponent<InteractibleObject>().soundEvent, playingObjects[i], true);
			}

			if (playingObjects[i].GetComponent<AudioEventManager>() != null)
			{
				playingObjects[i].GetComponent<AudioEventManager>().SounStopdIdle();
				
				playingObjects[i].GetComponent<AudioEventManager>().SoundPlayIdle();

				//WwiseAudioManager.PlayLoopEvent (playingObjects[i].GetComponent<InteractibleObject>().soundEvent, playingObjects[i], true);

			}
		}
	}
}
