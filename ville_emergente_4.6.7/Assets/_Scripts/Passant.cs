using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using RAIN.Core;
using RAIN.Navigation.Targets;
using RAIN.Minds;
using RAIN.Serialization;
using RAIN.Motion;

//Classe principale pour le PNJ Passant
public class Passant : MonoBehaviour {


    public RAIN.Memory.BasicMemory tMemory;
    
    public float waitTimer = 0, minEndWaitTimer = 10, maxEndWaitTimer = 15, endWaitTimer, // timer pour l'attente des pnj à chacune des NT_Passant
        povTimer = 0,endPovTimer =5; // timer pour l'attente à l'arrivée sur le point de vue

    private string audioEventName = "";
    public string defaultAudioEventName = "";

    public static GameObject[] allNavTargets = null; // l'ensemble des navigations targets pour tous les passants  
    public List<GameObject> targets = new List<GameObject>(); // l'ensemble des navigations target de ce passant
    public GameObject previousTarget = null; // la dernière navigation target visitée par le passant


    public List<SceneRange> availableScenes = new List<SceneRange>(); // scènes à portée du passant

    public SceneRange selectedScene; // la scène actuelle sélectionnée par le passant
    public GameObject sceneLeader,sceneSpot; // le musicien ou guide qui dirige la scène en question, et la place du passant dans cette scène
    public int selectedSpotIndex = -1; // index de la place du passant dans la liste des places de la scène

    public void Awake()
    {
        tag = "NPC";
        int matriculePassant = int.Parse(this.gameObject.name.Substring(7)); // on récupère le matricule du passant dans son nom : "PassantX" X est le matricule

        if (allNavTargets == null) // récupération de toutes les navigations targets si elles n'ont pas déjà été récupérée
            allNavTargets = GameObject.FindGameObjectsWithTag("NavigationTarget");
        foreach (GameObject gObject in allNavTargets) // récupération des navigations target ayant le même matricule que le passant
        {
            if (gObject.name == "NT_Passant_" + matriculePassant)
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
        tMemory = aiRig.AI.WorkingMemory as RAIN.Memory.BasicMemory; // récupération des composants RAIN
        endWaitTimer = (int)Random.Range(minEndWaitTimer, maxEndWaitTimer); // séléction aléatoire de la fin du timer d'attente
	}
	
	// Update is called once per frame
	void Update () {
        if (tMemory.GetItem<bool>("destinationReached") && !tMemory.GetItem<bool>("inLifeScene")) // gestion du timer d'attente sur les NT
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
        if (tMemory.GetItem<bool>("guideIsOnPov") )// gestion du timer d'attente sur les points de vue
        {
            if (povTimer < endPovTimer && !tMemory.GetItem<bool>("povTimerHasEnded"))
            {
                povTimer = povTimer + Time.deltaTime;
            }
            else if (povTimer!=0)
            {
                povTimer = 0;
            }
            if (povTimer > endPovTimer)
            {
                tMemory.SetItem<bool>("povTimerHasEnded", true);
                tMemory.SetItem<bool>("guideIsOnPov", false);
            }
        }

	}

    public void EmitSound()// émission du son du passant
    {
        WwiseAudioManager.PlayFiniteEvent(audioEventName, this.gameObject);
        print("Emit Sound");
    }
    // remet à false la variable RAIN de portée de scène, si il n'y a plus de scènes à portée
    // ( utilisé quand le passant sort du trigger des scènes ) 
    public void SetInRangeOfScene(bool boolean) 
    {
        if (!boolean) 
        {
            if (availableScenes.Count == 0) // si c'était la dernière scène on remet à 0 
            {
                tMemory.SetItem<bool>("inRangeOfScene", boolean);
            }
        }
        else
        {
            tMemory.SetItem<bool>("inRangeOfScene", boolean);
        }
    }
    // assignation du booleen de la variable RAIN 
    public void SetInLifeScene(bool boolean)
    {
        tMemory.SetItem<bool>("inLifeScene", boolean);
    }

    // assignation de la cible dans RAIN
    public void SetTarget(GameObject gObject)
    {
        tMemory.SetItem<GameObject>("target", gObject);
    }

    // renvoie vrai si il y a au moins une place de libre dans l'une des scènes à portée
    public bool IsThereAtLeastOneFreeSpot()
    {
        for (int i = 0; i < availableScenes.Count; i++)// parcours des scenes
        {
            for (int j = 0; j < availableScenes[i].availablesSpots.Count; j++) // parcours des places dans la scene
            {
                if (availableScenes[i].availablesSpots[j])// si il ya une place de libre
                {
                    return true;
                }
            }
        }
        return false;
    }

    // accesseur pour savoir si le musicien est en train de danser
    public bool IsTheMusicianDancing()
    {
        AIRig aiRig = sceneLeader.GetComponentInChildren<AIRig>();
        RAIN.Memory.BasicMemory tMusicianMemory = aiRig.AI.WorkingMemory as RAIN.Memory.BasicMemory;
        return tMusicianMemory.GetItem<bool>("isDancing");

    } 

    // choisit l'une des scènes disponibles à portée et s'inscrit dedans
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

    // quitte la scèe
    public void LeaveSelectedScene()
    {
        selectedScene.availablesSpots[selectedSpotIndex] = true;
        sceneSpot = null;
        sceneLeader = null;
        selectedScene = null;
        SetTarget(null); 
        selectedSpotIndex = -1;
    }
    // assignateur RAIN pour la proximité 
    public void SetPlayerIsInRange(bool boolean)
    {
        tMemory.SetItem<bool>("playerIsInRange", boolean);
    }
    //assignateur RAIN pour la cible à regarder
    public void SetTargetLookAt(GameObject lookAtTarget)
    {
        tMemory.SetItem<GameObject>("lookAtTarget", lookAtTarget);
    }
}