using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using System.Text.RegularExpressions; 
using System;



[CustomEditor(typeof(LifeSceneRoleNames))]
public class LifeSceneRoleNamesEditor : Editor 
{
	private MethodInfo boldFontMethodInfo = null;
	private List<int> idx = new List<int>();
	private List<int> lifeSceneSelected = new List<int>();
    private GameObject lifescenes;
    private GameObject characters;
    private List<GameObject> rootObjects = new List<GameObject>();


	private void SetBoldDefaultFont(bool value) 
	{
		if(boldFontMethodInfo == null)
			boldFontMethodInfo = typeof(EditorGUIUtility).GetMethod("SetBoldDefaultFont", BindingFlags.Static | BindingFlags.NonPublic);
		boldFontMethodInfo.Invoke(null, new[] { value as object });	
	}
	
	int CompareObNames( GameObject x, GameObject y )
	{
		int result = 0;
		Regex re = new Regex(@"(.*)(\d+)$");
		Match m1 = re.Match(x.name);
		Match m2 = re.Match(y.name);
		if (m1.Success && m2.Success)
		{
			result = m1.Groups[1].Value.CompareTo(m2.Groups[1].Value);
			if(result==0)
				result = int.Parse(m1.Groups[2].Value).CompareTo(int.Parse(m2.Groups[2].Value));
		}
		else{
			result = x.name.CompareTo(y.name);
		}
		return result;
	}

	public override void OnInspectorGUI() 
	{

		RoleManager roleNames = RoleManager.Instance;
        LifeSceneManager lifeScenesLS = LifeSceneManager.Instance;
        lifeScenesLS.UpdateLS();
        CharacterManager characterM = CharacterManager.Instance;
        characterM.UpdateC();
        EditorGUI.indentLevel += 1;
		SerializedProperty Roles = serializedObject.FindProperty("Roles");
		SerializedProperty lifeScene = serializedObject.FindProperty("LifeScenes");
		if(Roles.arraySize == 0){
			Roles.InsertArrayElementAtIndex(0);
			lifeScene.InsertArrayElementAtIndex(0);
			Roles.GetArrayElementAtIndex(0).stringValue = "";
			lifeScene.GetArrayElementAtIndex(0).stringValue = "";

		}
		if(lifeScene.arraySize != Roles.arraySize)
			lifeScene.arraySize = Roles.arraySize;

		idx.Clear();
		lifeSceneSelected.Clear();
		List<string> lifeSceneListName = new List<string>();
		for( int i = 0 ; i < Roles.arraySize; ++i )
		{
			GUILayout.BeginHorizontal();
			SerializedProperty item = Roles.GetArrayElementAtIndex(i);
			string RoleName = item.stringValue;
			
			if (roleNames.roleNames.Length == 0)
				roleNames.roleNames.SetValue("", 0); 

			idx.Add(-1);
			for( int j = 0 ; j < roleNames.roleNames.Length; ++j )
				if( roleNames.roleNames[ j ] == RoleName )
					idx[i] = j;

			/*for( int k = 0 ; k < Roles.arraySize; ++k ){
				if(i != k && RoleName == Roles.GetArrayElementAtIndex(k).stringValue && RoleName != ""){
					idx[i] = 0;
					item.stringValue = "";
				}
				
			}*/
			int newIdx = EditorGUILayout.Popup( idx[i], roleNames.roleNames );
			if( newIdx != idx[i] )
				item.stringValue = roleNames.roleNames[ newIdx ];
			
			lifeSceneSelected.Add(-1);
			SerializedProperty item1 = lifeScene.GetArrayElementAtIndex(i);
			string lifeSceneName = item1.stringValue;
			int countLS = 0;
            for (int j = 0; j < lifeScenesLS.LifeSceneNames.Length; ++j)
            {
                lifeSceneListName.Add(lifeScenesLS.LifeSceneNames[j].name);
				if( lifeSceneListName[j] == lifeSceneName )
					lifeSceneSelected[i] = j;
				if(lifeSceneListName[j] != lifeSceneName)
					countLS = countLS + 1;
			}

            if (countLS == lifeScenesLS.LifeSceneNames.Length)
            {
				lifeSceneSelected[i] = -1;
				item1.stringValue = "";
			}

			
			for( int k = 0 ; k < lifeScene.arraySize; ++k ){
				if(i != k && lifeSceneName == lifeScene.GetArrayElementAtIndex(k).stringValue && lifeSceneName != "" ){
					lifeSceneSelected[i] = 0;
					item1.stringValue = "";
				}
			}


			int newlifeSceneSelected = EditorGUILayout.Popup( lifeSceneSelected[i], lifeSceneListName.ToArray());
			if( newlifeSceneSelected != lifeSceneSelected[i] )
				item1.stringValue = lifeSceneListName[newlifeSceneSelected];
			
			GUILayout.Space(5);
			if( GUILayout.Button ("-", EditorStyles.miniButton, GUILayout.ExpandWidth(false) ) )
			{
				if(i >= 0){
					Roles.DeleteArrayElementAtIndex(i);
					lifeScene.DeleteArrayElementAtIndex(i);
				}
				GUILayout.EndHorizontal();
				break;
			}
			
			GUILayout.Space(5);
			GUILayout.EndHorizontal();
			
			
		}
	
		GUILayout.Space(5);
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button("Add Role", EditorStyles.miniButtonLeft, GUILayout.ExpandWidth(false), GUILayout.Width(100) ) )
		{
			Roles.InsertArrayElementAtIndex(Roles.arraySize-1);
			Roles.GetArrayElementAtIndex(Roles.arraySize-1 ).stringValue = string.Empty;
			lifeScene.InsertArrayElementAtIndex(Roles.arraySize-1);
			lifeScene.GetArrayElementAtIndex(Roles.arraySize-1 ).stringValue = string.Empty;
		}
		if( GUILayout.Button("Edit Roles", EditorStyles.miniButtonRight, GUILayout.ExpandWidth(false), GUILayout.Width(100) ) )
		{
			Selection.activeObject = roleNames;
		}
		
