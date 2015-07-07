using UnityEngine;
using System.Collections;
using mlv;
using System;
using System.Collections.Generic;

public class InteractionDetection : MonoBehaviour
{

    private List<bool> nearEntities = new List<bool>();
    private List<bool> nearPlayers = new List<bool>();
    private List<int> indexCharact = new List<int>();
    private List<int> ancIndex = new List<int>();

    public bool nearEntity = false;
    public bool nearPlayer = false;
    Entity entity = null;
    private uint indexPlayer = 0;
    private uint indexInteract = 0;
    //private uint ancIndex = 1000;

    private GameObject[] LifeSceneCharacter;
    private GameObject[] LifeScenePlayer;
    //private SphereCollider col;							// Reference to the sphere collider trigger component.
    //private Entity InteractEntity;
    public double distanceEntity= 2.5;
    public double distancePlayer = 2.5;
    public int probabilityInteraction = 100;
    public int interactionLimite = 2;
    public float ViewAngleEntity = 175;
    public float ViewAnglePlayer = 175;

    private int indexInter = -1;
    private int countE = 0;
    private int countP = 0;
    private bool init = false;


    void OnEnable()
    {
        Invoke("StartDetection", 0.5f);
        
    }

    void StartDetection()
    {
        if(entity.enabled == false)
            entity.enabled = true;

        if (init == false)
        {
            UInt32 EventProp;
            if (!entity.getWorkingKnowledge().hasProperty("Interacting"))
            {
                EventProp = entity.getWorkingKnowledge().addProperty("Interacting");
            }
            else
            {
                EventProp = entity.getWorkingKnowledge().getProperty("Interacting");
            }

            UInt32 param;
            if (!entity.getWorkingKnowledge().hasChild(EventProp, "nearEntity"))
            {
                param = entity.getWorkingKnowledge().addChild(EventProp, "nearEntity");
                entity.getWorkingKnowledge().setBool(param, nearEntity);
            }
            if (!entity.getWorkingKnowledge().hasChild(EventProp, "activePlayer"))
            {
                param = entity.getWorkingKnowledge().addChild(EventProp, "activePlayer");
                entity.getWorkingKnowledge().setInt(param, -1);
            }
            if (!entity.getWorkingKnowledge().hasChild(EventProp, "activeEntity"))
            {
                param = entity.getWorkingKnowledge().addChild(EventProp, "activeEntity");
                entity.getWorkingKnowledge().setInt(param, -1);
            }
            if (!entity.getWorkingKnowledge().hasChild(EventProp, "probability"))
            {
                param = entity.getWorkingKnowledge().addChild(EventProp, "probability");
                entity.getWorkingKnowledge().setInt(param, probabilityInteraction);
            }
            if (!entity.getWorkingKnowledge().hasChild(EventProp, "nearPlayer"))
            {
                param = entity.getWorkingKnowledge().addChild(EventProp, "nearPlayer");
                entity.getWorkingKnowledge().setBool(param, nearPlayer);
            }
            if (!entity.getWorkingKnowledge().hasChild(EventProp, "negociate"))
            {
                param = entity.getWorkingKnowledge().addChild(EventProp, "negociate");
                entity.getWorkingKnowledge().setBool(param, false);
            }
            if (!entity.getWorkingKnowledge().hasChild(EventProp, "indexInter"))
            {
                param = entity.getWorkingKnowledge().addChild(EventProp, "indexInter");
                entity.getWorkingKnowledge().setInt(param, indexInter);
            }
            if (!entity.getWorkingKnowledge().hasChild(EventProp, "indexPlayer"))
            {
                param = entity.getWorkingKnowledge().addChild(EventProp, "indexPlayer");
                entity.getWorkingKnowledge().setInt(param, (int)indexPlayer);
            }
            init = true;
        }
    }
    
