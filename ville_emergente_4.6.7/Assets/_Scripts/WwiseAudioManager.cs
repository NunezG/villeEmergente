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

	public static void PlayFiniteEvent(string eventName, GameObject gObject )
    {
		AkSoundEngine.PostEvent(eventName, gObject);
    }

	public static void PlayFiniteEvent(string eventName, GameObject gObject, AkCallbackManager.EventCallback callBackFunction )
	{
		AkSoundEngine.PostEvent(eventName, gObject, (uint)AkCallbackType.AK_EndOfEvent, callBackFunction, gObject);	
	}

	public static void PlayLoopEvent(string eventName, GameObject gObject, bool convolution = false)
	{
		if (convolution)
			eventName = eventName+"_convolver";
		
		AkSoundEngine.PostEvent(eventName+"_play", gObject);
	}
	
	public static void StopLoopEvent(string eventName, GameObject gObject, bool convolution = false)
	{
		if (convolution)
			eventName = eventName+"_convolver";
		
		AkSoundEngine.PostEvent(eventName+"_stop", gObject);
	}

}
