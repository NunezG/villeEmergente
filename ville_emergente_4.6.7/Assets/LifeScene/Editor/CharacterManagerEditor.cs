using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using System.Text.RegularExpressions; 
using System;


[CustomEditor(typeof(CharacterManager))]
public class CharacterManagerEditor : Editor 
{
	private MethodInfo boldFontMethodInfo = null;
    //private List<GameObject> names = new List<GameObject>();

	
	private void SetBoldDefaultFont(bool value) 
	{
		if(boldFontMethodInfo == null)
			boldFontMethodInfo = typeof(EditorGUIUtility).GetMethod("SetBoldDefaultFont", BindingFlags.Static | BindingFlags.NonPublic);
		boldFontMethodInfo.Invoke(null, new[] { value as object });	
	}

    public override void OnInspectorGUI() 
	{
		EditorGUI.indentLevel += 1;
        SerializedProperty character = serializedObject.FindProperty("CharacterNames");

        (target as CharacterManager).UpdateC();

        if (character.arraySize == 0)
        {
            character.InsertArrayElementAtIndex(0);
            character.GetArrayElementAtIndex(character.arraySize - 1).objectReferenceValue = null;
        }

        for (int i = 0; i < character.arraySize; ++i)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.ObjectField(new GUIContent(""), character.GetArrayElementAtIndex(i).objectReferenceValue, typeof(GameObject), true );
            if (GUILayout.Button("-", EditorStyles.miniButtonMid, GUILayout.ExpandWidth(false)))
            {
                if (i >= 0)
                {
                    GameObject.DestroyImmediate(character.GetArrayElementAtIndex(i).objectReferenceValue);
                    character.GetArrayElementAtIndex(i).objectReferenceValue = null;
                    character.DeleteArrayElementAtIndex(i);
                }
                GUILayout.EndHorizontal();
                break;
            }

            GUILayout.Space(5);
            GUILayout.EndHorizontal();

        }

        if (character.isInstantiatedPrefab)
            SetBoldDefaultFont(character.prefabOverride);
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add Character", EditorStyles.miniButton, GUILayout.ExpandWidth(false), GUILayout.Width(100)))
        {
            //lifeScene.InsertArrayElementAtIndex(lifeScene.arraySize - 1);
            //lifeScene.GetArrayElementAtIndex(lifeScene.arraySize - 1).stringValue = "";
            EditorWindow.GetWindow(typeof(NPCWindow), false, "Character");
        }
        /*if (GUILayout.Button("Sort Characters", EditorStyles.miniButton, GUILayout.ExpandWidth(false), GUILayout.Width(100)))
        {
            names.Clear();
            for (int i = 0; i < character.arraySize; ++i)
            {
                names.Add(character.GetArrayElementAtIndex(i).objectReferenceValue as GameObject);
            }
            GameObject[] names1 = names.ToArray();
            Array.Sort(names1, (target as CharacterManager).CompareObNames);
            for (int i = 0; i < character.arraySize; ++i)
            {
                character.GetArrayElementAtIndex(i).objectReferenceValue = names1[i];
            }
        }*/

        EditorGUI.indentLevel -= 1;
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();


        serializedObject.ApplyModifiedProperties();		
	}
}
