using UnityEngine;
using UnityEditor;
using mlv;
using System;
using System.Collections;
using System.Collections.Generic;

public class NPCWindow : EditorWindow {
	private string Wname = "";
	private string Pname = "Pedestrian";
	private float delay = 0.5f;
	private bool groupEnabled;
	private GameObject characters;
	private GameObject character;
	private GameObject SmartZone;
	private GameObject SpeechBubble;
	private SerializedObject tagManager;
    private Vector3 posNPC = new Vector3 (0, 0, 0);
	//private string NPCAnimator = "NPCMover";
	private GameObject PrefabNPC;
	private CapsuleCollider zone;
	private Entity entity;
	private EnableLife ELife;
	private ZoneDetection ZDec;
	private SphereCollider smartZ;
    private List<GameObject> characts = new List<GameObject>();
    private static GUIContent cleanButtonContent = new GUIContent("-", "Remove role");
    private static GUIContent editButtonContent = new GUIContent("+", "Edit roles");
    private float size = 265;
    private List<GameObject> Asset = new List<GameObject>();

	
	// Add menu named "My Window" to the Window menu
	[MenuItem ("Window/LIFE/LifeScene/Create Character")]
    [MenuItem("MASA LIFE/LifeScene/Create Character")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		NPCWindow Nwindow = (NPCWindow)EditorWindow.GetWindow (typeof (NPCWindow), false, "Character");
		Nwindow.Focus();

	}

	void AddTag( string tag2add )
	{
		SerializedObject tagManager = new SerializedObject (AssetDatabase.LoadAllAssetsAtPath ("ProjectSettings/TagManager.asset")[0]);
		SerializedProperty it = tagManager.FindProperty ("tags");
		bool set = false;
		for( int i = 0; i < it.arraySize; ++i )
		{
			SerializedProperty t = it.GetArrayElementAtIndex( i );
			if( t.stringValue == tag2add )
			{
				set = true;
				break;
			}
			if( t.stringValue == string.Empty )
			{
				t.stringValue = tag2add;
				set = true;
				break;
			}
		}
		if( !set )
		{
			it.InsertArrayElementAtIndex (it.arraySize - 1);
			it.GetArrayElementAtIndex (it.arraySize - 1).stringValue = tag2add;
		}
		
		tagManager.ApplyModifiedProperties ();
		
	}
	
	void Create()
	{
		characters = GameObject.Find("Characters");
		if(characters == null)
			characters = new GameObject("Characters");

        if (characts[0] != null)
        {
			if(characters.transform.FindChild(Wname) == null){
                for (int i = 0; i < characts.Count; i++)
                {
                    if (characts[i] != null)
                    {
                        Asset.Add(Instantiate(characts[i]) as GameObject);
                        Asset[i].transform.parent = characters.transform;
                        Asset[i].transform.position = posNPC;
                        string nameA = Asset[i].name;
                        if (nameA.Contains("(Clone)"))
                            nameA = nameA.Replace("(Clone)", "");
                        Asset[i].name = Wname + "_" + nameA;

				        /*character = Instantiate(PrefabNPC) as GameObject; 
				        character.name = Wname;
				        character.transform.parent = characters.transform;
                        character.transform.position = posNPC; */
                        AddTag("Character");
                        Asset[i].tag = "Character";
                        if (Asset[i].GetComponent("CapsuleCollider") == null)
                            zone = Asset[i].AddComponent("CapsuleCollider") as CapsuleCollider;
				        else
                            zone = Asset[i].GetComponent("CapsuleCollider") as CapsuleCollider;
				        zone.radius = 1;
				        zone.isTrigger = false;
				        zone.height = 2.5f;
                        if (Asset[i].GetComponent("Entity") == null)
                            entity = Asset[i].AddComponent("Entity") as Entity;
				        else
                            entity = Asset[i].GetComponent("Entity") as Entity;
				        entity.prototype = Pname;
                        if (Asset[i].GetComponent("EnableLife") == null)
                            ELife = Asset[i].AddComponent("EnableLife") as EnableLife;
				        else
                            ELife = Asset[i].GetComponent("EnableLife") as EnableLife;
				        ELife.wait = delay;
                        if (Asset[i].GetComponent("LifeSceneRoleNames") == null)
                            Asset[i].AddComponent("LifeSceneRoleNames");
                        if (Asset[i].GetComponent("ZoneDetection") == null)
                            ZDec = Asset[i].AddComponent("ZoneDetection") as ZoneDetection;
				        else
                            ZDec = Asset[i].GetComponent("ZoneDetection") as ZoneDetection;
				        ZDec.useTrigger = true;
                        if (Asset[i].GetComponent("InteractionDetection") == null)
                            Asset[i].AddComponent("InteractionDetection");
                        if (Asset[i].GetComponent("NavMeshAgent") == null)
                            Asset[i].AddComponent("NavMeshAgent");
                        if (Asset[i].GetComponent("Rigidbody") == null)
                            Asset[i].AddComponent("Rigidbody");
				        /*if(NPCAnimator != "" && character.GetComponent(NPCAnimator) == null)
					        character.AddComponent(NPCAnimator);*/
                        if (Asset[i].GetComponentInChildren<SphereCollider>() == null)
                        {
					        SmartZone = new GameObject("SmartZone");
                            SmartZone.transform.parent = Asset[i].transform;
					        if(SmartZone.GetComponent("SphereCollider") == null)
						        smartZ = SmartZone.AddComponent("SphereCollider") as SphereCollider;
					        else 
						        smartZ = SmartZone.GetComponent("SphereCollider") as SphereCollider;
					        smartZ.radius = 1;
					        smartZ.isTrigger = true;
				        }

                        if (Asset[i].GetComponentInChildren<MessageDisplayer>() == null)
                        {
					        SpeechBubble = new GameObject("SpeechBubble");
                            SpeechBubble.transform.parent = Asset[i].transform;
					        if(SpeechBubble.GetComponent("MessageDisplayer") == null)
						        SpeechBubble.AddComponent("MessageDisplayer");
					        if(SpeechBubble.GetComponent("AutoFacingCam") == null)
						        SpeechBubble.AddComponent("AutoFacingCam");
				        }
                    }
                }
			}

			if(Wname != "" && Wname != " "){
                EditorGUIUtility.PingObject(Asset[0]);
                Selection.activeGameObject = Asset[0];

				this.Close();
				Selection.activeGameObject.GetComponent<Entity>();
			}
		 }
	}

