using UnityEngine;
using mlv;
using System;
using System.Collections;
using System.Collections.Generic;


[SerializeField]
public class ZoneDetection : MonoBehaviour {


	public bool useTrigger = true;
	public bool inIP = false;
	public bool inEvent = false;
	public bool nearEntity = false;
    public string nameIP = "null";
    public string nameEvent = "null";
    private string ancLS = "none";
    public GameObject[] LifeSceneNames;
    public GameObject newLS;
    public int numberLS = -1;
    public string buttonText = "Lock Inspector and Select LifeScenes";
    public bool order = false;
    public bool repeat = false;

	private GameObject[] LifeScene;
	private Entity entity;	
	private bool init = false;

	void Awake () 
	{
		LifeScene = GameObject.FindGameObjectsWithTag( "LifeScene" );
		entity = GetComponent< Entity >();

		useTrigger = true;
		inIP = false;
		inEvent = false;
		init = false;
		nearEntity = false;
        nameIP = "null";
        nameEvent = "null";
        ancLS = "none";
        UnityEngine.Random.seed = 42;
    }
	
	void Update () 
	{
		if (entity.enabled){
		    if(init == false){

				UInt32 EventProp;
				if(!entity.getWorkingKnowledge().hasProperty("Navigation")){
					EventProp = entity.getWorkingKnowledge().addProperty("Navigation");
				} else {
					EventProp = entity.getWorkingKnowledge().getProperty("Navigation");
				}

                UInt32 param;
				if(!entity.getWorkingKnowledge().hasChild(EventProp, "useExternalTrigger")){
					param = entity.getWorkingKnowledge().addChild(EventProp, "useExternalTrigger");
					entity.getWorkingKnowledge().setBool(param, useTrigger);
				}
                if (!entity.getWorkingKnowledge().hasChild(EventProp, "inZone"))
                {
                    param = entity.getWorkingKnowledge().addChild(EventProp, "inZone");
                    entity.getWorkingKnowledge().setInt(param, -1);
                }

                if (!entity.getWorkingKnowledge().hasProperty("LifeScenes.orderChoice"))
                {
                    EventProp = entity.getWorkingKnowledge().addProperty("LifeScenes.orderChoice");
                }
                else
                {
                    EventProp = entity.getWorkingKnowledge().getProperty("LifeScenes.orderChoice");
                }
                entity.getWorkingKnowledge().setBool(EventProp, order);

                if (!entity.getWorkingKnowledge().hasProperty("LifeScenes.repeatOrder"))
                {
                    EventProp = entity.getWorkingKnowledge().addProperty("LifeScenes.repeatOrder");
                }
                else
                {
                    EventProp = entity.getWorkingKnowledge().getProperty("LifeScenes.repeatOrder");
                }
                entity.getWorkingKnowledge().setBool(EventProp, repeat);

                if (!entity.getWorkingKnowledge().hasProperty("LifeScenes.Active"))
                {
                    EventProp = entity.getWorkingKnowledge().addProperty("LifeScenes.Active");
                }
                else
                {
                    EventProp = entity.getWorkingKnowledge().getProperty("LifeScenes.Active");
                }

                entity.getWorkingKnowledge().resize(EventProp, LifeSceneNames.Length);
                for (int j = 0; j < LifeSceneNames.Length; j++)
                {
                    if (!entity.getWorkingKnowledge().hasChild(EventProp, "Name"))
                    {
                        param = entity.getWorkingKnowledge().addChild(entity.getWorkingKnowledge().getChild(EventProp, j), "Name");
                        entity.getWorkingKnowledge().setString(param, LifeSceneNames[j].name);
                    }
                 }

				init = true;
			}
		}
	}

	void OnTriggerEnter (Collider other){
		if(useTrigger == true && entity.enabled){
			for(int i = 0; i < LifeScene.Length; i++){
                if (other.gameObject == LifeScene[i] && LifeScene[i].GetComponent<Entity>().getWorkingKnowledge().getBool("LifeScene.Temporary") == false && ancLS != LifeScene[i].name) // 
                { 
                    entity.getWorkingKnowledge().setInt("IP.inIP", 0);
                    Vector3 position = LifeScene[i].transform.position;
					entity.getWorkingKnowledge().put("IP.position", position);
                    nameIP = LifeScene[i].name;
                    ancLS = LifeScene[i].name;
                    entity.getWorkingKnowledge().setString("Navigation.location", nameIP);
                    entity.getWorkingKnowledge().setInt("Navigation.inZone", 0);
                    //entity.getWorkingKnowledge().setString("Navigation.goal", "null");
 					inIP = true;
				}
                if (other.gameObject == LifeScene[i] && LifeScene[i].GetComponent<Entity>().getWorkingKnowledge().getBool("LifeScene.Activated") == true && LifeScene[i].GetComponent<Entity>().getWorkingKnowledge().getBool("LifeScene.Temporary") == true && ancLS != LifeScene[i].name)
                {
                    entity.getWorkingKnowledge().setInt("Events.inEvent", 0);
 		            nameEvent = LifeScene[i].name;
                    ancLS = LifeScene[i].name;
                    Vector3 position = LifeScene[i].transform.position;
                    entity.getWorkingKnowledge().put("Events.position", position);
                    entity.getWorkingKnowledge().setString("Navigation.location", nameEvent);
                    entity.getWorkingKnowledge().setInt("Navigation.inZone", 0);
                    //entity.getWorkingKnowledge().setString("Navigation.goal", "null");
                    inEvent = true;
				}
			}
            /*for(int i = 0; i < LifeSceneCharacter.Length; i++){
                if(other.gameObject == LifeSceneCharacter[i] && nearEntity == false){ //entity.getWorkingKnowledge().getBool("Interacting.nearEntity") == false){
                    //string nameEntity = LifeSceneCharacter[i].name;
                    InteractEntity = LifeSceneCharacter[i].GetComponent<Entity>();
                    entity.getWorkingKnowledge().setBool("Interacting.nearEntity", true);
                    entity.getWorkingKnowledge().setInt("Interacting.activeEntity", 0);
                    int IDEntity = (int)InteractEntity.entityID;
                    entity.getWorkingKnowledge().setInt("Interacting.indexInter", IDEntity);
                    nearEntity = true;
                }
            }*/
        }
	}

