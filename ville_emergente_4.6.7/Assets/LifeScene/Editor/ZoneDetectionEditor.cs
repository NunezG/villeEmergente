using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;

[CustomEditor(typeof(ZoneDetection))]
public class ZoneDetectionEditor : Editor 
{
    //private List<int> lifeSceneSelected = new List<int>();
    private bool clicked = false;
    private bool clickedB = false;
    private GameObject actualWindow;
    private GameObject[] selectLS;

    private MethodInfo boldFontMethodInfo = null;
    //private List<GameObject> names = new List<GameObject>();


    private void SetBoldDefaultFont(bool value)
    {
        if (boldFontMethodInfo == null)
            boldFontMethodInfo = typeof(EditorGUIUtility).GetMethod("SetBoldDefaultFont", BindingFlags.Static | BindingFlags.NonPublic);
        boldFontMethodInfo.Invoke(null, new[] { value as object });
    }

    public override void OnInspectorGUI() 
	{
		EditorGUI.indentLevel += 1;
		SerializedProperty useExternalTrigger = serializedObject.FindProperty("useTrigger");
		SerializedProperty inIP = serializedObject.FindProperty("inIP");
		SerializedProperty inEvent = serializedObject.FindProperty("inEvent");
		SerializedProperty nameIP = serializedObject.FindProperty("nameIP");
		SerializedProperty nameEvent = serializedObject.FindProperty("nameEvent");
        SerializedProperty order = serializedObject.FindProperty("order");
        SerializedProperty repeat = serializedObject.FindProperty("repeat");

		EditorGUILayout.PropertyField( useExternalTrigger, new GUIContent("Use External Trigger"), true);
		GUI.enabled = false;
		EditorGUILayout.PropertyField( inIP, new GUIContent("In IP"), true);
		EditorGUILayout.PropertyField( inEvent, new GUIContent("In Events"), true);
		EditorGUILayout.PropertyField( nameIP, new GUIContent("IP Name"), GUILayout.Width(300));
		EditorGUILayout.PropertyField( nameEvent, new GUIContent("Event Name"), GUILayout.Width(300));
		GUI.enabled = true;

        GUILayout.Space(5);
        GUILayout.FlexibleSpace();
        GUIStyle myStyle = GUI.skin.GetStyle("HelpBox");
        myStyle.richText = true;
        myStyle.fontSize = 11;
        EditorGUILayout.TextArea("<b>Active LifeScenes</b> (All by default)", myStyle);
        EditorGUILayout.LabelField("Choice (random by default) : ");
        order.boolValue = EditorGUILayout.Toggle("Ordered", order.boolValue);
        repeat.boolValue = EditorGUILayout.Toggle("Repeat", repeat.boolValue);
        GUILayout.FlexibleSpace();

        GUILayout.Space(5);
        //EditorGUI.indentLevel += 1;
        SerializedProperty lifeScene = serializedObject.FindProperty("LifeSceneNames");
        SerializedProperty buttonText = serializedObject.FindProperty("buttonText");
        LifeSceneManager lifeScenesLS = LifeSceneManager.Instance;
        lifeScenesLS.UpdateLS();

        /*if (lifeScene.arraySize == 0)
        {
            lifeScene.InsertArrayElementAtIndex(0);
            lifeScene.GetArrayElementAtIndex(0).objectReferenceValue = null;

        }*/

        for (int i = 0; i < lifeScene.arraySize; ++i)
        {
            GUILayout.BeginHorizontal();
            //EditorGUILayout.PropertyField(lifeScene.GetArrayElementAtIndex(i), new GUIContent(""), true);
            GUI.enabled = false;
            EditorGUILayout.ObjectField(new GUIContent(""), lifeScene.GetArrayElementAtIndex(i).objectReferenceValue, typeof(GameObject), true);
            GUI.enabled = true;
            if (GUILayout.Button("-", EditorStyles.miniButtonMid, GUILayout.ExpandWidth(false)))
            {
                if (i >= 0)
                {
                    //GameObject.DestroyImmediate(lifeScene.GetArrayElementAtIndex(i).objectReferenceValue);
                    lifeScenesLS.LSnames.Remove(lifeScene.GetArrayElementAtIndex(i).objectReferenceValue as GameObject);
                    lifeScene.GetArrayElementAtIndex(i).objectReferenceValue = null;
                    lifeScene.DeleteArrayElementAtIndex(i);
                }
                GUILayout.EndHorizontal();
                break;
            }

            GUILayout.Space(5);
            GUILayout.EndHorizontal();

        }
        GUILayout.Space(10);
        //string NPC = serializedObject.targetObject.name;
        SerializedProperty newEntity = serializedObject.FindProperty("newLS");
        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Drag a LifeScene");
        EditorGUILayout.PropertyField(newEntity, new GUIContent(""));
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Add an LifeScene", EditorStyles.miniButton, GUILayout.ExpandWidth(false), GUILayout.Width(150)))
        {
             lifeScene.InsertArrayElementAtIndex(lifeScene.arraySize);
             lifeScene.GetArrayElementAtIndex(lifeScene.arraySize-1).objectReferenceValue = newEntity.objectReferenceValue;
             newEntity.objectReferenceValue = null;
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("Multi-Selection of LifeScenes");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Space(5);

        Type type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.InspectorWindow");
        PropertyInfo propertyInfo = type.GetProperty("isLocked");
        //string textbutton = "Lock Inspector and Select LifeScenes";

        clicked = GUILayout.Toggle(clicked, new GUIContent(buttonText.stringValue), EditorStyles.miniButton, GUILayout.ExpandWidth(false), GUILayout.Width(220));
        if (clicked == true && clickedB == false)
        {
            clickedB = true;
            if (EditorWindow.focusedWindow != null && EditorWindow.focusedWindow.GetType().Name == "InspectorWindow" && clickedB == true)
            {
                propertyInfo.SetValue(EditorWindow.focusedWindow, true, null);
                actualWindow = Selection.activeGameObject;
                buttonText.stringValue = "Unlock Inspector after Adding LifeScenes";
            }
        }

        if (clicked == false && clickedB == true)
        {
            clickedB = false;
            if (EditorWindow.focusedWindow != null && EditorWindow.focusedWindow.GetType().Name == "InspectorWindow" && clickedB == false)
            {
                Selection.activeGameObject = actualWindow;
                propertyInfo.SetValue(EditorWindow.focusedWindow, false, null);
                buttonText.stringValue = "Lock Inspector and Select LifeScenes";

            }
        }

        if (clickedB == true)
            GUI.enabled = true;
        else
            GUI.enabled = false;
        if (GUILayout.Button("Add selected LifeScenes", EditorStyles.miniButton, GUILayout.ExpandWidth(false), GUILayout.Width(150)))
        {
            selectLS = Selection.gameObjects;
            Array.Sort(selectLS, CompareObNames);
            for (int i = 0; i < lifeScenesLS.LifeSceneNames.Length; i++)
            {
                for (int j = 0; j < selectLS.Length; j++)
                {
                    if (selectLS[j].name == lifeScenesLS.LifeSceneNames[i].name)
                    {
                        int alreadyLS = 0;
                        for (int k = 0; k < lifeScene.arraySize; k++)
                            if (selectLS[j].name == lifeScene.GetArrayElementAtIndex(k).objectReferenceValue.name)
                                alreadyLS = 1;
                        if (alreadyLS == 0)
                        {
                            lifeScene.InsertArrayElementAtIndex(lifeScene.arraySize);
                            lifeScene.GetArrayElementAtIndex(lifeScene.arraySize - 1).objectReferenceValue = selectLS[j];
                        }
                    }
                }
            }
        }
        if (clickedB == true)
            GUI.enabled = true;
        else
            GUI.enabled = false;

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        if (lifeScene.isInstantiatedPrefab)
            SetBoldDefaultFont(lifeScene.prefabOverride);
        GUILayout.Space(10);

 		serializedObject.ApplyModifiedProperties();

	}

    int CompareObNames(GameObject x, GameObject y)
    {
        int result = 0;
        Regex re = new Regex(@"(.*)(\d+)$");
        Match m1 = re.Match(x.name);
        Match m2 = re.Match(y.name);
        if (m1.Success && m2.Success)
        {
            result = m1.Groups[1].Value.CompareTo(m2.Groups[1].Value);
            if (result == 0)
                result = int.Parse(m1.Groups[2].Value).CompareTo(int.Parse(m2.Groups[2].Value));
        }
        else
        {
            result = x.name.CompareTo(y.name);
        }
        return result;
    }

}
