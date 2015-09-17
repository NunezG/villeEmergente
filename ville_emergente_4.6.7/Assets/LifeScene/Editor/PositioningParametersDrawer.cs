using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(PositioningParameters))]
public class PositioningParametersDrawer
: PropertyDrawer
{
    private static GUIContent cleanButtonContent = new GUIContent("-", "Remove role");
    private static GUIContent editButtonContent = new GUIContent("+", "Edit roles");
    private SerializedProperty item;
    //private PositioningParameters posParam = new PositioningParameters();
    private GameObject PrefabPlace;
    //private GameObject PlaceNPC;
    private GameObject Places;
    private List<GameObject> PlacesNPC = new List<GameObject>();
    private int ancAngleOrient = 0;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //BTNameManager BTnamesManag = BTNameManager.Instance;
        RoleManager roleNames = RoleManager.Instance;
        label = EditorGUI.BeginProperty(position, label, property);
        string indexPlacement = label.text;
        indexPlacement = indexPlacement.Replace("Placement ", "");
        property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, label);
        if (property.isExpanded)
        {
            EditorGUI.indentLevel += 1;
            SerializedProperty typeForm = property.FindPropertyRelative("TypeForm");
            //SerializedProperty fullBehavior = property.FindPropertyRelative("fullBehavior");
            //EditorGUILayout.PropertyField( behavior, new GUIContent("Behavior executed by the participants"));
            SerializedProperty role = property.FindPropertyRelative("ConcernedRole");
            SerializedProperty generate = property.FindPropertyRelative("GenerationOfPlace");
            SerializedProperty unity = property.FindPropertyRelative("UnityPlace");

            EditorGUILayout.PropertyField(typeForm, new GUIContent("Type de formation"));

            int roleSelected = -1;
            for (int i = 0; i < roleNames.roleNames.Length; ++i)
            {
                if (roleNames.roleNames[i] == role.stringValue)
                    roleSelected = i;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent("Concerned Role"));
            GUILayout.Space(-35);
            int newRoleSelected = EditorGUILayout.Popup(roleSelected, roleNames.roleNames);
            if (newRoleSelected >= 0 && newRoleSelected < roleNames.roleNames.Length)
            {
                role.stringValue = roleNames.roleNames[newRoleSelected];
            }
            if (GUILayout.Button(cleanButtonContent, EditorStyles.miniButtonLeft, GUILayout.ExpandWidth(false)))
            {
                role.stringValue = string.Empty;
            }
            if (GUILayout.Button(editButtonContent, EditorStyles.miniButtonRight, GUILayout.ExpandWidth(false)))
            {
                Selection.activeInstanceID = roleNames.GetInstanceID();
            }
            EditorGUILayout.EndHorizontal();

            /*int idx = -1; 
            for (int i = 0; i < BTnamesManag.BTNames.Length; ++i) 
            {
                if( BTnamesManag.BTNames[i] == fullBehavior.stringValue )
                    idx = i;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent("Behavior if full"));
            GUILayout.Space(-35);
            int newIdx = EditorGUILayout.Popup(idx, BTnamesManag.BTNames);
            if (newIdx >= 0 && newIdx < BTnamesManag.BTNames.Length)
            {
                fullBehavior.stringValue = BTnamesManag.BTNames[newIdx];
            }
            if (GUILayout.Button(cleanButtonContent, EditorStyles.miniButtonLeft, GUILayout.ExpandWidth(false)))
            {
                fullBehavior.stringValue = "none";
            }
            if (GUILayout.Button(editButtonContent, EditorStyles.miniButtonRight, GUILayout.ExpandWidth(false)))
            {
                Selection.activeInstanceID = BTnamesManag.GetInstanceID();
            }
            EditorGUILayout.EndHorizontal(); 
            GUILayout.Space(5);*/

            EditorGUILayout.PropertyField(generate, new GUIContent("Automated generation of the formation places", "Automated generation of the formation places"));
            unity.boolValue = !generate.boolValue;

            EditorGUILayout.PropertyField(unity, new GUIContent("Does Formation uses places from Unity", "Does Formation uses places from Unity"));
            generate.boolValue = !unity.boolValue;

            if (unity.boolValue)
            {
                EditorGUI.indentLevel += 1;
                SerializedProperty place = property.FindPropertyRelative("UnityPlaces");
                /*if(place.arraySize == 0){
                    place.InsertArrayElementAtIndex(0);
                }*/
                for (int i = 0; i < place.arraySize; ++i)
                {
                    string name = "Place " + (i + 1);
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel(new GUIContent(name));
                    GUILayout.Space(-55);

                    EditorGUILayout.PropertyField(place.GetArrayElementAtIndex(i), GUILayout.ExpandWidth(true));
                    if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
                    {
                        if (i >= 0)
                        {
                            place.GetArrayElementAtIndex(i).objectReferenceValue = null;
                            place.DeleteArrayElementAtIndex(i);
                        }
                        GUILayout.EndHorizontal();
                        break;
                    }

                    GUILayout.EndHorizontal();

                }

                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add Place", EditorStyles.miniButtonLeft, GUILayout.ExpandWidth(false), GUILayout.Width(75)))
                {
                    place.InsertArrayElementAtIndex(place.arraySize);
                    place.GetArrayElementAtIndex(place.arraySize - 1).objectReferenceValue = null;
                }

                if (GUILayout.Button("Fill with Automated Places", EditorStyles.miniButtonMid, GUILayout.ExpandWidth(false), GUILayout.Width(155)))
                {
                    Places = Selection.activeGameObject.transform.FindChild("Places").gameObject;
                    if (Places != null)
                    {
                        for (int i = 0; i < Places.transform.childCount; i++)
                        {
                            Transform namePl = Places.transform.GetChild(i);
                            if (namePl != null && namePl.name.Contains("Formation" + indexPlacement +"-" + Selection.activeGameObject.name + "-"))
                            {
                                place.InsertArrayElementAtIndex(place.arraySize);
                                place.GetArrayElementAtIndex(place.arraySize - 1).objectReferenceValue = namePl.gameObject;

                            }
                        }
                    }
                }

                if (GUILayout.Button("Reset Places", EditorStyles.miniButtonRight, GUILayout.ExpandWidth(false), GUILayout.Width(95)))
                {
                    place.ClearArray();
                    /*for (int i = 0; i < place.arraySize; i++)
                    {
                        place.InsertArrayElementAtIndex(place.arraySize);
                        place.GetArrayElementAtIndex(place.arraySize - 1).objectReferenceValue = namePl.gameObject;

                    }*/
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                EditorGUI.indentLevel -= 1;
            }
            if (generate.boolValue)
            {

                SerializedProperty placeGeneration = property.FindPropertyRelative("PlaceGeneration");
                SerializedProperty nbrPlace = placeGeneration.FindPropertyRelative("PlaceNumber");
                SerializedProperty posref = placeGeneration.FindPropertyRelative("Position");
                SerializedProperty align = placeGeneration.FindPropertyRelative("Alignment");
                SerializedProperty leaderDist = placeGeneration.FindPropertyRelative("LeaderDistance");
                SerializedProperty AgentDist = placeGeneration.FindPropertyRelative("AgentDistance");
                SerializedProperty AgentOrient = placeGeneration.FindPropertyRelative("AgentOrientation");
                SerializedProperty alignNumber = placeGeneration.FindPropertyRelative("alignNumber");

                //SerializedProperty formation = property.FindPropertyRelative("FormationParameters");
                //SerializedProperty formationNumber = property.FindPropertyRelative("number");
                //EditorGUILayout.PropertyField(placeGeneration, new GUIContent("Place generation parameters"), true);
                GUILayout.Space(5);
                EditorGUILayout.PropertyField(posref, GUILayout.ExpandWidth(true));
                EditorGUILayout.PropertyField(nbrPlace, GUILayout.ExpandWidth(true));
                string[] aligns = { "0", "90", "180", "-90" };
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(new GUIContent("Alignment"));
                GUILayout.Space(-150);
                alignNumber.intValue = EditorGUILayout.Popup(alignNumber.intValue, aligns);
                if (ancAngleOrient != alignNumber.intValue)
                {
                    if (alignNumber.intValue == 0)
                        align.floatValue = 0;
                    if (alignNumber.intValue == 1)
                        align.floatValue = 90;
                    if (alignNumber.intValue == 2)
                        align.floatValue = 180;
                    if (alignNumber.intValue == 3)
                        align.floatValue = -90;
                }
                ancAngleOrient = alignNumber.intValue;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.PropertyField(leaderDist, GUILayout.ExpandWidth(true));
                EditorGUILayout.PropertyField(AgentDist, GUILayout.ExpandWidth(true));
                EditorGUILayout.PropertyField(AgentOrient, GUILayout.ExpandWidth(true));
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Generate Automatic Places", EditorStyles.miniButtonLeft, GUILayout.ExpandWidth(false), GUILayout.Width(155)))
                {
                    //posParam.GenerateAutoPlaces();
                    //SerializedProperty placeP = placeParam.FindPropertyRelative("UnityPlaces");
                    
                    Transform posref1 = null;
                    if (posref.objectReferenceValue != null)
                        posref1 = (Transform)posref.objectReferenceValue;
                    else
                        posref1 = Selection.activeGameObject.transform;

                    PrefabPlace = AssetDatabase.LoadAssetAtPath("Assets/LifeScene/Prefabs/PlacePrefab.prefab", typeof(GameObject)) as GameObject;
                    if (PrefabPlace != null && Selection.activeGameObject != null)
                    {
                        if (Selection.activeGameObject.transform.FindChild("Places") == null)
                        {
                            Places = new GameObject("Places");
                            Places.transform.parent = Selection.activeGameObject.transform;
                        }
                        else
                            Places = Selection.activeGameObject.transform.FindChild("Places").gameObject;

                        Places = Selection.activeGameObject.transform.FindChild("Places").gameObject;
                        List<GameObject> listDelete = new List<GameObject>();
                        if (Places != null)
                        {
                            for (int i = 0; i < Places.transform.childCount; i++)
                            {
                                Transform namePl = Places.transform.GetChild(i);
                                if (namePl != null && namePl.name.Contains("Formation" + indexPlacement + "-" + Selection.activeGameObject.name + "-"))
                                    listDelete.Add(namePl.gameObject);
                            }
                            for (int i = 0; i < listDelete.Count; i++)
                                GameObject.DestroyImmediate(listDelete[i]);
                        }
                        
                        PlacesNPC.Clear();
                        for (int i = 0; i < nbrPlace.intValue; i++)
                            PlacesNPC.Add(null);

                        float angle = align.floatValue * Mathf.PI / 180.0f;
                        float angleVar = (AgentDist.floatValue * 15) * Mathf.PI / 180.0f;
                        Vector3 ancPos1 = Vector3.one;
                        Vector3 ancPos2 = Vector3.one;

                        for (int i = 0; i < nbrPlace.intValue; i++)
                        {
                            string namePlace = "Formation" + indexPlacement + "-" + Selection.activeGameObject.name + "-" + (i + 1);
                            if (Places.transform.FindChild(namePlace) == null)
                            {
                                PlacesNPC.Insert(i, GameObject.Instantiate(PrefabPlace) as GameObject);
                                //PlacesNPC[i] = GameObject.Instantiate(PrefabPlace) as GameObject;
                                PlacesNPC[i].transform.parent = Places.transform;
                                //namePlace = "Formation" + Selection.activeGameObject.name + "-" + (PlacesNPC.Count);
                                PlacesNPC[i].name = namePlace;
                                PlacesNPC[i].transform.rotation = Quaternion.AngleAxis(AgentOrient.floatValue, Vector3.up);

                                if (typeForm.enumValueIndex == 1 && nbrPlace.intValue > 0 && posref1 != null)
                                {
                                    if (align.floatValue == -90)
                                        PlacesNPC[i].transform.position = new Vector3(posref1.position.x, posref1.position.y, posref1.position.z + leaderDist.floatValue + (AgentDist.floatValue) * i);
                                    if (align.floatValue == 90)
                                        PlacesNPC[i].transform.position = new Vector3(posref1.position.x, posref1.position.y, posref1.position.z - leaderDist.floatValue - (AgentDist.floatValue) * i);
                                    if (align.floatValue == 0)
                                        PlacesNPC[i].transform.position = new Vector3(posref1.position.x + leaderDist.floatValue + (AgentDist.floatValue) * i, posref1.position.y, posref1.position.z);
                                    if (align.floatValue == 180)
                                        PlacesNPC[i].transform.position = new Vector3(posref1.position.x - leaderDist.floatValue - (AgentDist.floatValue) * i, posref1.position.y, posref1.position.z);
                                    /*if(placeP != null){
                                        placeP.InsertArrayElementAtIndex(placeP.arraySize);
                                        placeP.GetArrayElementAtIndex(placeP.arraySize-1).objectReferenceValue = PlaceNPC;
                                    }*/
                                    //Debug.Log("PosRelX : " + PlacesNPC[i].transform.position.x);
                                    //Debug.Log("PosRelZ : " + PlacesNPC[i].transform.position.z);
                                }
                                else if (typeForm.enumValueIndex == 2 && nbrPlace.intValue > 0)
                                {

                                    if (i == 0)
                                    {
                                        if (align.floatValue == -90)
                                            PlacesNPC[i].transform.position = new Vector3(posref1.position.x, posref1.position.y, posref1.position.z + leaderDist.floatValue + (AgentDist.floatValue) * i);
                                        if (align.floatValue == 90)
                                            PlacesNPC[i].transform.position = new Vector3(posref1.position.x, posref1.position.y, posref1.position.z - leaderDist.floatValue - (AgentDist.floatValue) * i);
                                        if (align.floatValue == 0)
                                            PlacesNPC[i].transform.position = new Vector3(posref1.position.x + leaderDist.floatValue + (AgentDist.floatValue) * i, posref1.position.y, posref1.position.z);
                                        if (align.floatValue == 180)
                                            PlacesNPC[i].transform.position = new Vector3(posref1.position.x - leaderDist.floatValue - (AgentDist.floatValue) * i, posref1.position.y, posref1.position.z);
                                        //PlacesNPC[i].transform.position = new Vector3(posref1.position.x - leaderDist.floatValue * (Mathf.Cos(angle)), posref1.position.y, posref1.position.z + leaderDist.floatValue * (Mathf.Cos(angle)));
                                        angle = angle + angleVar;
                                        ancPos1 = PlacesNPC[i].transform.position;
                                        ancPos2 = PlacesNPC[i].transform.position;
                                        //Debug.Log("PosRelX : " + PlacesNPC[i].transform.position.x);
                                        //Debug.Log("PosRelZ : " + PlacesNPC[i].transform.position.z);
                                    }
                                    if (i >= 1)
                                    {
                                        if (align.floatValue == -90)
                                        {
                                            if (i % 2 == 0)
                                            {
                                                PlacesNPC[i].transform.position = new Vector3(ancPos2.x + (AgentDist.floatValue), posref1.position.y, ancPos2.z - (AgentDist.floatValue - 1));
                                                ancPos2 = PlacesNPC[i].transform.position;
                                                //Debug.Log("PosRelX : " + PlacesNPC[i].transform.position.x);
                                                //Debug.Log("PosRelZ : " + PlacesNPC[i].transform.position.z);
                                            }
                                            else
                                            {
                                                PlacesNPC[i].transform.position = new Vector3(ancPos1.x - (AgentDist.floatValue), posref1.position.y, ancPos1.z - (AgentDist.floatValue - 1));
                                                ancPos1 = PlacesNPC[i].transform.position;
                                                //Debug.Log("PosRelX : " + PlacesNPC[i].transform.position.x);
                                                //Debug.Log("PosRelZ : " + PlacesNPC[i].transform.position.z);
                                            }
                                        }

                                        if (align.floatValue == 90)
                                        {
                                            if (i % 2 == 0)
                                            {
                                                PlacesNPC[i].transform.position = new Vector3(ancPos2.x + (AgentDist.floatValue), posref1.position.y, ancPos2.z + (AgentDist.floatValue - 1));
                                                ancPos2 = PlacesNPC[i].transform.position;
                                                //Debug.Log("PosRelX : " + PlacesNPC[i].transform.position.x);
                                                //Debug.Log("PosRelZ : " + PlacesNPC[i].transform.position.z);
                                            }
                                            else
                                            {
                                                PlacesNPC[i].transform.position = new Vector3(ancPos1.x - (AgentDist.floatValue), posref1.position.y, ancPos1.z + (AgentDist.floatValue - 1));
                                                ancPos1 = PlacesNPC[i].transform.position;
                                                //Debug.Log("PosRelX : " + PlacesNPC[i].transform.position.x);
                                                //Debug.Log("PosRelZ : " + PlacesNPC[i].transform.position.z);
                                            }

                                        }
                                        if (align.floatValue == 0)
                                        {
                                            if (i % 2 == 0)
                                            {
                                                PlacesNPC[i].transform.position = new Vector3(ancPos2.x - (AgentDist.floatValue - 1), posref1.position.y, ancPos2.z - (AgentDist.floatValue));
                                                ancPos2 = PlacesNPC[i].transform.position;
                                                //Debug.Log("PosRelX : " + PlacesNPC[i].transform.position.x);
                                                //Debug.Log("PosRelZ : " + PlacesNPC[i].transform.position.z);
                                            }
                                            else
                                            {
                                                PlacesNPC[i].transform.position = new Vector3(ancPos1.x - (AgentDist.floatValue - 1), posref1.position.y, ancPos1.z + (AgentDist.floatValue));
                                                ancPos1 = PlacesNPC[i].transform.position;
                                                //Debug.Log("PosRelX : " + PlacesNPC[i].transform.position.x);
                                                //Debug.Log("PosRelZ : " + PlacesNPC[i].transform.position.z);
                                            }

                                        }
                                        if (align.floatValue == 180)
                                        {
                                            if (i % 2 == 0)
                                            {
                                                PlacesNPC[i].transform.position = new Vector3(ancPos2.x + (AgentDist.floatValue - 1), posref1.position.y, ancPos2.z + (AgentDist.floatValue));
                                                ancPos2 = PlacesNPC[i].transform.position;
                                                //Debug.Log("PosRelX : " + PlacesNPC[i].transform.position.x);
                                                //Debug.Log("PosRelZ : " + PlacesNPC[i].transform.position.z);
                                            }
                                            else
                                            {
                                                PlacesNPC[i].transform.position = new Vector3(ancPos1.x + (AgentDist.floatValue - 1), posref1.position.y, ancPos1.z - (AgentDist.floatValue));
                                                ancPos1 = PlacesNPC[i].transform.position;
                                                //Debug.Log("PosRelX : " + PlacesNPC[i].transform.position.x);
                                                //Debug.Log("PosRelZ : " + PlacesNPC[i].transform.position.z);
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (GUILayout.Button("Reset Automatic Places", EditorStyles.miniButtonRight, GUILayout.ExpandWidth(false), GUILayout.Width(155)))
                {
                    Places = Selection.activeGameObject.transform.FindChild("Places").gameObject;
                    List<GameObject> listDelete = new List<GameObject>();
                    if (Places != null)
                    {
                        for (int i = 0; i < Places.transform.childCount; i++)
                        {
                            Transform namePl = Places.transform.GetChild(i);
                            if (namePl != null && namePl.name.Contains("Formation" + indexPlacement + "-" + Selection.activeGameObject.name + "-"))
                                listDelete.Add(namePl.gameObject);
                        }
                        for (int i = 0; i < listDelete.Count; i++)
                            GameObject.DestroyImmediate(listDelete[i]);
                    }
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

            }
            EditorGUI.indentLevel -= 1;

        }
        EditorGUI.EndProperty();
    }

    /*public void reset(int i){
    SerializedProperty typeForm = property.FindPropertyRelative("TypeForm");
    SerializedProperty behavior = property.FindPropertyRelative("Behavior");
    SerializedProperty role = property.FindPropertyRelative("ExcluedRole");
    SerializedProperty generate = property.FindPropertyRelative ("GenerationOfPlace");
    SerializedProperty unity = property.FindPropertyRelative ("UnityPlace");
    SerializedProperty place = property.FindPropertyRelative("UnityPlaces");
    SerializedProperty placeGeneration = property.FindPropertyRelative("PlaceGeneration");

    place.ClearArray();
    role.stringValue = "None";
    typeForm.enumValueIndex = 0;
    behavior.stringValue = "None";
    placeGeneration.ClearArray();



}*/
}

