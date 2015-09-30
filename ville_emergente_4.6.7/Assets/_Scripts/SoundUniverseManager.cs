using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundUniverseManager : MonoBehaviour {
	
	private string switchAtmo = "switch_atmo";
	public static string switchType;
	private string switchDark = "switch_dark";

	public float resetTimer;
	public float stopTimer;

	public float ResetTime = 12.0f; //Temps mis pour reset de tous les timer et counter
	public float maxStopTime = 6.0f; //Temps mis en arret pour passer a l'atmo

	public static List<GameObject> playingObjects;

	public Color ambientLight;
	public Color upLightLight;
	public Color downLightLight;
	public Color skyBoxLight;
	public Color FogLight;
	private float fogDensityLight;

	public Color ambientDark;
	public Color upLightDark;
	public Color downLightDark;
	public Color skyBoxDark;
	public Color FogDark;
	private float fogDensityDark;

	private bool stopCounted = false;

	//juste pour affiche la valeur actuelle dans l'editeur
	public Color ambientTest;
	public Color upLightTest;
	public Color downLighTest;
	public Color skyBoxTest;
	public Color FogTest;
	private float fogDensityTest;

	private Color upLightStart;
	private Color downLightStart;
	private Color skyBoxStart;
	private Color ambientStart;
	private Color fogStart;
	private float fogDensityStart;

	private Color upLightEnd;
	private Color downLightEnd;
	private Color skyBoxEnd;
	private Color ambientEnd;
	private Color fogEnd;
	private float fogDensityEnd;

	void Awake () {
		playingObjects = new List<GameObject>();
		switchType = switchAtmo;
	}
	
	// Use this for initialization
	void Start () {

		WwiseAudioManager.PlayFiniteEvent("ville_calme", this.gameObject);

		//Prends les variables par défaut
		ambientLight = RenderSettings.ambientLight;
		upLightLight = GameObject.Find ("Lumières").transform.FindChild ("Directional_light_up").GetComponent<Light> ().color;
		downLightLight = GameObject.Find("Lumières").transform.FindChild("Directional_light_down").GetComponent<Light>().color;
		skyBoxLight = RenderSettings.skybox.GetColor ("_Tint");
		FogLight =  RenderSettings.fogColor;
		fogDensityLight = RenderSettings.fogDensity;

		ambientDark = new Color(25.0f/255.0f, 32.0F/255.0f, 36.0F/255.0f, 0.5F);
		upLightDark = new Color(8.0F/255.0f, 16.0F/255.0f, 66.0F/255.0f, 0.5F);
		downLightDark = new Color(58.0f/255.0f, 0.0F/255.0f, 68.0F/255.0f, 0.5F);
		skyBoxDark = new Color(0.0F/255.0f, 0.0F/255.0f, 0.0F/255.0f, 0.5F);
		FogDark = new Color(57.0F/255.0f, 74.0F/255.0f, 112.0F/255.0f, 0.5F);
		fogDensityDark = 0.004f;

		upLightStart = upLightLight;
		downLightStart = downLightLight;
		skyBoxStart =skyBoxLight;
		ambientStart = ambientLight;
		fogStart = FogLight;

		upLightEnd = upLightDark;
		downLightEnd = downLightDark;
		skyBoxEnd = skyBoxDark;
		ambientEnd = ambientDark;
		fogEnd = FogDark;
		
		RenderSettings.fog = true;
		
	}

	void OnDestroy() {

		RenderSettings.ambientLight = ambientLight;
		GameObject.Find ("Lumières").transform.FindChild ("Directional_light_up").GetComponent<Light> ().color = upLightLight;
		GameObject.Find ("Lumières").transform.FindChild ("Directional_light_down").GetComponent<Light> ().color = downLightLight;
		RenderSettings.skybox.SetColor ("_Tint", skyBoxLight);
		RenderSettings.fogColor = FogLight;
		RenderSettings.fogDensity = fogDensityLight;		
	}

	// Update is called once per frame
	void Update () 
	{
		//update variables de l'editeur
		ambientTest = RenderSettings.ambientLight;
		upLightTest = GameObject.Find ("Lumières").transform.FindChild ("Directional_light_up").GetComponent<Light> ().color;
		downLighTest = GameObject.Find("Lumières").transform.FindChild("Directional_light_down").GetComponent<Light>().color;
		skyBoxTest = RenderSettings.skybox.GetColor ("_Tint");
		FogTest = RenderSettings.fogColor;

		if (resetTimer >= ResetTime) {

			stopTimer += Time.deltaTime;

			if (stopTimer >= maxStopTime) {
				resetTimer = 0f;
				stopTimer = 0f;
			}

		}else{
			resetTimer += Time.deltaTime;
			RenderSettings.ambientLight = Color.Lerp (ambientStart, ambientEnd, resetTimer / ResetTime);
			GameObject.Find ("Lumières").transform.FindChild ("Directional_light_up").GetComponent<Light> ().color = Color.Lerp (upLightStart, upLightEnd, resetTimer / ResetTime);
			GameObject.Find ("Lumières").transform.FindChild ("Directional_light_down").GetComponent<Light> ().color = Color.Lerp (downLightStart, downLightEnd, resetTimer / ResetTime);
			RenderSettings.skybox.SetColor ("_Tint", Color.Lerp (skyBoxStart, skyBoxEnd, resetTimer / ResetTime));
			RenderSettings.fogColor = Color.Lerp (fogStart, fogEnd, resetTimer / ResetTime);
			RenderSettings.fogDensity = Mathf.Lerp (fogDensityStart, fogDensityEnd, resetTimer / ResetTime);


			if (resetTimer >= ResetTime) 
			{
				if (switchType != switchDark) {
					
					upLightStart = upLightDark;
					downLightStart = downLightDark;
					skyBoxStart =skyBoxDark;
					ambientStart = ambientDark;
					fogStart = FogDark;
					fogDensityStart =  fogDensityDark;

					upLightEnd = upLightLight;
					downLightEnd = downLightLight;
					skyBoxEnd = skyBoxLight;
					ambientEnd = ambientLight;
					fogEnd = FogLight;
					fogDensityEnd = fogDensityLight;

					switchType = switchDark;
				}else {
					
					upLightStart = upLightLight;
					downLightStart = downLightLight;
					skyBoxStart =skyBoxLight;
					ambientStart = ambientLight;
					fogStart = FogLight;
					fogDensityStart =  fogDensityLight;

					upLightEnd = upLightDark;
					downLightEnd = downLightDark;
					skyBoxEnd = skyBoxDark;
					ambientEnd = ambientDark;
					fogEnd = FogDark;
					fogDensityEnd = fogDensityDark;

					switchType = switchAtmo;
				}

				switchSounds ();
			}
		}	
	}

	void resetTimers()
	{
		resetTimer = 0;
		stopTimer = 0;
		stopCounted = false;
	}

	public static void addSoundEvent(GameObject target)
	{
		playingObjects.Add (target);

	}
	
	public static void removeSoundEvent(GameObject target)
	{
		playingObjects.Remove (target);	
	}
	
	void switchSounds()
	{
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

			else if (playingObjects[i].GetComponentInChildren<AudioEventManager>() != null && playingObjects[i].GetComponentInChildren<AudioEventManager>().idleSound)
			{
				GameObject playingNPC = playingObjects[i];

				//Stoppe son Idle (PNJ)
				playingNPC.GetComponentInChildren<AudioEventManager>().SounStopdIdle();

				//play son Idle (PNJ)
				playingNPC.GetComponentInChildren<AudioEventManager>().SoundPlayIdle();
			}
		}
	}
}
