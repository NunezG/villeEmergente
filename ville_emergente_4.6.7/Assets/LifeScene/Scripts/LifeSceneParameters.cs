using UnityEngine;
using mlv;
using System;
using System.Collections;
using System.Collections.Generic;


public class LifeSceneParameters : MonoBehaviour {

	/*private static LifeSceneParameters s_Instance = null;
	public static LifeSceneParameters instance {
		get {
			if (s_Instance == null) {
				s_Instance =  FindObjectOfType(typeof (LifeSceneParameters)) as LifeSceneParameters;
			}          
			return s_Instance;
		}
	}  */

	public string LSname = "none";
	public float Priority = 0;
	public bool Exit = false;
	public string Behavior = "none";
    private bool init = false;
    public float size = 0;

	public RoleParameters roleParameters;
	public BehaviorParameters behaviorParameters;
	public TriggerParameters triggerParameters;
	//public TriggerTypeParameters triggerParameters;

	public StopParameters stopParameters;
	public PlaceParameters placeParameters;

	public bool Formation = false;
	public PositioningParameters Formations;
	public FormationParameters Placements;
	public MotivationParameters motivationParameters;
	public EntityRolesParameters entityRolesParameters;

	private Entity entity;
    private string interactBNPC = "none";
    private string interactBPlayer = "none";
	public bool Temporary = false;
    private bool activated = false;
    private DynamicObject entityKnowledge;

    public LifeSceneTimeLine timeLine;
    private MeshRenderer[] meshObject;
    private bool[] iterAff;
    private bool iterGO = false;

	public void Apply(){
		/*knowledge = GetComponent< Knowledge >();
		DynamicObject eKnowledge = knowledge.self.dynamicObject;
		UInt32 Essai;
		Essai = eKnowledge.addProperty("Essai");
		eKnowledge.setString(Essai, "Hello");*/
	}
	
	void Awake () 
	{
        entity = GetComponent<Entity>();
        entityKnowledge = entity.getWorkingKnowledge();
        meshObject = GetComponentsInChildren<MeshRenderer>();
        iterAff = new bool[meshObject.Length];
        for (int i = 0; i < meshObject.Length; i++)
            iterAff[i] = false;
       
        init = false;
	}

