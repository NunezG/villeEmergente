using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using RAIN.Core;
using RAIN.Navigation.Targets;
using RAIN.Minds;
using RAIN.Serialization;
using RAIN.Motion;

// Classe principale pour le PNJ Musicien
public class Musicien : MonoBehaviour{


    public RAIN.Memory.BasicMemory tMemory; // mémoire RAIN 

    public float soundTimer = 0,minEndSoundTimer=10, maxEndSoundTimer = 15, endSoundTimer, // timer d'émission du son 
                waitTimer=0,minEndWaitTimer=10, maxEndWaitTimer = 15, endWaitTimer, // timer d'attente aux navigations targets                
                danceTimer=0,endDanceTimer=10; // timer de danse

    public FragmentBubble fragmentBubble; // bulle affichée au dessus du musicien par intervalle quand il lui manque un fragment
    public Material defaultMaterial, elecMaterial, liquidMaterial, metalMaterial, urbanMaterial, woodMaterial; // materials du musicien pour chaque famille de fragment

    public static GameObject[] allNavTargets = null; // ensemble des navigations targets pour les passants
    public List<GameObject> targets = new List<GameObject>(); // navigations target pour ce passant
	public GameObject previousTarget = null;// navigation target précédente

	public GameObject[] buildings; // bâtiment qui doivent s'abaisser quand le musicien est complété ( peut être vide )
    public SceneRange scene; // scène liée à ce musicien

	public bool isFragmentComplete=false;// boolean pour la completion du guide
	
	public void Awake()
    {
       tag = "NPC";
       int matriculeMusicien = int.Parse(this.gameObject.name.Substring(8)); // récupération du matricule X du musicien "MusicienX"


        if (allNavTargets==null)
            allNavTargets = GameObject.FindGameObjectsWithTag("NavigationTarget"); // récupération de toutes les navigations targets si ce n'est pas déjà fait
        foreach (GameObject gObject in allNavTargets)// récupération des navigations targets propre à ce musicien
       {
           if (gObject.name == "NT_Musicien_" + matriculeMusicien)
           {
               targets.Add(gObject);
           }
        }
	}

	// Use this for initialization
    public void Start()
    {
        AIRig aiRig = GetComponentInChildren<AIRig>();
        tMemory = aiRig.AI.WorkingMemory as RAIN.Memory.BasicMemory; // référence sur mémoire RAIN 


        endSoundTimer = (int)Random.Range(minEndSoundTimer, maxEndSoundTimer); // initialisation aléatoire de timer
        endWaitTimer = (int)Random.Range(minEndWaitTimer, maxEndWaitTimer);
	}
	
	// Update is called once per frame
    public void Update()
    {
        
        if (soundTimer < endSoundTimer && !tMemory.GetItem<bool>("soundTimerHasEnded")) // gestion des timers et des variables RAIN correspondantes
        {
            soundTimer = soundTimer + Time.deltaTime;
        }
        else if (!tMemory.GetItem<bool>("soundTimerHasEnded"))
        {
            tMemory.SetItem<bool>("soundTimerHasEnded", true);
        }
        if (soundTimer != 0 && tMemory.GetItem<bool>("soundTimerHasEnded"))
        {
            soundTimer = 0;
            endSoundTimer = (int)Random.Range(minEndSoundTimer, maxEndSoundTimer);
        }
        //----------------------
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
        //---------------------------------
        if (tMemory.GetItem<bool>("isDancing"))
        {
            if (danceTimer < endDanceTimer )
            {
                danceTimer = danceTimer + Time.deltaTime;
            }
            else 
            {
                danceTimer = 0;
                tMemory.SetItem<bool>("isDancing", false);
            }
        }
	}

    // Méthode pour faire s'abaisser certains bâtiments liés au musicien
    public void OpenTheWay()
    {
        for (int i = 0; i < buildings.Length; i++)
        {
            buildings[i].GetComponent<Building>().Down();
        }
    }
    //active la scène du musicien
    public void ActiveScene()
    {
        scene.gameObject.SetActive(true);
    }

    // méthode à appeler quand on déponse un fragment sur le musicien
    public void OnAddingFragment(Fragment fragment)
    {
        print("NPC:OnAddingFragment");

        SetJustReceivedFragmentComplete(true); // setting de la variable RAIN 
        Material addedMaterial;
        switch(fragment.family){ // récupération du matérial correspondant au fragment qui vient d'être ajouté au musicien
            case FragmentType.electricity:
                addedMaterial = elecMaterial;
                break;
            case FragmentType.liquid:
                addedMaterial = liquidMaterial;
                break;
            case FragmentType.metal:
                addedMaterial = metalMaterial;
                break;
            case FragmentType.urban:
                addedMaterial = urbanMaterial;
                break;
            case FragmentType.wood:
                addedMaterial = woodMaterial;
                break;
            default:
                addedMaterial = defaultMaterial;
                break;
        }
        this.transform.FindChild("mesh").GetChild(1).renderer.material = addedMaterial;// affectation du material


    }
    //appellé par les actions RAIN 
    public void EmitSound()
    {

        fragmentBubble.BubblingIn(); // fait apparaitre la bulle 
    }

    // Assignateurs pour les variables RAIN----------------
    public void SetJustReceivedFragmentComplete(bool boolean)
    {
        isFragmentComplete = boolean;
        tMemory.SetItem<bool>("justReceivedFragment", boolean);
    }

    public void SetIsFragmentComplete(bool boolean)
    {
        isFragmentComplete = boolean;
        tMemory.SetItem<bool>("isFragmentComplete", boolean);
    }

    public void SetPlayerIsInRange(bool boolean)
    {
        tMemory.SetItem<bool>("playerIsInRange", boolean);
    }

    public void SetTargetLookAt(GameObject lookAtTarget)
    {
        tMemory.SetItem<GameObject>("lookAtTarget", lookAtTarget);
    }
    //-------------------------------------------------
}
