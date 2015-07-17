﻿using UnityEngine;
using System.Collections;

public class WwiseAudioManager : MonoBehaviour {

    public static WwiseAudioManager wwiseAudioManager;
    uint bankID;
	// Use this for initialization
	public void LoadBank () {
        wwiseAudioManager = this;
        AkSoundEngine.LoadBank("Main", AkSoundEngine.AK_DEFAULT_POOL_ID, out bankID);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void PlayEvent(string eventName, GameObject gObject)
    {
        AkSoundEngine.PostEvent(eventName, gObject);
    }
    public void StopEvent(string eventName, GameObject gObject, int fadeout)
    {
        uint eventID = AkSoundEngine.GetIDFromString(eventName);
        AkSoundEngine.ExecuteActionOnEvent(eventID, AkActionOnEventType.AkActionOnEventType_Stop, gObject, fadeout * 1000, AkCurveInterpolation.AkCurveInterpolation_Sine);
    }
    public void PauseEvent(string eventName, GameObject gObject, int fadeout)
    {
        uint eventID = AkSoundEngine.GetIDFromString(eventName);
        AkSoundEngine.ExecuteActionOnEvent(eventID, AkActionOnEventType.AkActionOnEventType_Pause, gObject, fadeout * 1000, AkCurveInterpolation.AkCurveInterpolation_Sine);
    }
    public void ResumeEvent(string eventName, GameObject gObject, int fadeout)
    {
        uint eventID = AkSoundEngine.GetIDFromString(eventName);
        AkSoundEngine.ExecuteActionOnEvent(eventID, AkActionOnEventType.AkActionOnEventType_Resume, gObject, fadeout * 1000, AkCurveInterpolation.AkCurveInterpolation_Sine);
    }
}