	void OnGUI () {
		//GUILayout.Label ("Character", EditorStyles.boldLabel);
		//GUI.SetNextControlName("CName");
		//GUI.FocusControl("CName");
        GUILayout.Space(20); 
        Wname = EditorGUILayout.TextField("Name", Wname, GUILayout.Width(300));
		GUILayout.Space(10);
		Pname = EditorGUILayout.TextField ("MASA LIFE Prototype", Pname, GUILayout.Width(300));
		//GUILayout.Space(10);
        //PrefabNPC = EditorGUILayout.ObjectField("Character Prefab", PrefabNPC, typeof(GameObject), true, GUILayout.Width(300)) as GameObject;
		GUILayout.Space(10);
        if (characts.Count == 0)
            characts.Add(null);
        for (int i = 0; i < characts.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            string nameO = "Prefab Character " + (i + 1);
            characts[i] = EditorGUILayout.ObjectField(nameO, characts[i], typeof(GameObject), true, GUILayout.Width(260)) as GameObject;

            if (GUILayout.Button(cleanButtonContent, EditorStyles.miniButtonLeft, GUILayout.ExpandWidth(false)))
            {
                characts.RemoveAt(i);
                size = size - 25;
            }
            if (GUILayout.Button(editButtonContent, EditorStyles.miniButtonRight, GUILayout.ExpandWidth(false)))
            {
                characts.Add(null);
                size = size + 25;
            }
            minSize = new Vector2(335, size);
            maxSize = new Vector2(375, size);
            title = "Character";
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);

        }
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUIStyle myStyle = GUI.skin.GetStyle("HelpBox");
        myStyle.richText = true;
        EditorGUILayout.TextArea("Character prefab needs to have <b>all animation components assigned </b>to work with LifeScenes", myStyle);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Position"), GUILayout.Width(145));
        posNPC = EditorGUILayout.Vector3Field(new GUIContent(""), posNPC, GUILayout.Width(150));
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(-5); 
		/*NPCAnimator = EditorGUILayout.TextField ("Animator Component", NPCAnimator, GUILayout.Width(300));
		GUILayout.Space(10);*/
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(new GUIContent("Behavior Start Time"), GUILayout.Width(145));
		delay = EditorGUILayout.FloatField(delay, GUILayout.Width(50)); 
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(15);
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Create", EditorStyles.miniButtonLeft, GUILayout.ExpandWidth(false), GUILayout.Width(100)))
			if(Wname != "" && Wname != " ")
				Create();
        Event e = Event.current;
        if (e.keyCode == KeyCode.Return) 
            Create();
        GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

	}
}
