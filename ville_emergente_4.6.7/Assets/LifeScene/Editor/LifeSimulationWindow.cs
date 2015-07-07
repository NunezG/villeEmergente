using UnityEngine;
using UnityEditor;
using mlv;
using System;
using System.Collections;
using System.Collections.Generic;

public class LifeSimulationWindow : EditorWindow {
    private string Wname = "LIFE_Simulation";
    private string Pname = "";
    private int Pport = 8000;
    private bool Adebug = true;
    private Simulation sim;
    private GameObject simulation = null;
    private int idLog = 1;
    private DeployedData dsim = null;
    private List<GameObject> rootObjects = new List<GameObject>();
	
	// Add menu named "My Window" to the Window menu
    [MenuItem("Window/LIFE/LifeScene/Add Life Simulation Component")]
    [MenuItem("MASA LIFE/LifeScene/Add Life Simulation Component")]
	static void Init () {
		// Get existing open window or if none, make a new one:
        LifeSimulationWindow LSwindow = (LifeSimulationWindow)EditorWindow.GetWindow(typeof(LifeSimulationWindow), false, "Life Scene");
		LSwindow.Focus();

	}
	
	void Create()
	{
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
                simulation = obj;
            }
        }

        if (simulation == null)
            simulation = new GameObject(Wname);

        if (simulation.GetComponent("Simulation") == null)
            sim = simulation.AddComponent("Simulation") as Simulation;
		else
            sim = simulation.GetComponent("Simulation") as Simulation;
        sim.allowDebugging = Adebug;
        dsim.simFile = Pname;
        sim.simulationFile = dsim;
        sim.debugPort = Pport;
        sim.logLevel = idLog;
        if (simulation.GetComponent("Initialisation") == null)
            simulation.AddComponent("Initialisation");

		if(Wname != "" && Wname != " "){
            EditorGUIUtility.PingObject(simulation);
            Selection.activeGameObject = simulation;

			this.Close();
		}
	}

	void OnGUI () {
        GUILayout.Space(20);
        Wname = EditorGUILayout.TextField("LIFE Simulation Name", Wname, GUILayout.Width(300));
        GUILayout.Space(10);
        dsim = EditorGUILayout.ObjectField(new GUIContent("MASA LIFE Simulation (mlsim)"), dsim, typeof(DeployedData), false) as DeployedData;
        GUILayout.Space(10);
        Adebug = EditorGUILayout.Toggle("Debugging Port", Adebug, GUILayout.Width(300));
        GUILayout.Space(10);
        Pport = EditorGUILayout.IntField("Debugging Port", Pport, GUILayout.Width(300));
        GUILayout.Space(10);
        String[] logLevel = { "Warning", "Info", "Trace" };
        EditorGUILayout.BeginHorizontal();
 		EditorGUILayout.LabelField(new GUIContent("Log Level"), GUILayout.Width(145));
        idLog = EditorGUILayout.Popup(idLog, logLevel, GUILayout.Width(100)); 
		EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal(); 
        GUILayout.FlexibleSpace(); 
        if (GUILayout.Button("Create", EditorStyles.miniButtonLeft, GUILayout.ExpandWidth(false), GUILayout.Width(100)))
			if(Wname != "" && Wname != " ")
				Create();
        Event e = Event.current;
        if (e.keyCode == KeyCode.Return) 
            Create();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
	}
}
