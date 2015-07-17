using UnityEngine;
using System.Collections;

public class MusicController : WwiseAudioManager {

    public GameObject[] tabGobs;

	// Use this for initialization
	void Start () {
        LoadBank();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayLoopEvent("water_3DObject_play", tabGobs[0]);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlayLoopEvent("water_3DObject_stop", tabGobs[0]);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayLoopEvent("water_3DObject_play", tabGobs[1]);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayLoopEvent("water_3DObject_stop", tabGobs[1]);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayLoopEvent("water_3DObject_play", tabGobs[2]);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            PlayLoopEvent("water_3DObject_stop", tabGobs[2]);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            AkSoundEngine.PostEvent("prendre_morceau", tabGobs[0], (uint)AkCallbackType.AK_EndOfEvent, MyCallbackFunction, tabGobs[0]);
        }

	}
    void MyCallbackFunction(object in_cookie, AkCallbackType in_type, object in_info)
    {

        if (in_type == AkCallbackType.AK_EndOfEvent)
        {
            //AkEventCallbackInfo info = (AkEventCallbackInfo)in_info; //Then do stuff.
            print("CALLBACK");
        }
    }



}
