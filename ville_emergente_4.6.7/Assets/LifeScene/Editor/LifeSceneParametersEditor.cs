using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

//[CanEditMultipleObjects]
[CustomEditor(typeof(LifeSceneParameters))]
public class LifeSceneParametersEditor : Editor 
{

	private GameObject smartZone;
	private bool countPushRole = true;
	private bool countPushBehaviors = true;
	private bool countPushTriggers = false;
	//private bool countPushStops = false;
	private bool countPushPlaces = false;
	//private bool countPushFormations = false;
	private bool countPushPlacements = false;
	private bool countPushEntities = false;
	private bool countPushMotivations = false;
	//private bool countPushTimeline = true;
    private GameObject lifescenes;
    private GameObject characters;
    private int slide = 0;
    private List<GameObject> rootObjects = new List<GameObject>();

	public void start(){
	}

	public override void OnInspectorGUI()
	{
		/*SerializedProperty parentName = serializedObject.FindProperty ("parentName");
		parentName.stringValue = serializedObject.targetObject.*/

        SerializedProperty name = serializedObject.FindProperty("LSname");
		SerializedProperty priority = serializedObject.FindProperty("Priority");
		SerializedProperty roleparameters = serializedObject.FindProperty ("roleParameters");
		SerializedProperty behaviorparameters = serializedObject.FindProperty ("behaviorParameters");
		SerializedProperty triggerParams = serializedObject.FindProperty ("triggerParameters");
		//SerializedProperty triggerParams = serializedObject.FindProperty ("triggerParameters");
		//SerializedProperty stopparameters = serializedObject.FindProperty ("stopParameters");
		SerializedProperty placeparameters = serializedObject.FindProperty ("placeParameters");
		//SerializedProperty formations = serializedObject.FindProperty ("Formations");
		SerializedProperty Placements = serializedObject.FindProperty ("Placements");
		SerializedProperty motivationparameters = serializedObject.FindProperty ("motivationParameters");
		SerializedProperty entityRolesParameters = serializedObject.FindProperty ("entityRolesParameters");

		GUILayout.Space(10);
		GUILayout.BeginHorizontal();
		string scene = serializedObject.targetObject.name;
		EditorGUILayout.PrefixLabel(new GUIContent( "LifeScene name" ));
		EditorGUILayout.LabelField(new GUIContent( scene, "LifeScene name" ));
		name.stringValue = scene;
		GUILayout.EndHorizontal();	
		GUILayout.Space(5);
		//EditorGUILayout.PropertyField( priority, new GUIContent("Priority [0-1]"), GUILayout.Width(200));
        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(new GUIContent("Priority"));
        slide = EditorGUILayout.IntSlider(slide, 0, 10, GUILayout.Width(200));
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        //SerializedProperty name = serializedObject.FindProperty ("name");
		//EditorGUILayout.PropertyField( name, new GUIContent( "LifeScene name","LifeScene name" ) );

		//SerializedProperty bt = serializedObject.FindProperty ("Behavior");
		//EditorGUILayout.PropertyField( bt, new GUIContent( "LifeScene behavior","LifeScene behavior" ) );

		/*SerializedProperty formation = serializedObject.FindProperty ("Formation");
		EditorGUILayout.PropertyField( formation, new GUIContent( "Does the lifescene need group formation ?","Does the lifescene need group formation ?" ) );
		*/

		GUIStyle myFoldoutStyleLeft = new GUIStyle(EditorStyles.miniButtonLeft);
		GUIStyle myFoldoutStyleMid = new GUIStyle(EditorStyles.miniButtonMid);
		GUIStyle myFoldoutStyleRight = new GUIStyle(EditorStyles.miniButtonRight);

		GUILayout.Space(10);
		GUILayout.BeginHorizontal();
		countPushRole = GUILayout.Toggle(countPushRole, "Roles", myFoldoutStyleLeft, GUILayout.ExpandWidth(true)) ;
		countPushBehaviors = GUILayout.Toggle(countPushBehaviors, "Behaviors", myFoldoutStyleMid, GUILayout.ExpandWidth(true)) ;
		countPushTriggers = GUILayout.Toggle(countPushTriggers, "Trigger Conditions", myFoldoutStyleMid, GUILayout.ExpandWidth(true)) ;
		//countPushStops = GUILayout.Toggle(countPushStops, "Stop conditions", myFoldoutStyleRight, GUILayout.ExpandWidth(true)) ;
		GUILayout.EndHorizontal();	
		GUILayout.BeginHorizontal();
		countPushPlaces = GUILayout.Toggle(countPushPlaces, "Places", myFoldoutStyleLeft, GUILayout.ExpandWidth(true)) ;
		countPushPlacements = GUILayout.Toggle(countPushPlacements, "Formations", myFoldoutStyleMid, GUILayout.ExpandWidth(true)) ;
		countPushEntities = GUILayout.Toggle(countPushEntities, "Entities", myFoldoutStyleMid, GUILayout.ExpandWidth(true)) ;
		countPushMotivations = GUILayout.Toggle(countPushMotivations, "Motivations", myFoldoutStyleRight, GUILayout.ExpandWidth(true)) ;
		GUILayout.EndHorizontal();	

		GUILayout.Space(10);
		//countPushTimeline = GUILayout.Toggle(countPushTimeline, "Timeline editor", myFoldoutStyleRight, GUILayout.ExpandWidth(true)) ;

		if (GUILayout.Button("Timeline editor"))
		{
			LifeSceneTimeLineWindow self = (LifeSceneTimeLineWindow)EditorWindow.GetWindow(typeof(LifeSceneTimeLineWindow));
			self.OnSelectionChange();
			self.Show();
		}

		/*LifeSceneTimeLineWindow self = (LifeSceneTimeLineWindow)EditorWindow.GetWindow(typeof(LifeSceneTimeLineWindow));
		if(countPushTimeline){
			self.OnSelectionChange();
			self.Show();
		} else {
			EditorWindow.Destroy(self);
		}*/
		


		if( countPushRole == true){
			EditorGUILayout.PropertyField( roleparameters, new GUIContent( "Roles / Role Types" ) );
		}

		if( countPushBehaviors == true){
			EditorGUILayout.PropertyField( behaviorparameters, new GUIContent( "Behaviors / Roles" ), true );
		}
		if( countPushTriggers == true){
			EditorGUILayout.PropertyField( triggerParams, new GUIContent( "Trigger Conditions","Trigger Conditions" ), true );
		}
		/*if( countPushStops == true){
			EditorGUILayout.PropertyField( stopparameters, new GUIContent( "Stop Conditions" ), true );
		}*/
		if( countPushPlaces == true){
			EditorGUILayout.PropertyField( placeparameters, new GUIContent( "Places" ), true );
		}
		/*if( countPushFormations == true){
			EditorGUILayout.PropertyField( formations, new GUIContent( "Formation parameters","Formation parameters" ), true );
		}*/
		if( countPushPlacements == true){
			EditorGUILayout.PropertyField( Placements, new GUIContent( "Placements","Placements" ), true );
		}
		if( countPushEntities == true){
			EditorGUILayout.PropertyField( entityRolesParameters, new GUIContent( "Entities" ), true );
		}
		if( countPushMotivations == true){
			EditorGUILayout.PropertyField( motivationparameters, new GUIContent( "Motivations" ), true );
		}
		GUILayout.Space(10);

		/*if (GUILayout.Button("Apply"))
			ApplyInKB();*/

		serializedObject.ApplyModifiedProperties();

        //sorting gameobject on the fly
        LifeSceneManager lifeScenesLS = LifeSceneManager.Instance;
        lifeScenesLS.UpdateLS();
        CharacterManager characterM = CharacterManager.Instance;
        characterM.UpdateC();
        rootObjects.Clear();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("LifeScene"))
        {
            if (obj.transform.parent == null)
            {
                rootObjects.Add(obj);
            }
        }

