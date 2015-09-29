using UnityEngine;
using System.Collections;

public static class WwiseAudioManager {

   // public static WwiseAudioManager instance;
	static readonly uint bankID;

	// Use this for initialization
	static WwiseAudioManager()
	{
		//instance = this;
		AkSoundEngine.LoadBank("Main", AkSoundEngine.AK_DEFAULT_POOL_ID, out bankID);

		//Fonction de vibration
		AkSoundEngine.AddPlayerMotionDevice (0, 1, AkSoundEngine.AKMOTIONDEVICEID_RUMBLE);
		//AkSoundEngine.SetPlayerListener (0, 0);
		AkSoundEngine.SetListenerPipeline (0, true, true);
		//AkSoundEngine.SetActiveListeners(GameObject.FindWithTag("Player"),1); 
	}

    //Methode pour jouer un event Wwise ayant une durée finie, sans callback
    // eventName : le nom de l'event
    // gObject : l'objet sur lequel il sera positionné dans l'espace ( si il s'agit  d'un son 3d )
	public static void PlayFiniteEvent(string eventName, GameObject gObject )
    {
		AkSoundEngine.PostEvent(eventName, gObject);
    }
    //Methode pour jouer un event Wwise ayant une durée finie, en appelant une methode de callback quand il se termine
    // eventName : le nom de l'event
    // gObject : l'objet sur lequel il sera positionné dans l'espace ( si il s'agit  d'un son 3d )
    // callBackFunction : la fonction a appellé à la fin de l'event
	public static void PlayFiniteEvent(string eventName, GameObject gObject, AkCallbackManager.EventCallback callBackFunction )
	{
		AkSoundEngine.PostEvent(eventName, gObject, (uint)AkCallbackType.AK_EndOfEvent, callBackFunction, gObject);	
	}
    //Methode pour jouer un event Wwise en boucle, ou son équivalent avec convolution
    // /!\ nécessite que les events Wwise respectent la convention de nommage "event_play" et "event_convolver_play"
    // eventName : le nom de l'event
    // gObject : l'objet sur lequel il sera positionné dans l'espace ( si il s'agit  d'un son 3d )
    // convolution : si il s'agit de l'event avec convolution ou non
	public static void PlayLoopEvent(string eventName, GameObject gObject, bool convolution = false)
	{
		if (convolution)
			eventName = eventName+"_convolver";
		
		AkSoundEngine.PostEvent(eventName+"_play", gObject);
	}
    //Methode pour stopper un event Wwise qui boucle, ou son équivalent avec convolution
    // /!\ nécessite que les events Wwise respectent la convention de nommage "event_stop" et "event_convolver_stop"
    // eventName : le nom de l'event
    // gObject : l'objet sur lequel il sera positionné dans l'espace ( si il s'agit  d'un son 3d )    
    // convolution : si il s'agit de l'event avec convolution ou non
	public static void StopLoopEvent(string eventName, GameObject gObject, bool convolution = false)
	{
		if (convolution)
			eventName = eventName+"_convolver";
		
		AkSoundEngine.PostEvent(eventName+"_stop", gObject);
	}
}