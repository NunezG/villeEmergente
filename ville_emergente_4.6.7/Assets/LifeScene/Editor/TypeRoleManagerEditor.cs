using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;


[CustomEditor(typeof(TypeRoleManager))]
public class TypeRoleManagerEditor : Editor 
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
		SerializedProperty typeRoles = serializedObject.FindProperty("typeRoles");
		if(typeRoles.arraySize == 0){
			typeRoles.InsertArrayElementAtIndex(0);
			typeRoles.GetArrayElementAtIndex(typeRoles.arraySize-1 ).stringValue = "";
		}
		for( int i = 0 ; i < typeRoles.arraySize; ++i )
		{
			GUILayout.BeginHorizontal();
			GUI.enabled = false;
			EditorGUILayout.PropertyField( typeRoles.GetArrayElementAtIndex(i), new GUIContent(""), true);
			GUI.enabled = true;
			/*if( GUILayout.Button ("-", EditorStyles.miniButtonMid, GUILayout.ExpandWidth(false) ) )
			{
				if(i >= 0){
					typeRoles.DeleteArrayElementAtIndex(i);
				}
				GUILayout.EndHorizontal();
				break;
			}*/
			
			GUILayout.Space(5);
			GUILayout.EndHorizontal();			
			
		}
		if (typeRoles.isInstantiatedPrefab)
			SetBoldDefaultFont (typeRoles.prefabOverride);

		/*GUILayout.Space(5);
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button("Add", EditorStyles.miniButton, GUILayout.ExpandWidth(false), GUILayout.Width(100) ) )
		{
			typeRoles.InsertArrayElementAtIndex(typeRoles.arraySize-1);
			typeRoles.GetArrayElementAtIndex(typeRoles.arraySize-1 ).stringValue = "";
		}
		
		EditorGUI.indentLevel -= 1;
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();*/
			
			

		SetBoldDefaultFont (false);
		
		serializedObject.ApplyModifiedProperties();

	}
}