    void Awake()
    {
        LifeSceneCharacter = GameObject.FindGameObjectsWithTag("Character");
        LifeScenePlayer = GameObject.FindGameObjectsWithTag("Player");
        entity = GetComponent<Entity>();

        for (int i = 0; i < LifeSceneCharacter.Length; i++)
        {
            nearEntities.Add(false);
            ancIndex.Add(-2);
            indexCharact.Add(-1);
        }

        for (int i = 0; i < LifeScenePlayer.Length; i++)
            nearPlayers.Add(false);

        //col = GetComponent<SphereCollider>();
        //col = transform.FindChild("Merchant").GetComponent<SphereCollider>();
    }


    void Update()
    {

        if (entity.enabled) { 
            
            //NearEntity
            for (int i = 0; i < LifeSceneCharacter.Length; i++)
            {
                Vector3 directionE = LifeSceneCharacter[i].transform.position - transform.position;
			    float angleE = Vector3.Angle(directionE, transform.forward);
			
			    // If the angle between forward and where the player is, is less than half the angle of view...
                if (nearEntities[i] == false && LifeSceneCharacter[i] != gameObject && ancIndex[i] != indexCharact[i] && Vector3.Distance(transform.position, LifeSceneCharacter[i].transform.position) < distanceEntity && angleE < ViewAngleEntity * 0.5f)  //other.gameObject == LifeSceneCharacter[i] && nearEntity == false)
                //if (Vector3.Distance(transform.position, LifeSceneCharacter[i].transform.position) < distanceEntity && nearEntities[i] == false && LifeSceneCharacter[i] != gameObject && ancIndex[i] != indexCharact[i])  //other.gameObject == LifeSceneCharacter[i] && nearEntity == false)
                {
                    //string nameEntity = LifeSceneCharacter[i].name;
                    //InteractEntity = LifeSceneCharacter[i].GetComponent<Entity>();
                    nearEntities[i] = true;
                    indexCharact[i] = (int)LifeSceneCharacter[i].GetComponent<Entity>().entityID;
                    ancIndex[i] = (int)LifeSceneCharacter[i].GetComponent<Entity>().entityID;
                    indexInteract = LifeSceneCharacter[i].GetComponent<Entity>().entityID;
                }

                //if (Vector3.Distance(transform.position, LifeSceneCharacter[i].transform.position) >= distanceEntity + 1 && nearEntities[i] == true && LifeSceneCharacter[i] != gameObject)  //other.gameObject == LifeSceneCharacter[i] && nearEntity == false)
                if (nearEntities[i] == true && LifeSceneCharacter[i] != gameObject && Vector3.Distance(transform.position, LifeSceneCharacter[i].transform.position) > distanceEntity + 1 && angleE > ViewAngleEntity * 0.5f + 1)  //other.gameObject == LifeSceneCharacter[i] && nearEntity == false)
                {
                    nearEntities[i] = false;
                    indexCharact[i] = -1;
                }
            }

            nearEntity = false;
            countE = 0;
            for (int i = 0; i < LifeSceneCharacter.Length; i++)
            {
                if (nearEntities[i] == true)
                    countE = countE + 1;
            }
            if (countE > 0 && countE < interactionLimite && entity.getWorkingKnowledge().getBool("Interacting.nearEntity") == false)
            {
                entity.getWorkingKnowledge().setBool("Interacting.nearEntity", true);
                entity.getWorkingKnowledge().setBool("Interacting.negociate", true);
                entity.getWorkingKnowledge().setInt("Interacting.indexInter", (int)indexInteract);
                //entity.getWorkingKnowledge().setInt("Interacting.activeEntity", 0);
                nearEntity = true;
                //ancIndex = indexCharact;
            }
            if (countE != 1 && entity.getWorkingKnowledge().getBool("Interacting.nearEntity") == true)
            {
               entity.getWorkingKnowledge().setBool("Interacting.negociate", false);
             }
            if (countE >= interactionLimite && entity.getWorkingKnowledge().getBool("Interacting.nearEntity") == true)
            {
                entity.getWorkingKnowledge().setBool("Interacting.nearEntity", false);
                entity.getWorkingKnowledge().setInt("Interacting.indexInter", -1);
                entity.getWorkingKnowledge().setInt("Interacting.activeEntity", -1);
                nearEntity = false;
                for(int j = 0; j < ancIndex.Count; j++)
                    ancIndex[j] = -2;
            }
            if (countE == 0 && entity.getWorkingKnowledge().getBool("Interacting.nearEntity") == true)
            {
                entity.getWorkingKnowledge().setBool("Interacting.nearEntity", false);
                entity.getWorkingKnowledge().setInt("Interacting.indexInter", -1);
                entity.getWorkingKnowledge().setInt("Interacting.activeEntity", -1);
                nearEntity = false;
                for (int j = 0; j < ancIndex.Count; j++)
                    ancIndex[j] = -2;
            }

            //NearPlayer
            for (int i = 0; i < LifeScenePlayer.Length; i++)
            {
                Vector3 directionP = LifeScenePlayer[i].transform.position - transform.position;
                float angleP = Vector3.Angle(directionP, transform.forward);

                if (nearPlayers[i] == false && LifeScenePlayer[i] != gameObject && Vector3.Distance(transform.position, LifeScenePlayer[i].transform.position) < distancePlayer && angleP < ViewAnglePlayer * 0.5f)  //other.gameObject == LifeSceneCharacter[i] && nearEntity == false)
                {
                    //string nameEntity = LifeSceneCharacter[i].name;
                    //InteractEntity = LifeSceneCharacter[i].GetComponent<Entity>();
                    nearPlayers[i] = true;
                    indexPlayer = LifeScenePlayer[i].GetComponent<Entity>().entityID;
 
                }
                if (nearPlayers[i] == true && LifeScenePlayer[i] != gameObject && Vector3.Distance(transform.position, LifeScenePlayer[i].transform.position) > distancePlayer + 1 && angleP < ViewAnglePlayer * 0.5f + 1)  //other.gameObject == LifeSceneCharacter[i] && nearEntity == false)
                {
                    nearPlayers[i] = false;
                }
            }
            


            nearPlayer = false;
            countP = 0;
            for (int i = 0; i < LifeScenePlayer.Length; i++)
            {
                if (nearPlayers[i] == true)
                    countP = countP + 1;
            }
            if (countP == 1 && entity.getWorkingKnowledge().getBool("Interacting.nearPlayer") == false)
            {
                entity.getWorkingKnowledge().setBool("Interacting.nearPlayer", true);
                entity.getWorkingKnowledge().setInt("Interacting.indexPlayer", (int)indexPlayer);
                nearPlayer = true;
            }
            if (countP != 1 && entity.getWorkingKnowledge().getBool("Interacting.nearPlayer") == true)
            {
               //nearPlayer = false;
            }
            if (countP == 0 && entity.getWorkingKnowledge().getBool("Interacting.nearPlayer") == true)
            {
                entity.getWorkingKnowledge().setInt("Interacting.activePlayer", -1);
                entity.getWorkingKnowledge().setBool("Interacting.nearPlayer", false);
                nearEntity = false;
            }
        }
        }

    /*void OnTriggerEnter(Collider other)
    {
        // If the player has entered the trigger sphere...
        for (int i = 0; i < LifeSceneCharacter.Length; i++)
        {
            if (other.gameObject == LifeSceneCharacter[i] && nearEntity == false)
            { 
                //string nameEntity = LifeSceneCharacter[i].name;
                //InteractEntity = LifeSceneCharacter[i].GetComponent<Entity>();
                nearEntity = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < LifeSceneCharacter.Length; i++)
        {
            if (other.gameObject == LifeSceneCharacter[i] && nearEntity == true)
            { 
                nearEntity = false;
            }
        }
    }*/

}
