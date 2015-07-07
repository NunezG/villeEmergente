using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(EnableLife))]
public class EnableLifeEditor : Editor 
{
	public override void OnInspectorGUI() 
	{
		EditorGUI.indentLevel += 1;
		SerializedProperty wait = serializedObject.FindProperty("wait");

		//GUI.enabled = false;
		EditorGUILayout.PropertyField( wait, new GUIContent("Behavior Delay"), GUILayout.Width(200));
		//GUI.enabled = true;

		EditorGUI.indentLevel -= 1;
		serializedObject.ApplyModifiedProperties();

	}
}