		EditorGUI.indentLevel -= 1;
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
        GUILayout.Space(5);
						
		serializedObject.ApplyModifiedProperties();


		/*RoleManager roleNames = RoleManager.Instance;

		EditorGUILayout.BeginHorizontal();
		SerializedProperty role = serializedObject.FindProperty ("allowedRole");
		int roleSelected = -1;
		for (int i = 0; i < roleNames.roleNames.Length; ++i) 
		{
			if( roleNames.roleNames[i] == role.stringValue )
				roleSelected = i;
		}
		if (role.isInstantiatedPrefab)
			SetBoldDefaultFont (role.prefabOverride);
		
		EditorGUILayout.PrefixLabel( new GUIContent( "Allowed Role","Role that may take this place") );
		int newRoleSelected = EditorGUILayout.Popup (roleSelected, roleNames.roleNames);
		if (newRoleSelected >= 0 && newRoleSelected < roleNames.roleNames.Length)
		{
			role.stringValue = roleNames.roleNames[newRoleSelected];
		}

		if (GUILayout.Button(cleanButtonContent, EditorStyles.miniButtonLeft,GUILayout.ExpandWidth(false)) )
		{
			role.stringValue = string.Empty;
		}
		if (GUILayout.Button(editButtonContent, EditorStyles.miniButtonRight,GUILayout.ExpandWidth(false)) )
		{
			Selection.activeInstanceID = roleNames.GetInstanceID();
		}
		EditorGUILayout.EndHorizontal();
		SetBoldDefaultFont (false);
		
		serializedObject.ApplyModifiedProperties();*/

        //sorting gameobject on fly
        rootObjects.Clear();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Character"))
        {
            if (obj.transform.parent == null)
            {
                rootObjects.Add(obj);
            }
        }

         if (GameObject.Find("Characters") == null)
             characters = new GameObject("Characters");
         else
             characters = GameObject.Find("Characters");

        foreach (GameObject obj in rootObjects)
        {
            if (obj.GetComponent<LifeSceneRoleNames>() != null)
            {
                obj.transform.parent = characters.transform;
                EditorGUIUtility.PingObject(obj);
                Selection.activeGameObject = obj.gameObject;
            }
        }
	}
	
}
