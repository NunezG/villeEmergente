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
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            PlayEvent("water_3DObject_play",tabGobs[0]);
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            PlayEvent("water_3DObject_stop", tabGobs[0]);

        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {

        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {

        }
	}
}
