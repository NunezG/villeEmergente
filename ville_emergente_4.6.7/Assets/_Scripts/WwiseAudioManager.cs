using UnityEngine;
using System.Collections;

public class WwiseAudioManager : MonoBehaviour {

    public static WwiseAudioManager instance;
    uint bankID;
	// Use this for initialization


	public void Awake()
	{
		instance = this;
		LoadBank();
		PlayFiniteEvent("ville_calme", this.gameObject);
	}

    public void Start()
    {
        
    }

	public void LoadBank () {
        AkSoundEngine.LoadBank("Main", AkSoundEngine.AK_DEFAULT_POOL_ID, out bankID);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
	public void PlayLoopEvent(string eventName, GameObject gObject, bool convolution = false)
    {

		if (convolution)
			eventName = eventName+"_convolver";

        AkSoundEngine.PostEvent(eventName+"_play", gObject);
    }

	public void StopLoopEvent(string eventName, GameObject gObject, bool convolution = false)
    {
		if (convolution)
			eventName = eventName+"_convolver";

        AkSoundEngine.PostEvent(eventName+"_stop", gObject);
    }

    public void PlayFiniteEvent(string eventName, GameObject gObject, AkCallbackManager.EventCallback callBackFunction )
    {
        AkSoundEngine.PostEvent(eventName, gObject, (uint)AkCallbackType.AK_EndOfEvent, callBackFunction, gObject);

	}

    public void PlayFiniteEvent(string eventName, GameObject gObject )
    {
        AkSoundEngine.PostEvent(eventName, gObject);
    }


	//Sound events

	public void soundPlayIdle(string playerName, GameObject gObject )
	{
		PlayLoopEvent (playerName+"_idle", gObject);
	}

	public void sounStopdIdle(string playerName, GameObject gObject )
	{
		StopLoopEvent (playerName+"_idle", gObject);
	}

	public void soundJoie(string playerName, GameObject gObject, AkCallbackManager.EventCallback callBackFunction )
	{
		PlayFiniteEvent (playerName+"_joie", this.gameObject,callBackFunction);	
		//gObject.GetComponentInChildren<Animator> ().SetBool ("",true);
	}


	public void soundSon(string playerName, GameObject gObject, AkCallbackManager.EventCallback callBackFunction )
	{
		PlayFiniteEvent (playerName+"_son", this.gameObject,callBackFunction);	
	}

	public void soundNouveauSon(string playerName, GameObject gObject , AkCallbackManager.EventCallback callBackFunction)
	{
		PlayFiniteEvent (playerName+"_nouveau_son", this.gameObject,callBackFunction);	
	}

	public void soundTourne(string playerName, GameObject gObject , AkCallbackManager.EventCallback callBackFunction)
	{
		PlayFiniteEvent (playerName+"_tourne", this.gameObject,callBackFunction);	
	}

	public void soundOuverture(string playerName, GameObject gObject, AkCallbackManager.EventCallback callBackFunction )
	{
		PlayFiniteEvent (playerName+"_ouverture", this.gameObject,callBackFunction);	
	}

	public void soundOrdre(string playerName, GameObject gObject, AkCallbackManager.EventCallback callBackFunction )
	{
		PlayFiniteEvent (playerName+"_ordre", this.gameObject,callBackFunction);	
	}

	public void soundChangeCouleur(string playerName, GameObject gObject, AkCallbackManager.EventCallback callBackFunction )
	{
		PlayFiniteEvent (playerName+"_change_couleur", this.gameObject,callBackFunction);	
	}

	public void soundDanse(string playerName, GameObject gObject , AkCallbackManager.EventCallback callBackFunction)
	{
		PlayFiniteEvent (playerName+"_danse", this.gameObject,callBackFunction);	
	}

    /*
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
    }*/
}
