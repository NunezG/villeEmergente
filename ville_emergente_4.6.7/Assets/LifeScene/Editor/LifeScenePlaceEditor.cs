using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;


[CustomEditor(typeof(LifeScenePlace))]
public class LifeScenePlaceEditor : Editor 
{
	private static GUIContent cleanButtonContent = new GUIContent ("-", "Remove role");
	private static GUIContent editButtonContent = new GUIContent ("+", "Edit roles");
	private MethodInfo boldFontMethodInfo = null;
	
	private void SetBoldDefaultFont(bool value) 
	{
		if(boldFontMethodInfo == null)
			boldFontMethodInfo = typeof(EditorGUIUtility).GetMethod("SetBoldDefaultFont", BindingFlags.Static | BindingFlags.NonPublic);
		boldFontMethodInfo.Invoke(null, new[] { value as object });	
	}

	public override void OnInspectorGUI() 
	{
		//LifeSceneRoleNames roleNames = (target as LifeScenePlace ).lifeSceneRoleNames;
		RoleManager roleNames = RoleManager.Instance;

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
			role.stringValue = roleNames.roleNames [newRoleSelected];
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
		serializedObject.ApplyModifiedProperties();
	}
	

}
