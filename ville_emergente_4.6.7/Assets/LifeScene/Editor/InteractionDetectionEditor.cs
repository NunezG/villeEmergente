using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(InteractionDetection))]
public class InteractionDetectionEditor : Editor 
{
	public override void OnInspectorGUI() 
	{
		EditorGUI.indentLevel += 1;
		SerializedProperty nearEntity = serializedObject.FindProperty("nearEntity");
		SerializedProperty nearPlayer = serializedObject.FindProperty("nearPlayer");
		SerializedProperty distanceEntity = serializedObject.FindProperty("distanceEntity");
		SerializedProperty distancePlayer = serializedObject.FindProperty("distancePlayer");
		SerializedProperty probabilityInteraction = serializedObject.FindProperty("probabilityInteraction");
		SerializedProperty interactionLimite = serializedObject.FindProperty("interactionLimite");
		SerializedProperty viewAngleEntity = serializedObject.FindProperty("ViewAngleEntity");
		SerializedProperty viewAnglePlayer = serializedObject.FindProperty("ViewAnglePlayer");

		GUI.enabled = false;
		EditorGUILayout.PropertyField( nearEntity, new GUIContent("Near Entity"), true);
		EditorGUILayout.PropertyField( nearPlayer, new GUIContent("Near Player"), true);
		GUI.enabled = true;
		EditorGUILayout.PropertyField( distanceEntity, new GUIContent("Distance Entity"), GUILayout.Width(200));
		EditorGUILayout.PropertyField( distancePlayer, new GUIContent("Distance Player"), GUILayout.Width(200));
		EditorGUILayout.PropertyField( probabilityInteraction, new GUIContent("Interaction Probability"), GUILayout.Width(200));
		EditorGUILayout.PropertyField( interactionLimite, new GUIContent("Interaction Limite"), GUILayout.Width(200));
		EditorGUILayout.PropertyField( viewAngleEntity, new GUIContent("View Angle Entity"), GUILayout.Width(200));
		EditorGUILayout.PropertyField( viewAnglePlayer, new GUIContent("View Angle Player"), GUILayout.Width(200));


		EditorGUI.indentLevel -= 1;

		serializedObject.ApplyModifiedProperties();

	}
}
