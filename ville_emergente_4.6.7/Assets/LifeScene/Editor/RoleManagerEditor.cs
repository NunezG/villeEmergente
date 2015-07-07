using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using System;


[CustomEditor(typeof(RoleManager))]
public class RoleManagerEditor : Editor 
{
	private MethodInfo boldFontMethodInfo = null;
    private List<String> names = new List<string>();
	
	private void SetBoldDefaultFont(bool value) 
	{
		if(boldFontMethodInfo == null)
			boldFontMethodInfo = typeof(EditorGUIUtility).GetMethod("SetBoldDefaultFont", BindingFlags.Static | BindingFlags.NonPublic);
		boldFontMethodInfo.Invoke(null, new[] { value as object });	
	}
	
	public override void OnInspectorGUI() 
	{
		
		EditorGUI.indentLevel += 1;
		SerializedProperty roleNames = serializedObject.FindProperty("roleNames");
		if(roleNames.arraySize == 0){
			roleNames.InsertArrayElementAtIndex(0);
			roleNames.GetArrayElementAtIndex(roleNames.arraySize-1 ).stringValue = "";
		}
		for( int i = 0 ; i < roleNames.arraySize; ++i )
		{
			GUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField( roleNames.GetArrayElementAtIndex(i), new GUIContent(""), true);
			if( GUILayout.Button ("-", EditorStyles.miniButtonMid, GUILayout.ExpandWidth(false) ) )
			{
				if(i >= 0){
					roleNames.DeleteArrayElementAtIndex(i);
				}
				GUILayout.EndHorizontal();
				break;
			}
			
			GUILayout.Space(5);
			GUILayout.EndHorizontal();			
			
		}
		
		if (roleNames.isInstantiatedPrefab)
			SetBoldDefaultFont (roleNames.prefabOverride);
		GUILayout.Space(5);
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button("Add Role", EditorStyles.miniButton, GUILayout.ExpandWidth(false), GUILayout.Width(100) ) )
		{
			roleNames.InsertArrayElementAtIndex(roleNames.arraySize-1);
			roleNames.GetArrayElementAtIndex(roleNames.arraySize-1 ).stringValue = "";
		}
        if (GUILayout.Button("Sort Roles", EditorStyles.miniButton, GUILayout.ExpandWidth(false), GUILayout.Width(100)))
        {
            names.Clear();
            for (int i = 0; i < roleNames.arraySize; ++i)
            {
                names.Add(roleNames.GetArrayElementAtIndex(i).stringValue);
            }
            String[] names1 = names.ToArray();
            Array.Sort(names1, EditorUtility.NaturalCompare);
            for (int i = 0; i < roleNames.arraySize; ++i)
            {
                roleNames.GetArrayElementAtIndex(i).stringValue = names1[i];
            }
        }
		
		EditorGUI.indentLevel -= 1;
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	
		
		serializedObject.ApplyModifiedProperties();
		
	}
}
