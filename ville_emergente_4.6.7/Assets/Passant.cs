using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using RAIN.Core;
using RAIN.Navigation.Targets;
using RAIN.Minds;
using RAIN.Serialization;
using RAIN.Motion;

public class Passant : MonoBehaviour {


    public RAIN.Memory.BasicMemory tMemory;



    public float waitTimer = 0, minEndWaitTimer = 10, maxEndWaitTimer = 15, endWaitTimer;

    public Material material;
    private string audioEventName = "";
    public string defaultAudioEventName = "";

    public static GameObject[] allNavTargets = null;
    public List<GameObject> targets = new List<GameObject>();
    public GameObject previousTarget = null;


    public List<SceneRange> availableScenes = new List<SceneRange>();

    public GameObject sceneLeader,sceneSpot;
    

	// Use this for initialization
    void Start()
    {
        audioEventName = defaultAudioEventName;
	
	}
	
	// Update is called once per frame
	void Update () {
        if (tMemory.GetItem<bool>("destinationReached"))
        {
            if (waitTimer < endWaitTimer && !tMemory.GetItem<bool>("waitTimerHasEnded"))
            {
                waitTimer = waitTimer + Time.deltaTime;
            }
            else if (!tMemory.GetItem<bool>("waitTimerHasEnded"))
            {
                tMemory.SetItem<bool>("waitTimerHasEnded", true);
            }
            if (waitTimer != 0 && tMemory.GetItem<bool>("waitTimerHasEnded"))
            {
                waitTimer = 0;
                endWaitTimer = (int)Random.Range(minEndWaitTimer, maxEndWaitTimer);
            }
        }
	}

    public void EmitSound()
    {
        WwiseAudioManager.instance.PlayFiniteEvent(audioEventName, this.gameObject);
        print("Emit Sound");
    }

    public void SetInRangeOfScene(bool boolean)
    {
        tMemory.SetItem<bool>("InRangeOfScene", boolean);
    }


    public void SelectAndEnterScene()
    {
        for (int i = 0; i < availableScenes[0].availablesSpots.Count; i++)
        {
            if (availableScenes[0].availablesSpots[i])
            {
                sceneSpot = availableScenes[0].spots[i];
                break;
            }
        }
            //availableScenes[0].mainActor;

    }

}