	void OnTriggerStay (Collider other)
	{
		if(useTrigger == true && entity.enabled){
			for(int i = 0; i < LifeScene.Length; i++){
                if (other.gameObject == LifeScene[i] && LifeScene[i].GetComponent<Entity>().getWorkingKnowledge().getBool("LifeScene.Temporary") == false && ancLS != LifeScene[i].name) // && ancLS != LifeScene[i].name  && ancNameIP != LifeScene[i].name) //entity.getWorkingKnowledge().getInt("IP.inIP") == -1){
                { 
                    entity.getWorkingKnowledge().setInt("IP.inIP", 0);
                    Vector3 position = LifeScene[i].transform.position;
					entity.getWorkingKnowledge().put("IP.position", position);
                    nameIP = LifeScene[i].name;
                    ancLS = LifeScene[i].name;
                    entity.getWorkingKnowledge().setString("Navigation.location", nameIP);
                    entity.getWorkingKnowledge().setInt("Navigation.inZone", 0);
                    //entity.getWorkingKnowledge().setString("lifeScene", nameIP);
                    //entity.getWorkingKnowledge().setString("Navigation.goal", "null");
                    inIP = true;
				}
                if (other.gameObject == LifeScene[i] && LifeScene[i].GetComponent<Entity>().getWorkingKnowledge().getBool("LifeScene.Activated") == true && LifeScene[i].GetComponent<Entity>().getWorkingKnowledge().getBool("LifeScene.Temporary") == true && ancLS != LifeScene[i].name)
                {
                    entity.getWorkingKnowledge().setInt("Events.inEvent", 0);
                    nameEvent = LifeScene[i].name;
                    ancLS = LifeScene[i].name;
                    entity.getWorkingKnowledge().setString("Navigation.location", nameEvent);
                    entity.getWorkingKnowledge().setInt("Navigation.inZone", 0);
                    //entity.getWorkingKnowledge().setString("lifeScene", nameEvent);
                    //entity.getWorkingKnowledge().setString("Navigation.goal", "null");
                    Vector3 position = LifeScene[i].transform.position;
                    entity.getWorkingKnowledge().put("Events.position", position);
                    inEvent = true;
				}
 			}
            /*for(int i = 0; i < LifeSceneCharacter.Length; i++){
                if(other.gameObject == LifeSceneCharacter[i] && nearEntity == false){ //entity.getWorkingKnowledge().getBool("Interacting.nearEntity") == false){
                    //string nameEntity = LifeSceneCharacter[i].name;
                    InteractEntity = LifeSceneCharacter[i].GetComponent<Entity>();
                    entity.getWorkingKnowledge().setBool("Interacting.nearEntity", true);
                    entity.getWorkingKnowledge().setInt("Interacting.activeEntity", 0);
                    //int IDEntity = (int)InteractEntity.entityID;
                    //entity.getWorkingKnowledge().setInt("Interacting.indexInter", IDEntity);
                    entity.getWorkingKnowledge().setInt("Interacting.indexInter", i);
                    nearEntity = true;
                }
            }*/
 
        }
	}

	void OnTriggerExit (Collider other)
	{
		if(useTrigger == true && entity.enabled){
			for(int i = 0; i < LifeScene.Length; i++){
				if(other.gameObject == LifeScene[i] &&  inIP == true && LifeScene[i].GetComponent< Entity >().getWorkingKnowledge().getBool("LifeScene.Temporary") == false){ //entity.getWorkingKnowledge().getInt("IP.inIP") != -1){
                    entity.getWorkingKnowledge().setInt("IP.inIP", -1);
                    Vector3 position = Vector3.zero; ;
					entity.getWorkingKnowledge().put("IP.position", position);
 					entity.getWorkingKnowledge().setString( "Navigation.location", "null");
                    entity.getWorkingKnowledge().setInt("Navigation.inZone", -1);
					inIP = false;
				}
                if (other.gameObject == LifeScene[i] && inEvent == true && LifeScene[i].GetComponent<Entity>().getWorkingKnowledge().getBool("LifeScene.Temporary") == true)
                {
                    entity.getWorkingKnowledge().setInt("Events.inEvent", -1);
                    entity.getWorkingKnowledge().setString("Navigation.location", "null");
                    entity.getWorkingKnowledge().setInt("Navigation.inZone", -1);
                    Vector3 position = Vector3.zero; ;
					entity.getWorkingKnowledge().put("Events.position", position);
                    inEvent = false;
				}
			}
            /*for(int i = 0; i < LifeSceneCharacter.Length; i++){
                if(other.gameObject == LifeSceneCharacter[i] && nearEntity == true){ //entity.getWorkingKnowledge().getBool("Interacting.nearEntity") == true){
                    entity.getWorkingKnowledge().setBool("Interacting.nearEntity", false);
                    entity.getWorkingKnowledge().setInt("Interacting.indexInter", -1);
                    entity.getWorkingKnowledge().setInt("Interacting.activeEntity", -1);
                    nearEntity = false;
                }
            }*/
        }
	}


}
