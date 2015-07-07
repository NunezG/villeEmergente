using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;


[CustomEditor(typeof(TypeTriggerManager))]
public class TypeTriggerManagerEditor : Editor 
{
	private MethodInfo boldFontMethodInfo = null;
	
	private void SetBoldDefaultFont(bool value) 
	{
		if(boldFontMethodInfo == null)
			boldFontMethodInfo = typeof(EditorGUIUtility).GetMethod("SetBoldDefaultFont", BindingFlags.Static | BindingFlags.NonPublic);
		boldFontMethodInfo.Invoke(null, new[] { value as object });	
	}
	
	public override void OnInspectorGUI() 
	{
			
		EditorGUI.indentLevel += 1;
		SerializedProperty triggerType = serializedObject.FindProperty("typeTriggers");
		if(triggerType.arraySize == 0){
			triggerType.InsertArrayElementAtIndex(0);
			triggerType.GetArrayElementAtIndex(triggerType.arraySize-1 ).stringValue = "";
		}
		for( int i = 0 ; i < triggerType.arraySize; ++i )
		{
			GUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField( triggerType.GetArrayElementAtIndex(i), new GUIContent(""), true);
			if( GUILayout.Button ("-", EditorStyles.miniButtonMid, GUILayout.ExpandWidth(false) ) )
			{
				if(i >= 0){
					triggerType.DeleteArrayElementAtIndex(i);
				}
				GUILayout.EndHorizontal();
				break;
			}
			
			GUILayout.Space(5);
			GUILayout.EndHorizontal();			
			
		}
		if (triggerType.isInstantiatedPrefab)
			SetBoldDefaultFont (triggerType.prefabOverride);

		GUILayout.Space(5);
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button("Add", EditorStyles.miniButton, GUILayout.ExpandWidth(false), GUILayout.Width(100) ) )
		{
			triggerType.InsertArrayElementAtIndex(triggerType.arraySize-1);
			triggerType.GetArrayElementAtIndex(triggerType.arraySize-1 ).stringValue = "";
		}
		
		EditorGUI.indentLevel -= 1;
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
			
			

		SetBoldDefaultFont (false);
		
		serializedObject.ApplyModifiedProperties();

	}
}
