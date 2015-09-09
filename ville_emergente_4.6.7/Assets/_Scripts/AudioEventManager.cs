using UnityEngine;
using System.Collections;

public class AudioEventManager : MonoBehaviour {

	public string audioName;

    public void Start()
    {
		//if (audioName=="")
		//audioName = gameObject.name;
    }
    
    public void PlayFiniteEventWithCallBack(string eventName)
    {
		//Stops previous event
		SounStopdIdle();
		WwiseAudioManager.PlayFiniteEvent (eventName, this.transform.parent.gameObject, MyCallbackFunction);
		//AkSoundEngine.PostEvent(eventName, this.gameObject, (uint)AkCallbackType.AK_EndOfEvent, MyCallbackFunction, this.gameObject);	
	}

	void MyCallbackFunction(object in_cookie, AkCallbackType in_type, object in_info)	
	{
		if (in_type == AkCallbackType.AK_EndOfEvent)
		{
			AkCallbackManager.AkEventCallbackInfo info = (AkCallbackManager.AkEventCallbackInfo)in_info; //Then do stuff.
			//GameObject go = info.gameObjID;
			//AkSoundEngine.AkGameObjectID ID = info.gameObjID

			//Restart the pervious event
			SoundPlayIdle ();
		}	
	}

	/*
	*Sound events
	*/
	public void SoundPlayIdle()
	{
		WwiseAudioManager.PlayLoopEvent (audioName+"_idle", this.transform.parent.gameObject);
	}

	public void SounStopdIdle()
	{
		WwiseAudioManager.StopLoopEvent (audioName+"_idle", this.transform.parent.gameObject);
	}


	/*
	*SFX Events
	*/
	public void soundJoie()
	{
		PlayFiniteEventWithCallBack (audioName+"_joie");	
		//gObject.GetComponentInChildren<Animator> ().SetBool ("",true);
	}

	public void soundSon()
	{
		PlayFiniteEventWithCallBack (audioName+"_son");	
	}

	public void soundNouveauSon()
	{
		PlayFiniteEventWithCallBack (audioName+"_nouveau_son");	
	}

	public void soundTourne()
	{
		PlayFiniteEventWithCallBack (audioName+"_tourne");	
	}

	public void soundOuverture()
	{
		PlayFiniteEventWithCallBack (audioName+"_ouverture");	
	}

	public void soundOrdre()
	{
		PlayFiniteEventWithCallBack (audioName+"_ordre");	
	}

	public void soundChangeCouleur()
	{
		PlayFiniteEventWithCallBack (audioName+"_change_couleur");	
	}

	public void soundDanse()
	{
		PlayFiniteEventWithCallBack (audioName+"_danse");	
	}

    
}
