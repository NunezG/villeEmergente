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

    public SceneRange selectedScene;
    public GameObject sceneLeader,sceneSpot;
    public int selectedSpotIndex = -1;

    public void Awake()
    {
        tag = "NPC";
        // if(targets==null)
        //Debug.Log ("START MUSICIENN");
        int matriculePassant = int.Parse(this.gameObject.name.Substring(7));
        print("matriculePassant " + matriculePassant);

        if (allNavTargets == null)
            allNavTargets = GameObject.FindGameObjectsWithTag("NavigationTarget");
        foreach (GameObject gObject in allNavTargets)
        {
            if (gObject.name == "Navigation Target " + matriculePassant)
            {
                targets.Add(gObject);
            }
        }        
    }

	// Use this for initialization
    void Start()
    {
        audioEventName = defaultAudioEventName;
        AIRig aiRig = GetComponentInChildren<AIRig>();
        tMemory = aiRig.AI.WorkingMemory as RAIN.Memory.BasicMemory;
        endWaitTimer = (int)Random.Range(minEndWaitTimer, maxEndWaitTimer);
	}
	
	// Update is called once per frame
	void Update () {
        if (tMemory.GetItem<bool>("destinationReached") && !tMemory.GetItem<bool>("inLifeScene"))
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
        if (!boolean)
        {
            if (availableScenes.Count == 0)
            {
                tMemory.SetItem<bool>("inRangeOfScene", boolean);
            }
        }
        else
        {
            tMemory.SetItem<bool>("inRangeOfScene", boolean);
        }
    }
    public void SetInLifeScene(bool boolean)
    {
        tMemory.SetItem<bool>("inLifeScene", boolean);
    }
    public void SetTarget(GameObject gObject)
    {
        tMemory.SetItem<GameObject>("target", gObject);
    }

    public bool IsThereAtLeastOneFreeSpot()
    {
        for (int i = 0; i < availableScenes.Count; i++)// parcours des scenes
        {
            for (int j = 0; j < availableScenes[i].availablesSpots.Count; j++) // parcours des places dans la scene
            {
                if (availableScenes[i].availablesSpots[i])// si il ya une place de libre
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsTheMusicianDancing()
    {
        AIRig aiRig = sceneLeader.GetComponentInChildren<AIRig>();
        RAIN.Memory.BasicMemory tMusicianMemory = aiRig.AI.WorkingMemory as RAIN.Memory.BasicMemory;
        return tMusicianMemory.GetItem<bool>("isDancing");

    } 

    public void SelectAndEnterScene()
    {

        float[] rdmTab = new float[availableScenes.Count];

        for (int i = 0; i < availableScenes.Count; i++) // evaluation des distances entre le passant et la scene 
        {
            float distance = (this.transform.position - availableScenes[i].transform.position).magnitude;
            rdmTab[i] = distance + Random.Range(0, 3); // bruitage 
        }
        int selectedIndex = 0;
        float min = 10000;
        for (int i = 0; i < rdmTab.Length; i++) // selection de la scene la plus proche
        {
            if (rdmTab[i] < min)
            {
                min = rdmTab[i];
                selectedIndex = i;
            }
        }
        selectedScene = availableScenes[selectedIndex];
        sceneLeader = selectedScene.mainActor; // recuperation du leader de la scene en question

        for (int i = 0; i < selectedScene.availablesSpots.Count; i++) // parcours des places dans la scene
        {
            if (selectedScene.availablesSpots[i])// si il ya une place de libre
            {
                sceneSpot = selectedScene.spots[i];
                SetTarget(sceneSpot); // le passant se dirige sur cette place
                selectedScene.availablesSpots[i] = false; // et la place est prise
                selectedSpotIndex = i;
                break;
            }
        }
    }

    public void LeaveSelectedScene()
    {
        selectedScene.availablesSpots[selectedSpotIndex] = true;
        sceneSpot = null;
        selectedScene = null;
        SetTarget(null); 
        selectedSpotIndex = -1;
    }

    public void SetPlayerIsInRange(bool boolean)
    {
        tMemory.SetItem<bool>("playerIsInRange", boolean);
    }

    public void SetTargetLookAt(GameObject lookAtTarget)
    {
        tMemory.SetItem<GameObject>("lookAtTarget", lookAtTarget);
    }
}