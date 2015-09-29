using UnityEngine;
using System.Collections;

public class AudioEventManager : MonoBehaviour {

	public NPCType audioName;
	public bool idleSound = true;


    public void Start()
    {
		//Ajoute idle dans la liste des switch
		SoundUniverseManager.addSoundEvent (this.transform.parent.gameObject); 
		SoundPlayIdle ();
		//if (audioName=="")
		//audioName = gameObject.name;
    }
    
    public void PlayFiniteEventWithCallBack(string eventName)
    {
		//Stops previous event
		if (idleSound) SounStopdIdle();
		WwiseAudioManager.PlayFiniteEvent (eventName, this.transform.parent.gameObject, MyCallbackFunction);
	}

	void MyCallbackFunction(object in_cookie, AkCallbackType in_type, object in_info)	
	{
		if (in_type == AkCallbackType.AK_EndOfEvent)
		{
			AkCallbackManager.AkEventCallbackInfo info = (AkCallbackManager.AkEventCallbackInfo)in_info; //Then do stuff.
			//GameObject go = info.gameObjID;
			//AkSoundEngine.AkGameObjectID ID = info.gameObjID

			//Restart the pervious event
			if (idleSound) SoundPlayIdle ();
		}	
	}

	/*
	*Sound events
	*/
	public void SoundPlayIdle()
	{		
		WwiseAudioManager.PlayFiniteEvent(SoundUniverseManager.switchType+"_mood", this.transform.parent.gameObject);
		WwiseAudioManager.PlayLoopEvent (audioName.ToString()+"_idle", this.transform.parent.gameObject);
	}

	public void SounStopdIdle()
	{
		WwiseAudioManager.StopLoopEvent (audioName.ToString()+"_idle", this.transform.parent.gameObject);
	}

	public void SoundRemovedIdle()
	{
		SoundUniverseManager.removeSoundEvent (this.transform.parent.gameObject);
		SounStopdIdle ();
	}


	/*
	*SFX Events
	*/
	public void soundSon()
	{
		PlayFiniteEventWithCallBack (audioName.ToString()+"_son");	
	}

	public void soundNouveauSon()
	{
		PlayFiniteEventWithCallBack (audioName.ToString()+"_nouveau_son");	
	}

	public void soundTourne()
	{
		PlayFiniteEventWithCallBack (audioName.ToString()+"_tourne");	
	}

	public void soundOuverture()
	{
		PlayFiniteEventWithCallBack (audioName.ToString()+"_ouverture");	
	}

	public void soundOrdre()
	{
		PlayFiniteEventWithCallBack (audioName.ToString()+"_ordre");	
	}

	public void soundChangeCouleur()
	{
		PlayFiniteEventWithCallBack (audioName.ToString()+"_change_couleur");	
	}

	public void soundDanse()
	{
		PlayFiniteEventWithCallBack (audioName.ToString()+"_danse");	
	}

    
}
