using UnityEngine;
using UnityEditor;
using mlv;
using System;
using System.Collections;
using System.Collections.Generic;

public class LifeSceneWindow : EditorWindow {
	private string Wname = "";
	private string Pname = "LifeScene";
	private int idCollider = 0;
	private float priority = 0f;
	private bool groupEnabled;
	private GameObject lifescenes;
	private GameObject lifescene;
	SerializedObject tagManager;
    private List<GameObject> PrefabLSs1 = new List<GameObject>();
    private GameObject PrefabLS;
    private List<GameObject> Asset = new List<GameObject>();
    private SphereCollider zoneS;
	private BoxCollider zoneB;
	private Entity entity;
	private LifeSceneParameters LSP;
    private Vector3 posLS = new Vector3(0, 0, 0);
    private static GUIContent cleanButtonContent = new GUIContent("-", "Remove role");
    private static GUIContent editButtonContent = new GUIContent("+", "Edit roles");
    private float size = 265;
    private GameObject assetO;
    private List<GameObject> rootObjects = new List<GameObject>();

	
	// Add menu named "My Window" to the Window menu
    [MenuItem("Window/LIFE/LifeScene/Create Life Scene")]
	[MenuItem ("MASA LIFE/LifeScene/Create Life Scene")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		LifeSceneWindow LSwindow = (LifeSceneWindow)EditorWindow.GetWindow (typeof (LifeSceneWindow), false, "Life Scene");
		LSwindow.Focus();

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
		lifescenes = GameObject.Find("LifeScenes");
		if(lifescenes == null)
			lifescenes = new GameObject("LifeScenes");

        if (lifescenes.transform.FindChild(Wname) == null)
        {
            if (PrefabLS != null)
            {
                lifescene = Instantiate(PrefabLS) as GameObject;
                lifescene.name = Wname;
            }
            else
                lifescene = new GameObject(Wname);
            lifescene.transform.parent = lifescenes.transform;
            lifescene.transform.position = posLS;
            AddTag("LifeScene");
            lifescene.tag = "LifeScene";
            if (lifescene.transform.FindChild("3DAssets") == null)
            {
                assetO = new GameObject("3DAssets");
                assetO.transform.parent = lifescene.transform;
            }
            else
                assetO = lifescene.transform.FindChild("3DAssets").gameObject;

            for (int i = 0; i < PrefabLSs1.Count; i++)
            {
                if (PrefabLSs1[i] != null)
                {
                    Asset.Add(Instantiate(PrefabLSs1[i]) as GameObject);
                    Asset[i].transform.parent = assetO.transform;
                    Asset[i].transform.position = posLS;
                    string nameA = Asset[i].name;
                    if (nameA.Contains("(Clone)"))
                        nameA = nameA.Replace("(Clone)", "");
                    Asset[i].name = Wname + "_" + nameA;
                }
            }
            if (idCollider == 0)
            {
                if (lifescene.GetComponent("BoxCollider") != null)
                    DestroyImmediate(lifescene.GetComponent("BoxCollider"));
                if (lifescene.GetComponent("SphereCollider") == null)
                    zoneS = lifescene.AddComponent("SphereCollider") as SphereCollider;
                else
                    zoneS = lifescene.GetComponent("SphereCollider") as SphereCollider;
                zoneS.radius = 5;
                zoneS.isTrigger = true;
            }
            if (idCollider == 1)
            {
                if (lifescene.GetComponent("SphereCollider") != null)
                    DestroyImmediate(lifescene.GetComponent("SphereCollider"));
                if (lifescene.GetComponent("BoxCollider") == null)
                    zoneB = lifescene.AddComponent("BoxCollider") as BoxCollider;
                else
                    zoneB = lifescene.GetComponent("BoxCollider") as BoxCollider;
                Vector3 size = new Vector3(5f, 5f, 7f);
                zoneB.size = size;
                zoneB.isTrigger = true;
            }
            if (lifescene.GetComponent("Entity") == null)
                entity = lifescene.AddComponent("Entity") as Entity;
            else
                entity = lifescene.GetComponent("Entity") as Entity;
            entity.prototype = Pname;
            if (lifescene.GetComponent("LifeSceneParameters") == null)
                LSP = lifescene.AddComponent("LifeSceneParameters") as LifeSceneParameters;
            else
                LSP = lifescene.GetComponent("LifeSceneParameters") as LifeSceneParameters;
            LSP.Priority = priority;

            rootObjects.Clear();
            foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
            {
                if (obj.transform.parent == null)
                {
                    rootObjects.Add(obj);
                }
            }

            foreach (GameObject obj in rootObjects)
            {
                if (obj.GetComponent<Simulation>() != null)
                {
                    if (obj.GetComponent("Initialisation") == null)
                        obj.AddComponent("Initialisation");
                }
            }


            if (Wname != "" && Wname != " ")
            {
                EditorGUIUtility.PingObject(lifescene);
                Selection.activeGameObject = lifescene;

                this.Close();
                Selection.activeGameObject.GetComponent<LifeSceneParameters>();
            }
        } else
            Debug.LogWarning("The name of the LifeScene already exists");
	}

	void OnGUI () {
		//GUILayout.Label ("Life Scene", EditorStyles.boldLabel);
		//GUI.SetNextControlName("LSName");
		//GUI.FocusControl("LSName");
        GUILayout.Space(20); 
        Wname = EditorGUILayout.TextField("Name", Wname, GUILayout.Width(300));
		GUILayout.Space(10);
		Pname = EditorGUILayout.TextField ("MASA LIFE Prototype", Pname, GUILayout.Width(300));
		GUILayout.Space(10);
        PrefabLS = EditorGUILayout.ObjectField("LifeScene Prefab", PrefabLS, typeof(GameObject), true, GUILayout.Width(300)) as GameObject;
        GUILayout.Space(10);
        if (PrefabLSs1.Count == 0)
            PrefabLSs1.Add(null);
        for (int i = 0; i < PrefabLSs1.Count; i++)
        {
            EditorGUILayout.BeginHorizontal(); 
            string nameO = "Add GameObject " + (i + 1);
            PrefabLSs1[i] = EditorGUILayout.ObjectField(nameO, PrefabLSs1[i], typeof(GameObject), true, GUILayout.Width(260)) as GameObject;

            if (GUILayout.Button(cleanButtonContent, EditorStyles.miniButtonLeft, GUILayout.ExpandWidth(false)))
            {
                PrefabLSs1.RemoveAt(i);
                size = size - 25;
            }
            if (GUILayout.Button(editButtonContent, EditorStyles.miniButtonRight, GUILayout.ExpandWidth(false)))
            {
                PrefabLSs1.Add(null);
                size = size + 25;
            }
            minSize = new Vector2(335, size);
            maxSize = new Vector2(375, size);
            title = "LifeScene";
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);

        }
        EditorGUILayout.BeginHorizontal(); 
        EditorGUILayout.LabelField(new GUIContent("Position"), GUILayout.Width(145)); 
        posLS = EditorGUILayout.Vector3Field(new GUIContent(""), posLS, GUILayout.Width(150));
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(-5); 
        String[] collider = { "Sphere", "Box" };
        EditorGUILayout.BeginHorizontal();
 		EditorGUILayout.LabelField(new GUIContent("Collider"), GUILayout.Width(145));
		idCollider = EditorGUILayout.Popup(idCollider, collider, GUILayout.Width(100)); 
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(10);
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(new GUIContent("Priority"), GUILayout.Width(145));
		priority = EditorGUILayout.FloatField(priority, GUILayout.Width(50)); 
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