         if (GameObject.Find("LifeScenes") == null)
             lifescenes = new GameObject("LifeScenes");
         else
             lifescenes = GameObject.Find("LifeScenes");

        foreach (GameObject obj in rootObjects)
        {
            if (obj.GetComponent<LifeSceneParameters>() != null)
            {
                obj.transform.parent = lifescenes.transform;
                Selection.activeGameObject = obj;
            }

        }

     }

	public void OnSceneGUI()
	{

        //Coloring SmartZones
        Handles.color = new Color(1,0,0,0.1f);
		LifeSceneParameters self = (target as LifeSceneParameters);
		//smartZone = GameObject.Find("SmartZone");
		//float radius = self.GetComponentInChildren<SphereCollider>().radius;
		//float radius = GameObject.Find("SmartZone").GetComponent<SphereCollider>().radius;
        float radius = 5;
        float rangex = 5;
        float rangez = 5;

        if (self.GetComponent<SphereCollider>() != null)
        {
            radius = (self.collider as SphereCollider).radius;
            
			float radiusScale =  Mathf.Max(Mathf.Abs(self.transform.lossyScale.x), Mathf.Max(Mathf.Abs(self.transform.lossyScale.y), Mathf.Abs(self.transform.lossyScale.z)));
			radius *= radiusScale;
			
			Handles.DrawSolidDisc(self.transform.position, Vector3.up, radius);
            Handles.color = Color.red;

            Vector3 vPos;

			vPos = Handles.FreeMoveHandle(self.transform.position + new Vector3(radius, 0, 0),
                                             Quaternion.identity,
                                             HandleUtility.GetHandleSize(self.transform.position + new Vector3(radius, 0, 0)) * .05f,
                                             Vector3.one,
                                             Handles.DotCap);
            radius = (vPos - self.transform.position).x;

            vPos = Handles.FreeMoveHandle(self.transform.position + new Vector3(-radius, 0, 0),
                                          Quaternion.identity,
                                          HandleUtility.GetHandleSize(self.transform.position + new Vector3(-radius, 0, 0)) * .05f,
                                          Vector3.one,
                                          Handles.DotCap);
            radius = Mathf.Abs((vPos - self.transform.position).x);

            vPos = Handles.FreeMoveHandle(self.transform.position + new Vector3(0, 0, radius),
                                          Quaternion.identity,
                                          HandleUtility.GetHandleSize(self.transform.position + new Vector3(0, 0, radius)) * .05f,
                                          Vector3.one,
                                          Handles.DotCap);
            radius = (vPos - self.transform.position).z;

            vPos = Handles.FreeMoveHandle(self.transform.position + new Vector3(0, 0, -radius),
                                          Quaternion.identity,
                                          HandleUtility.GetHandleSize(self.transform.position + new Vector3(0, 0, -radius)) * .05f,
                                          Vector3.one,
                                          Handles.DotCap);
            radius = Mathf.Abs((vPos - self.transform.position).z);

            radius = Mathf.Max(0, radius);
            //smartZone.GetComponent<SphereCollider>().radius = radius;
            //self.GetComponentInChildren<SphereCollider>().radius = radius;
            (self.collider as SphereCollider).radius = radius/radiusScale;
            //GameObject.Find("SmartZone").GetComponent<SphereCollider>().radius = radius;
        }

        if (self.GetComponent<BoxCollider>() != null)
        {
			BoxCollider collider = self.GetComponent<BoxCollider>();
            Vector3 center = collider.center;
            rangex = collider.size.x;
            rangez = collider.size.z;

            Vector3[] verts = new Vector3[] {
                new Vector3(-rangex/2, 0, -rangez/2), 
                new Vector3(-rangex/2, 0, rangez/2), 
                new Vector3(rangex/2, 0, rangez/2), 
                new Vector3(rangex/2, 0, -rangez/2) };
            Vector3[] vertsTrans = new Vector3[verts.Length];
            
            for (int i = 0; i < verts.Length; ++i)
                vertsTrans[i] = self.transform.TransformPoint(center + verts[i]);
            
            Handles.DrawSolidRectangleWithOutline(vertsTrans, Color.red, Color.green);
            Handles.color = Color.red;

            Vector3 vPos;
            vPos = Handles.FreeMoveHandle(self.transform.TransformPoint(new Vector3(rangex / 2, 0, 0)),
                                            Quaternion.identity,
                                            HandleUtility.GetHandleSize(self.transform.TransformPoint(new Vector3(rangex / 2, 0, 0))) * .05f,
                                            Vector3.one,
                                            Handles.DotCap);
			rangex = self.transform.InverseTransformPoint(vPos).x *2f;

            vPos = Handles.FreeMoveHandle(self.transform.TransformPoint(new Vector3(-rangex / 2, 0, 0)),
                                          Quaternion.identity,
                                          HandleUtility.GetHandleSize(self.transform.TransformPoint(new Vector3(-rangex / 2, 0, 0))) * .05f,
                                          Vector3.one,
                                          Handles.DotCap);
            rangex = Mathf.Abs(self.transform.InverseTransformPoint(vPos).x *2f);

            vPos = Handles.FreeMoveHandle(self.transform.TransformPoint(new Vector3(0, 0, rangez / 2)),
                                          Quaternion.identity,
                                          HandleUtility.GetHandleSize(self.transform.TransformPoint(new Vector3(0, 0, rangez / 2))) * .05f,
                                          Vector3.one,
                                          Handles.DotCap);
			rangez = self.transform.InverseTransformPoint(vPos).z * 2f;

            vPos = Handles.FreeMoveHandle(self.transform.TransformPoint(new Vector3(0, 0, -rangez / 2)),
                                          Quaternion.identity,
                                          HandleUtility.GetHandleSize(self.transform.TransformPoint(new Vector3(0, 0, -rangez / 2))) * .05f,
                                          Vector3.one,
                                          Handles.DotCap);
			rangez = Mathf.Abs(self.transform.InverseTransformPoint(vPos).z * 2f);


            rangex = Mathf.Max(0, rangex);
            rangez = Mathf.Max(0, rangez);
           
			(self.collider as BoxCollider).size = new Vector3(rangex, collider.size.y, rangez);
        }
    }

	private void ApplyInKB() {
		Debug.Log("Apply");
		(target as LifeSceneParameters ).Apply();
	}

}