    void Update()
    {

        if (entity.enabled && init == true && entityKnowledge.getBool("LifeScene.Temporary") == true)
        {
            for (int i = 0; i < meshObject.Length; i++)
            {
                if (entityKnowledge.getBool("LifeScene.Activated") == false && iterGO == false)
                {
                    meshObject[i].enabled = false;
                    iterGO = true;
                }

                if (entityKnowledge.getBool("LifeScene.Activated") == true && iterAff[i] == false)
                {
                    meshObject[i].enabled = true;
                    iterAff[i] = true;
                }
                if (entityKnowledge.getBool("LifeScene.Activated") == false && iterAff[i] == true)
                {
                    meshObject[i].enabled = false;
                    iterAff[i] = false;
                }

            }

        }

        
        if (entity.enabled && init == false && GameObject.Find("LifeScenes") != null)
        {
            //****************************
            //LifeScene information
            //****************************
            //EditorApplication.SaveScene();

            LifeSceneManager lifeScenesLS = LifeSceneManager.Instance;
            lifeScenesLS.UpdateLS();
            if (lifeScenesLS.LifeSceneNames.Length == 1)
                Debug.LogWarning("It works better with 2 LifeScenes");
            
            UInt32 EventProp;
            if (!entityKnowledge.hasProperty("LifeScene"))
            {
                EventProp = entityKnowledge.addProperty("LifeScene");
            }
            else
            {
                EventProp = entityKnowledge.getProperty("LifeScene");
            }

            if (triggerParameters.begining == -1 || triggerParameters.duration == -1)
            {
                   Temporary = false;
                   activated = true;
            }
            else
            {
                Temporary = true;
                activated = false;
            }

            if (Placements.Formations.Length != 0)
                if (Placements.Formations[0].UnityPlaces.Length > 0 || Placements.Formations[0].PlaceGeneration.PlaceNumber > 0)
                    Formation = true;

            if(LSname.Contains("_InteractionNPC"))
                interactBNPC = behaviorParameters.Behaviors[0];
            if(LSname.Contains("_InteractionPlayer"))
                interactBPlayer = behaviorParameters.Behaviors[0];
                
            UInt32 param;
            if (!entityKnowledge.hasChild(EventProp, "Name"))
            {
                param = entityKnowledge.addChild(EventProp, "Name");
                entityKnowledge.setString(param, LSname);
            }
            if (!entityKnowledge.hasChild(EventProp, "Type"))
            {
                param = entityKnowledge.addChild(EventProp, "Type");
                entityKnowledge.setString(param, "Complex");
            }
            if (!entityKnowledge.hasChild(EventProp, "Priority"))
			{
				param = entityKnowledge.addChild(EventProp, "Priority");
				entityKnowledge.setReal(param, Priority);
			}
			if (!entityKnowledge.hasChild(EventProp, "Temporary"))
            {
                param = entityKnowledge.addChild(EventProp, "Temporary");
                entityKnowledge.setBool(param, Temporary);
            }
            if (!entityKnowledge.hasChild(EventProp, "Activated"))
            {
                param = entityKnowledge.addChild(EventProp, "Activated");
                entityKnowledge.setBool(param, activated);
            }
            if (!entityKnowledge.hasChild(EventProp, "InteractionNPC"))
            {
                param = entityKnowledge.addChild(EventProp, "InteractionNPC");
                entityKnowledge.setString(param, interactBNPC);
            }
            if (!entityKnowledge.hasChild(EventProp, "InteractionPlayer"))
            {
                param = entityKnowledge.addChild(EventProp, "InteractionPlayer");
                entityKnowledge.setString(param, interactBPlayer);
            }
            if (!entityKnowledge.hasChild(EventProp, "Position"))
            {
                Vector3 pos = transform.position;
                param = entityKnowledge.addChild(EventProp, "Position");
                entityKnowledge.put(param, pos);
            }
            if (!entityKnowledge.hasChild(EventProp, "Behavior"))
            {
                param = entityKnowledge.addChild(EventProp, "Behavior");
                if (Placements.Formations.Length < 1)
                    Behavior = "mlv:BehavioralSequence.bt";
                if (Placements.Formations.Length >= 1)
                    Behavior = "mlv:Positioning.bt";
                entityKnowledge.setString(param, Behavior);
            }
            if (!entityKnowledge.hasChild(EventProp, "Size"))
            {
                param = entityKnowledge.addChild(EventProp, "Size");
                if (GetComponent<SphereCollider>() != null)
                    size = GetComponent<SphereCollider>().radius;
                if (GetComponent<BoxCollider>() != null)
                {
                    if(GetComponent<BoxCollider>().size.x <= GetComponentInChildren<BoxCollider>().size.z)
                        size = GetComponent<BoxCollider>().size.x/2;
                    else
                        size = GetComponent<BoxCollider>().size.z/2;
                }
                entityKnowledge.setReal(param, size);

            }
            if (!entityKnowledge.hasChild(EventProp, "Formation"))
            {
                param = entityKnowledge.addChild(EventProp, "Formation");
                entityKnowledge.setBool(param, Formation);
            }

            //***************************************
            // Roles
            //**************************************
            if (roleParameters.Roles.Length == 0 || roleParameters.Roles[0] == "")
                Debug.LogError("The Roles of the LifeScene " + gameObject.name + " are not defined");

            if (roleParameters.RolesType.Length == 0 || roleParameters.RolesType[0] == "")
                Debug.LogError("The Type of Roles of the LifeScene " + gameObject.name + " are not defined");

            if (behaviorParameters.Behaviors.Length == 0 || behaviorParameters.Behaviors[0] == "")
                Debug.LogError("The Behaviors of the LifeScene " + gameObject.name + " are not defined");

            if (behaviorParameters.Roles.Length == 0 || behaviorParameters.Roles[0] == "")
                Debug.LogError("The Roles associated to Behaviors of the LifeScene " + gameObject.name + " are not defined");

            if (!entityKnowledge.hasProperty("LifeScene.Roles"))
            {
                EventProp = entityKnowledge.addProperty("LifeScene.Roles");
            }
            else
            {
                EventProp = entityKnowledge.getProperty("LifeScene.Roles");
            }

            for (int i = 0; i < roleParameters.Roles.Length; i++)
            {
                if (roleParameters.RolesType[i] == "Lead Actor")
                {
                    if (!entityKnowledge.hasChild(EventProp, "Lead Actor"))
                    {
                        param = entityKnowledge.addChild(EventProp, "Lead Actor");
                        entityKnowledge.setString(param, roleParameters.Roles[i]);
                    }
                }
                if (roleParameters.RolesType[i] == "Actor")
                {
                    if (!entityKnowledge.hasChild(EventProp, "Actor"))
                    {
                        param = entityKnowledge.addChild(EventProp, "Actor");
                        entityKnowledge.setString(param, roleParameters.Roles[i]);
                    }
                    if (!entityKnowledge.hasChild(EventProp, "Follow"))
                    {
                        param = entityKnowledge.addChild(EventProp, "Follow");
                        entityKnowledge.setBool(param, roleParameters.follow[i]);
                    }
                }
                if (roleParameters.RolesType[i] == "Extra")
                {
                    if (!entityKnowledge.hasChild(EventProp, "Extra"))
                    {
                        param = entityKnowledge.addChild(EventProp, "Extra");
                        entityKnowledge.setString(param, roleParameters.Roles[i]);
                    }
                }
            }


            //****************************
            //TimeLine
            //****************************
            if (timeLine.timeLineForRoles == null || timeLine.timeLineForRoles.Length == 0)
                Debug.LogError("The Timeline of the LifeScene " +gameObject.name +" is not defined with Roles and Behaviors");

            if (timeLine.timeLineForRoles.Length > 0 && timeLine.timeLineForRoles[0].roleName == "")
                Debug.LogError("The Roles in the Timeline of the LifeScene " + gameObject.name + " are not defined");

            if (timeLine.timeLineForRoles.Length > 0 && timeLine.timeLineForRoles[0].sequence.Length <= 1)
                Debug.LogError("The Behaviors in the Timeline of the LifeScene " + gameObject.name + " are not defined");

            if (timeLine.timeLineForRoles.Length > 0 && timeLine.timeLineForRoles[0].roleName != "none" && timeLine.timeLineForRoles[0].roleName != "")
            {
                for (int i = 0; i < timeLine.timeLineForRoles.Length; i++)
                {
                    if (timeLine.timeLineForRoles[i].roleName != "none" || timeLine.timeLineForRoles[i].roleName != "" && roleParameters.Roles[0] != "")
                    {
                        if (!entityKnowledge.hasProperty("LifeScene.Timeline"))
                        {
                            EventProp = entityKnowledge.addProperty("LifeScene.Timeline");
                        }
                        else
                        {
                            EventProp = entityKnowledge.getProperty("LifeScene.Timeline");
                        }

                        UInt32 param1;
                        if (!entityKnowledge.hasChild(EventProp, "Roles"))
                        {
                            param1 = entityKnowledge.addChild(EventProp, "Roles");
                        }
                        else
                            param1 = entityKnowledge.getChild(EventProp, "Roles");

                        string nameRol = "none";
                        string typeRol = "none";
                        entityKnowledge.resize(param1, timeLine.timeLineForRoles.Length);
                        for (int j = 0; j < timeLine.timeLineForRoles.Length; j++)
                        {
                            if (!entityKnowledge.hasChild(param1, "Name"))
                            {
                                param = entityKnowledge.addChild(entityKnowledge.getChild(param1, j), "Name");
                                nameRol = timeLine.timeLineForRoles[j].roleName;
                                /*if (roleParameters.Roles.Length == 1)
                                    entityKnowledge.setString(param, "_Ambient");
                                else*/
                                    entityKnowledge.setString(param, nameRol);
                            }
                            for (int k = 0; k < roleParameters.Roles.Length; k++)
                            {
                                if (roleParameters.Roles[k] == nameRol)
                                {
                                    if (!entityKnowledge.hasChild(param1, "Type"))
                                    {
                                        param = entityKnowledge.addChild(entityKnowledge.getChild(param1, j), "Type");
                                        typeRol = roleParameters.RolesType[k];
                                        if (roleParameters.Roles.Length <= 1)
                                        {
                                            entityKnowledge.setString(param, "Ambient");
                                            entityKnowledge.setString("LifeScene.Type", "Simple");
                                        }
                                        else
                                            entityKnowledge.setString(param, typeRol);
                                        
                                    }
                                }
                            }


                            UInt32 param2;
                            if (!entityKnowledge.hasChild(param1, "Sequence"))
                            {
                                param2 = entityKnowledge.addChild(entityKnowledge.getChild(param1, j), "Sequence");
                            }
                            else
                                param2 = entityKnowledge.getChild(entityKnowledge.getChild(param1, j), "Sequence");

                            entityKnowledge.resize(param2, timeLine.timeLineForRoles[j].sequence.Length);
                            for (int k = 0; k < timeLine.timeLineForRoles[j].sequence.Length; k++)
                            {
                                string kind = timeLine.timeLineForRoles[j].sequence[k].kind.ToString();
                                if (!entityKnowledge.hasChild(param2, "Kind"))
                                {
                                    param = entityKnowledge.addChild(entityKnowledge.getChild(param2, k), "kind");
                                    entityKnowledge.setString(param, kind);
                                }
                                if (kind != "Multiple")
                                {
                                    if (!entityKnowledge.hasChild(param2, "uriBT"))
                                    {
                                        param = entityKnowledge.addChild(entityKnowledge.getChild(param2, k), "uriBT");
                                        if (timeLine.timeLineForRoles[j].sequence[k].kind.ToString() == "Empty" || timeLine.timeLineForRoles[j].sequence[k].behavior.uriBT == "")
                                            entityKnowledge.setString(param, "mlv:Waiting.bt");
                                        else
                                            entityKnowledge.setString(param, timeLine.timeLineForRoles[j].sequence[k].behavior.uriBT);
                                    }
                                    if (!entityKnowledge.hasChild(param2, "interuptible"))
                                    {
                                        param = entityKnowledge.addChild(entityKnowledge.getChild(param2, k), "interuptible");
                                        entityKnowledge.setBool(param, timeLine.timeLineForRoles[j].sequence[k].behavior.interuptible);
                                    }
                                    if (!entityKnowledge.hasChild(param2, "loopable"))
                                    {
                                        param = entityKnowledge.addChild(entityKnowledge.getChild(param2, k), "loopable");
                                        entityKnowledge.setBool(param, timeLine.timeLineForRoles[j].sequence[k].behavior.loopable);
                                    }
                                }
                                if (kind == "Multiple")
                                {
                                    UInt32 param3;
                                    if (!entityKnowledge.hasChild(param2, "Multiple"))
                                    {
                                        param3 = entityKnowledge.addChild(entityKnowledge.getChild(param2, k), "Multiple");
                                    }
                                    else
                                        param3 = entityKnowledge.getChild(entityKnowledge.getChild(param2, k), "Multiple");

                                    entityKnowledge.resize(param3, timeLine.timeLineForRoles[j].sequence[k].behaviors.Length);
                                    for (int m = 0; m < timeLine.timeLineForRoles[j].sequence[k].behaviors.Length; m++)
                                    {
                                        if (!entityKnowledge.hasChild(param3, "uriBT"))
                                        {
                                            param = entityKnowledge.addChild(entityKnowledge.getChild(param3, m), "uriBT");
                                            entityKnowledge.setString(param, timeLine.timeLineForRoles[j].sequence[k].behaviors[m].uriBT);
                                        }
                                        if (!entityKnowledge.hasChild(param3, "probability"))
                                        {
                                            param = entityKnowledge.addChild(entityKnowledge.getChild(param3, m), "probability");
                                            entityKnowledge.setReal(param, timeLine.timeLineForRoles[j].sequence[k].behaviors[m].probability);
                                        }
                                        if (!entityKnowledge.hasChild(param3, "random"))
                                        {
                                            param = entityKnowledge.addChild(entityKnowledge.getChild(param3, m), "random");
                                            entityKnowledge.setBool(param, timeLine.timeLineForRoles[j].sequence[k].isRandom);
                                        }
                                        if (!entityKnowledge.hasChild(param3, "interuptible"))
                                        {
                                            param = entityKnowledge.addChild(entityKnowledge.getChild(param3, m), "interuptible");
                                            entityKnowledge.setBool(param, timeLine.timeLineForRoles[j].sequence[k].behaviors[m].interuptible);
                                        }
                                        if (!entityKnowledge.hasChild(param3, "loopable"))
                                        {
                                            param = entityKnowledge.addChild(entityKnowledge.getChild(param3, m), "loopable");
                                            entityKnowledge.setBool(param, timeLine.timeLineForRoles[j].sequence[k].behaviors[m].loopable);
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }

            //****************************
            //Event information
            //****************************
            if (Temporary == true)
            {
                if (!entityKnowledge.hasProperty("LifeScene.Event"))
                {
                    EventProp = entityKnowledge.addProperty("LifeScene.Event");
                }
                else
                {
                    EventProp = entityKnowledge.getProperty("LifeScene.Event");
                }

                if (!entityKnowledge.hasChild(EventProp, "Duration"))
                {
                    param = entityKnowledge.addChild(EventProp, "Duration");
                    entityKnowledge.setReal(param, triggerParameters.duration);
                }
                if (!entityKnowledge.hasChild(EventProp, "Repetition"))
                {
                    param = entityKnowledge.addChild(EventProp, "Repetition");
                    entityKnowledge.setBool(param, triggerParameters.repetition);
                }
                if (!entityKnowledge.hasChild(EventProp, "RepetitionTime"))
                {
                    param = entityKnowledge.addChild(EventProp, "RepetitionTime");
                    entityKnowledge.setReal(param, triggerParameters.repetitionTime);
                }
                if (!entityKnowledge.hasChild(EventProp, "TriggerType"))
                {
                    string typeTriger = triggerParameters.type.ToString();
                    param = entityKnowledge.addChild(EventProp, "TriggerType");
                    entityKnowledge.setString(param, typeTriger);
                }

                if (triggerParameters.Place != null)
                {
                    if (!entityKnowledge.hasChild(EventProp, "TriggerRole"))
                    {
                        param = entityKnowledge.addChild(EventProp, "TriggerRole");
                        entityKnowledge.setString(param, triggerParameters.Place.allowedRole);
                    }
                    if (!entityKnowledge.hasChild(EventProp, "TriggerAlignment"))
                    {
                        param = entityKnowledge.addChild(EventProp, "TriggerAlignment");
                        entityKnowledge.setReal(param, triggerParameters.Place.transform.rotation.eulerAngles.y);
                    }
                    if (!entityKnowledge.hasChild(EventProp, "TriggerPosition"))
                    {
                        param = entityKnowledge.addChild(EventProp, "TriggerPosition");
                        if (triggerParameters.Place != null)
                        {
                            Vector3 pos1 = triggerParameters.Place.transform.position;
                            entityKnowledge.put(param, pos1);
                        }
                        else
                            Debug.LogWarning("You should specify the place of the Trigger Object/Entity in the Trigger Conditions  of the " + gameObject.name + " lifeScene");

                    }
                }

                if (!entityKnowledge.hasChild(EventProp, "TriggerBegining"))
                {
                    param = entityKnowledge.addChild(EventProp, "TriggerBegining");
                    entityKnowledge.setReal(param, triggerParameters.begining);
                }

                if (!entityKnowledge.hasChild(EventProp, "TriggerName"))
                {
                    param = entityKnowledge.addChild(EventProp, "TriggerName");
                    if (triggerParameters.entity != null)
                        entityKnowledge.setString(param, triggerParameters.entity.name);
                    else
                        Debug.LogError("You should specify the Trigger Object/Entity in the Trigger Conditions  of the " + gameObject.name + " lifeScene");
                }
                if (!entityKnowledge.hasChild(EventProp, "TriggerIndex"))
                {
                    param = entityKnowledge.addChild(EventProp, "TriggerIndex");
                    entityKnowledge.setInt(param, -1);
                }
                if (timeLine.timeLineForRoles.Length > 0)
                {
                    if (!entityKnowledge.hasChild(EventProp, "TriggerBehavior"))
                    {
                        param = entityKnowledge.addChild(EventProp, "TriggerBehavior");
                        if (timeLine.timeLineForRoles[0].roleName == "" || timeLine.timeLineForRoles[0].roleName == "none")
                            entityKnowledge.setString(param, behaviorParameters.Behaviors[0]);
                        else if (timeLine.timeLineForRoles.Length > 1)
                            entityKnowledge.setString(param, "mlv:BehavioralSequence.bt");

                        //entityKnowledge.setString(param, triggerParameters.behavior);
                    }
                }

            }

            //****************************
            //Position information
            //****************************
            if (!entityKnowledge.hasProperty("LifeScene.Positioning"))
            {
                EventProp = entityKnowledge.addProperty("LifeScene.Positioning");
            }
            else
            {
                EventProp = entityKnowledge.getProperty("LifeScene.Positioning");
            }

            if (!entityKnowledge.hasChild(EventProp, "AmbientDistance"))
            {
                param = entityKnowledge.addChild(EventProp, "AmbientDistance");
                entityKnowledge.setReal(param, size - Placements.distanceAmbient);
            }
            if (placeParameters.UnityPlaces.Length > 0 && placeParameters.UnityPlaces[0] != null)
            {
                UInt32 param1;
                if (!entityKnowledge.hasChild(EventProp, "Position"))
                {
                    param1 = entityKnowledge.addChild(EventProp, "Position");
                }
                else
                    param1 = entityKnowledge.getChild(EventProp, "Position");

                entityKnowledge.resize(param1, placeParameters.UnityPlaces.Length);
                for (int i = 0; i < placeParameters.UnityPlaces.Length; i++)
                {
                    if (placeParameters.UnityPlaces[i] != null)
                    {
                        if (!entityKnowledge.hasChild(param1, "Name"))
                        {
                            param = entityKnowledge.addChild(entityKnowledge.getChild(param1, i), "Name");
                            entityKnowledge.setString(param, placeParameters.UnityPlaces[i].name);
                        }
                        if (!entityKnowledge.hasChild(param1, "Position"))
                        {
                            Vector3 pos1 = placeParameters.UnityPlaces[i].transform.position;
                            param = entityKnowledge.addChild(entityKnowledge.getChild(param1, i), "Position");
                            entityKnowledge.put(param, pos1);
                        }
                        if (!entityKnowledge.hasChild(param1, "AgentOrientation"))
                        {
                            param = entityKnowledge.addChild(entityKnowledge.getChild(param1, i), "AgentOrientation");
                            //double rot = Placements.Formations[0].UnityPlaces[i].transform.rotation.eulerAngles.y;
                            float rot = placeParameters.UnityPlaces[i].transform.rotation.eulerAngles.y;
                            entityKnowledge.setReal(param, rot);
                        }
                    }
                }
            }
          
            //****************************
            //Positioning information
            //****************************
            if (Placements.Formations.Length > 0){
                for(int p = 0; p < Placements.Formations.Length; p++){
                    if (Placements.Formations[p].ConcernedRole == "none" )
                        Debug.LogWarning("You should complete the Parameters of the Placement " + (p + 1) + " (concerned role) in the Formations Menu of the " + gameObject.name + " lifeScene");
                    if(Placements.Formations[p].UnityPlace == true && Placements.Formations[p].UnityPlaces.Length == 0)
                        Debug.LogWarning("You should complete the Parameters of the Placement " + (p + 1) + " (unity places) in the Formations Menu  of the " + gameObject.name + " lifeScene");
                    if (Placements.Formations[p].GenerationOfPlace == true && Placements.Formations[p].PlaceGeneration.PlaceNumber == 0)
                        Debug.LogWarning("You should complete the Parameters of the Placement " + (p + 1) + " (number of places) in the Formations Menu  of the " + gameObject.name + " lifeScene");
                }
            }

            if (Formation == true)
            {
                if (!entityKnowledge.hasProperty("LifeScene.Positioning"))
                {
                    EventProp = entityKnowledge.addProperty("LifeScene.Positioning");
                }
                else
                {
                    EventProp = entityKnowledge.getProperty("LifeScene.Positioning");
                }

                if (Placements.Formations.Length > 0 && Placements.Formations[0] != null)
                {
                    UInt32 param1;
                    if (!entityKnowledge.hasChild(EventProp, "Placement"))
                    {
                        param1 = entityKnowledge.addChild(EventProp, "Placement");
                    }
                    else
                        param1 = entityKnowledge.getChild(EventProp, "Placement");

                    entityKnowledge.resize(param1, Placements.Formations.Length);
                    for (int i = 0; i < Placements.Formations.Length; i++)
                    {
                        if (Placements.Formations[i] != null)
                        {
                            if (!entityKnowledge.hasChild(param1, "Type"))
                            {
                                string typeString = Placements.Formations[i].TypeForm.ToString();
                                param = entityKnowledge.addChild(entityKnowledge.getChild(param1, i), "Type");
                                entityKnowledge.setString(param, typeString);
                            }
                            if (!entityKnowledge.hasChild(param1, "ConcernedRole"))
                            {
                                param = entityKnowledge.addChild(entityKnowledge.getChild(param1, i), "ConcernedRole");
                                entityKnowledge.setString(param, Placements.Formations[i].ConcernedRole);
                            }
                            if (!entityKnowledge.hasChild(param1, "fullBehavior"))
                            {
                                param = entityKnowledge.addChild(entityKnowledge.getChild(param1, i), "fullBehavior");
                                entityKnowledge.setString(param, Placements.Formations[i].fullBehavior);
                            }
                            if (!entityKnowledge.hasChild(param1, "Behavior"))
                            {
                                param = entityKnowledge.addChild(entityKnowledge.getChild(param1, i), "Behavior");
                                /*if (timeLine.timeLineForRoles[0].roleName == "" || timeLine.timeLineForRoles[0].roleName == "none" || timeLine.timeLineForRoles[0].roleName == "Default" || roleParameters.Roles.Length == 1)
                                    entityKnowledge.setString(param, behaviorParameters.Behaviors[0]);
                                else*/
                                entityKnowledge.setString(param, "mlv:BehavioralSequence.bt");

                                //entityKnowledge.setString(param, Placements.Formations[0].Behavior);
                            }
                            if (!entityKnowledge.hasChild(param1, "GenerationOfPlace"))
                            {
                                param = entityKnowledge.addChild(entityKnowledge.getChild(param1, i), "GenerationOfPlace");
                                entityKnowledge.setBool(param, Placements.Formations[i].GenerationOfPlace);
                            }
                            if (!entityKnowledge.hasChild(param1, "CustomPlaces"))
                            {
                                param = entityKnowledge.addChild(entityKnowledge.getChild(param1, i), "CustomPlaces");
                                entityKnowledge.setBool(param, Placements.Formations[i].UnityPlace);
                            }

                            //*******************************
                            //Place Generation information
                            //*******************************	
                            if (Placements.Formations[i].GenerationOfPlace == true)
                            {
                                if (!entityKnowledge.hasChild(param1, "PlaceGeneration"))
                                {
                                    param = entityKnowledge.addChild(entityKnowledge.getChild(param1, i), "PlaceGeneration");

                                } else
                                    param = entityKnowledge.getChild(entityKnowledge.getChild(param1, i), "PlaceGeneration");

                                UInt32 param2;
                                if (!entityKnowledge.hasChild(param, "Position"))
                                {
                                    if (Placements.Formations[i].PlaceGeneration.Position != null)
                                    {
                                        Vector3 pos1 = Placements.Formations[i].PlaceGeneration.Position.position;
                                        param2 = entityKnowledge.addChild(param, "Position");
                                        entityKnowledge.put(param2, pos1);
                                    }
                                }
                                if (!entityKnowledge.hasChild(param, "PlaceNumber"))
                                {
                                    param2 = entityKnowledge.addChild(param, "PlaceNumber");
                                    entityKnowledge.setInt(param2, Placements.Formations[i].PlaceGeneration.PlaceNumber);
                                }
                                if (!entityKnowledge.hasChild(param, "Alignment"))
                                {
                                    param2 = entityKnowledge.addChild(param, "Alignment");
                                    entityKnowledge.setReal(param2, Placements.Formations[i].PlaceGeneration.Alignment);
                                }
                                if (!entityKnowledge.hasChild(param, "LeaderDistance"))
                                {
                                    param2 = entityKnowledge.addChild(param, "LeaderDistance");
                                    entityKnowledge.setReal(param2, Placements.Formations[i].PlaceGeneration.LeaderDistance);
                                }
                                if (!entityKnowledge.hasChild(param, "AgentDistance"))
                                {
                                    param2 = entityKnowledge.addChild(param, "AgentDistance");
                                    entityKnowledge.setReal(param2, Placements.Formations[i].PlaceGeneration.AgentDistance);
                                }
                                if (!entityKnowledge.hasChild(param, "AgentOrientation"))
                                {
                                    param2 = entityKnowledge.addChild(param, "AgentOrientation");
                                    entityKnowledge.setReal(param2, Placements.Formations[i].PlaceGeneration.AgentOrientation);
                                }

                            }

                            //****************************
                            //Positioning information
                            //****************************
                            if (Placements.Formations[i].UnityPlace == true)
                            {
                                if (!entityKnowledge.hasChild(param1, "ExternalPlaces"))
                                {
                                    param = entityKnowledge.addChild(entityKnowledge.getChild(param1, i), "ExternalPlaces");

                                }
                                else
                                    param = entityKnowledge.getChild(entityKnowledge.getChild(param1, i), "ExternalPlaces");
                                
                                UInt32 param2;
                                if (!entityKnowledge.hasChild(param, "ExternalPlaceNumber"))
                                {
                                    param2 = entityKnowledge.addChild(param, "ExternalPlaceNumber");
                                    entityKnowledge.setInt(param2, Placements.Formations[i].UnityPlaces.Length);
                                }

                                if (!entityKnowledge.hasChild(param, "Places"))
                                {
                                    param2 = entityKnowledge.addChild(param, "Places");
                                }
                                else
                                    param2 = entityKnowledge.getChild(param, "Places");

                                UInt32 param3;
                                entityKnowledge.resize(param2, Placements.Formations[i].UnityPlaces.Length);
                                for (int j = 0; j < Placements.Formations[i].UnityPlaces.Length; j++)
                                {
                                    if (!entityKnowledge.hasChild(param2, "Name") && Placements.Formations[i].UnityPlaces[j] != null)
                                    {
                                        param3 = entityKnowledge.addChild(entityKnowledge.getChild(param2, j), "Name");
                                        entityKnowledge.setString(param3, Placements.Formations[i].UnityPlaces[j].name);
                                    }
                                    if (!entityKnowledge.hasChild(param2, "Position") && Placements.Formations[i].UnityPlaces[j] != null)
                                    {
                                        Vector3 pos1 = Placements.Formations[i].UnityPlaces[j].transform.position;
                                        param3 = entityKnowledge.addChild(entityKnowledge.getChild(param2, j), "Position");
                                        entityKnowledge.put(param3, pos1);
                                    }
                                    if (!entityKnowledge.hasChild(param2, "AgentOrientation") && Placements.Formations[i].UnityPlaces[j] != null)
                                    {
                                        param3 = entityKnowledge.addChild(entityKnowledge.getChild(param2, j), "AgentOrientation");
                                        //double rot = Placements.Formations[0].UnityPlaces[i].transform.rotation.eulerAngles.y;
                                        float rot = Placements.Formations[i].UnityPlaces[j].transform.rotation.eulerAngles.y;
                                        entityKnowledge.setReal(param3, rot);
                                    }
                                }
                            }
                        }

                    }
                }




            }
            init = true;
        }
    }
}
