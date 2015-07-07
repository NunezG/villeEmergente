using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using System.Text.RegularExpressions; 
using System;


[CustomEditor(typeof(LifeSceneManager))]
public class LifeSceneManagerEditor : Editor 
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
        SerializedProperty lifeScene = serializedObject.FindProperty("LifeSceneNames");

        (target as LifeSceneManager).UpdateLS();

        if (lifeScene.arraySize == 0)
        {
            lifeScene.InsertArrayElementAtIndex(0);
            lifeScene.GetArrayElementAtIndex(lifeScene.arraySize - 1).objectReferenceValue = null;
        }

        for (int i = 0; i < lifeScene.arraySize; ++i)
        {
            GUILayout.BeginHorizontal();
            //EditorGUILayout.PropertyField(lifeScene.GetArrayElementAtIndex(i), new GUIContent(""), true);
            EditorGUILayout.ObjectField(new GUIContent(""), lifeScene.GetArrayElementAtIndex(i).objectReferenceValue, typeof(GameObject), true);
            if (GUILayout.Button("-", EditorStyles.miniButtonMid, GUILayout.ExpandWidth(false)))
            {
                if (i >= 0)
                {
                    GameObject.DestroyImmediate(lifeScene.GetArrayElementAtIndex(i).objectReferenceValue);
                    //GameObject deleteLS = GameObject.Find(lifeScene.GetArrayElementAtIndex(i).stringValue);
                    //GameObject.DestroyImmediate(deleteLS);
                    lifeScene.GetArrayElementAtIndex(i).objectReferenceValue = null;
                    lifeScene.DeleteArrayElementAtIndex(i);
                }
                GUILayout.EndHorizontal();
                break;
            }

            GUILayout.Space(5);
            GUILayout.EndHorizontal();

        }

        if (lifeScene.isInstantiatedPrefab)
            SetBoldDefaultFont(lifeScene.prefabOverride);
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add LifeScene", EditorStyles.miniButton, GUILayout.ExpandWidth(false), GUILayout.Width(100)))
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


        serializedObject.ApplyModifiedProperties();		
	}
}
