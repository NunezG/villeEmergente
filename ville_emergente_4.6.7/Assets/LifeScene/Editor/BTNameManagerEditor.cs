using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using System;


[CustomEditor(typeof(BTNameManager))]
public class BTNameManagerEditor : Editor 
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
		SerializedProperty BTNames = serializedObject.FindProperty("BTNames");
		if(BTNames.arraySize == 0){
			BTNames.InsertArrayElementAtIndex(0);
			BTNames.GetArrayElementAtIndex(BTNames.arraySize-1 ).stringValue = "";
		}

		for( int i = 0 ; i < BTNames.arraySize; ++i )
		{

            GUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField( BTNames.GetArrayElementAtIndex(i), new GUIContent(""), true);
			if( GUILayout.Button ("-", EditorStyles.miniButtonMid, GUILayout.ExpandWidth(false) ) )
			{
				if(i >= 0){
					BTNames.DeleteArrayElementAtIndex(i);
				}
				GUILayout.EndHorizontal();
				break;
			}
			
			GUILayout.Space(5);
			GUILayout.EndHorizontal();			
			
		}
		if (BTNames.isInstantiatedPrefab)
			SetBoldDefaultFont (BTNames.prefabOverride);

		GUILayout.Space(5);
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button("Add Behavior", EditorStyles.miniButton, GUILayout.ExpandWidth(false), GUILayout.Width(100) ) )
		{
			BTNames.InsertArrayElementAtIndex(BTNames.arraySize-1);
			BTNames.GetArrayElementAtIndex(BTNames.arraySize-1 ).stringValue = "";
		}
		
       if (GUILayout.Button("Sort Behaviors", EditorStyles.miniButton, GUILayout.ExpandWidth(false), GUILayout.Width(100)))
        {
            names.Clear();
            for (int i = 0; i < BTNames.arraySize; ++i)
            {
                names.Add(BTNames.GetArrayElementAtIndex(i).stringValue);
            }
            String[] names1 = names.ToArray();
            Array.Sort(names1, EditorUtility.NaturalCompare);
            for (int i = 0; i < BTNames.arraySize; ++i)
            {
                BTNames.GetArrayElementAtIndex(i).stringValue = names1[i];
            }
        }

        EditorGUI.indentLevel -= 1;
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
		
		SetBoldDefaultFont (false);
		
		serializedObject.ApplyModifiedProperties();
		
	}
}
