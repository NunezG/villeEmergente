using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using System.Text.RegularExpressions; 
using System;


[CustomEditor(typeof(CharacterShow))]
public class CharacterShowEditor : Editor 
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
        GUILayout.Space(10);
        EditorGUI.indentLevel += 1;
        SerializedProperty character = serializedObject.FindProperty("CharacterNames");
        SerializedProperty numberC = serializedObject.FindProperty("numberC");
        CharacterManager characterM = CharacterManager.Instance;
        characterM.UpdateC();

        if (numberC.intValue != characterM.CharacterNames.Length)
        {
            character.ClearArray();
            for (int i = 0; i < characterM.CharacterNames.Length; ++i)
            {
                character.InsertArrayElementAtIndex(i);
                character.GetArrayElementAtIndex(i - 1).objectReferenceValue = characterM.CharacterNames[i];
            }
            numberC.intValue = characterM.CharacterNames.Length;
        }

        if (character.arraySize == 0)
        {
            character.InsertArrayElementAtIndex(0);
            character.GetArrayElementAtIndex(character.arraySize - 1).objectReferenceValue = null;
        }

        for (int i = 0; i < character.arraySize; ++i)
        {
            GUILayout.BeginHorizontal();
            //EditorGUILayout.PropertyField(lifeScene.GetArrayElementAtIndex(i), new GUIContent(""), true);
            GUI.enabled = false;
            EditorGUILayout.ObjectField(new GUIContent(""), character.GetArrayElementAtIndex(i).objectReferenceValue, typeof(GameObject), true);
            GUI.enabled = true;
            if (GUILayout.Button("-", EditorStyles.miniButtonMid, GUILayout.ExpandWidth(false)))
            {
                if (i >= 0)
                {
                    GameObject.DestroyImmediate(character.GetArrayElementAtIndex(i).objectReferenceValue);
                    //GameObject deleteLS = GameObject.Find(lifeScene.GetArrayElementAtIndex(i).stringValue);
                    //GameObject.DestroyImmediate(deleteLS);
                    characterM.Cnames.Remove(character.GetArrayElementAtIndex(i).objectReferenceValue as GameObject);
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
            EditorWindow.GetWindow(typeof (LifeSceneWindow), false, "Life Scene");
        }
        /*if (GUILayout.Button("Sort LifeScenes", EditorStyles.miniButton, GUILayout.ExpandWidth(false), GUILayout.Width(100)))
        {
            names.Clear();
            for (int i = 0; i < lifeScene.arraySize; ++i)
            {
                names.Add(lifeScene.GetArrayElementAtIndex(i).objectReferenceValue as GameObject);
            }
            GameObject[] names1 = names.ToArray();
            Array.Sort(names1, (target as LifeSceneManager).CompareObNames);
            for (int i = 0; i < lifeScene.arraySize; ++i)
            {
                lifeScene.GetArrayElementAtIndex(i).objectReferenceValue = names1[i];
            }
        }*/

        EditorGUI.indentLevel -= 1;
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        serializedObject.ApplyModifiedProperties();		
	}
}
