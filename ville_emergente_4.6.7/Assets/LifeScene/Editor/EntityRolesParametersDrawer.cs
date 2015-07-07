using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions; 
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Object = UnityEngine.Object;

[CustomPropertyDrawer(typeof(EntityRolesParameters))]
public class EntityRolesParametersDrawer : PropertyDrawer  {
	
	//private List<string> nameEntity = new List<string>();
	//private int charactNumber = 0;
	//private int entitySize = -1;
    private GameObject[] selectNPC;
    private GameObject actualWindow;
    private bool clicked = false;
    private bool clickedB = false;
    //private SerializedProperty selectEntities;
    //private SerializedProperty selectEntitiesAff;


	int CompareObNames( GameObject x, GameObject y )
	{
		int result = 0;
		Regex re = new Regex(@"(.*)(\d+)$");
		Match m1 = re.Match(x.name);
		Match m2 = re.Match(y.name);
		if (m1.Success && m2.Success)
		{
			result = m1.Groups[1].Value.CompareTo(m2.Groups[1].Value);
			if(result==0)
				result = int.Parse(m1.Groups[2].Value).CompareTo(int.Parse(m2.Groups[2].Value));
		}
		else{
			result = x.name.CompareTo(y.name);
		}
		return result;
	}
    
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) 
	{

		/*if(charact.Length != charactNumber){
			for(int i = 0; i < charact.Length; i++)
				nameEntity.Add( charact[i].name);
			charactNumber = charact.Length;
		}*/
		RoleManager roleNames = RoleManager.Instance;
		label = EditorGUI.BeginProperty(position, label, property);
		property.isExpanded = EditorGUILayout.Foldout( property.isExpanded, label );

		if( property.isExpanded )
		{
			
			EditorGUI.indentLevel += 1;
			SerializedProperty entities = property.FindPropertyRelative("Entities");
            SerializedProperty entitiesAff = property.FindPropertyRelative("EntitiesAff");
            //selectEntities = property.FindPropertyRelative("SelectEntities");
            //selectEntitiesAff = property.FindPropertyRelative("SelectEntitiesAff");
            SerializedProperty entitySize = property.FindPropertyRelative("entitySize");

			GameObject[] charact = GameObject.FindGameObjectsWithTag("Character");
			Array.Sort( charact, CompareObNames );
			string scene = property.serializedObject.targetObject.name;
            
            if (entities.arraySize == 0)
                entitySize.intValue = -1;

            for (int i = 0; i < entities.arraySize; i++)
                if (entities.GetArrayElementAtIndex(i).objectReferenceValue == null)
                    entities.arraySize = 0;

            for (int i = 0; i < entitiesAff.arraySize; i++)
                if (entitiesAff.GetArrayElementAtIndex(i).objectReferenceValue == null)
                    entitiesAff.arraySize = 0;

            if (entities.arraySize != entitySize.intValue || entities.GetArrayElementAtIndex(0).objectReferenceValue == null || entitiesAff.GetArrayElementAtIndex(0).objectReferenceValue == null || entities.arraySize != charact.Length || entitiesAff.arraySize != entitySize.intValue || entitiesAff.arraySize != charact.Length)
            {
				entities.arraySize = 0;
                entitiesAff.arraySize = 0;
				for(int i = 0; i < charact.Length; i++){
					entities.InsertArrayElementAtIndex(i);
					entities.GetArrayElementAtIndex(i).objectReferenceValue = charact[i];
                    entitiesAff.InsertArrayElementAtIndex(i);
                    entitiesAff.GetArrayElementAtIndex(i).objectReferenceValue = charact[i];
				}
				entitySize.intValue = entities.arraySize;
			}
				
			for(int i = 0; i < charact.Length; i++){
				LifeSceneRoleNames lsr = entities.GetArrayElementAtIndex(i).objectReferenceValue as LifeSceneRoleNames;
                for (int k = 0; k < lsr.LifeScenes.Length; k++)
                {
					if(scene == lsr.LifeScenes[k]){
						int roleSelected = -1;
						int index = -1;
						for (int j = 0; j < roleNames.roleNames.Length; ++j) 
						{
							if( roleNames.roleNames[j] == lsr.Roles[k] ){ 
									index = k;
									roleSelected = j;
							}
						}
						GUILayout.BeginHorizontal();
                        GUI.enabled = false;
                        EditorGUILayout.PropertyField(entitiesAff.GetArrayElementAtIndex(i), new GUIContent(""));
                        GUI.enabled = true;
						//EditorGUILayout.PrefixLabel(charact[i].name);
						int newRoleSelected = EditorGUILayout.Popup( roleSelected, roleNames.roleNames, GUILayout.Width(150));
						if( newRoleSelected != roleSelected )
							lsr.Roles[index] = roleNames.roleNames[ newRoleSelected ];

						GUILayout.Space(5);

                        /*if(lsr.Roles[k] == "Spectator")
                            GUILayout.Toggle(follow.boolValue, new GUIContent("Follow Main"), GUILayout.ExpandWidth(true)) ;

                        GUILayout.Space(5);*/

                        if ( GUILayout.Button ("-", EditorStyles.miniButton, GUILayout.ExpandWidth(false) ) )
						{
							if(i >= 0){
								List<string> list1 = new List<string>(lsr.LifeScenes);
								list1.RemoveAt(k);
								lsr.LifeScenes =  list1.ToArray();
								List<string> list2 = new List<string>(lsr.Roles);
								list2.RemoveAt(k);
								lsr.Roles =  list2.ToArray();
							}
							GUILayout.EndHorizontal();			
							break;
						}
						
						GUILayout.Space(5);
						GUILayout.EndHorizontal();	
					}
				}
				if(PrefabUtility.GetPrefabParent(lsr.gameObject) != null)
					PrefabUtility.RecordPrefabInstancePropertyModifications(lsr);
			}

			/*nameEntity.Sort((string s1, string s2)=>
			                {
				int result = 0;
				Regex re = new Regex(@"(.*)(\d+)$");
				Match m1 = re.Match(s1);
				Match m2 = re.Match(s2);
				if (m1.Success && m2.Success)
				{
					result = m1.Groups[1].Value.CompareTo(m2.Groups[1].Value);
					if(result==0)
						result = int.Parse(m1.Groups[2].Value).CompareTo(int.Parse(m2.Groups[2].Value));
				}
				else{
					result = s1.CompareTo(s2);
				}
				return result;
				
			});

			for(int i = 0; i < charact.Length; i++){
				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(new GUIContent( nameEntity[i]));
				GUILayout.EndHorizontal();
			}*/


			GUILayout.Space(15);
			SerializedProperty newEntity = property.FindPropertyRelative("newEntity");
			GUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Drag a new Character");
			EditorGUILayout.PropertyField(newEntity);
			GUILayout.EndHorizontal();
			GUILayout.Space(5);
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			
            if( GUILayout.Button("Add an Entity and a Role", EditorStyles.miniButton, GUILayout.ExpandWidth(false), GUILayout.Width(150) ) )
			{
				for(int i = 0; i < charact.Length; i++){
					if(newEntity.objectReferenceValue.name == charact[i].name){
						LifeSceneRoleNames lsr1 = entities.GetArrayElementAtIndex(i).objectReferenceValue as LifeSceneRoleNames;
						List<string> list1 = new List<string>(lsr1.LifeScenes);
						list1.Insert(lsr1.LifeScenes.Length, scene);
						lsr1.LifeScenes =  list1.ToArray();
						List<string> list2 = new List<string>(lsr1.Roles);
						list2.Insert(lsr1.Roles.Length, lsr1.allowedRole);;
						lsr1.Roles =  list2.ToArray();
					}
				}
				newEntity.objectReferenceValue = null;
			}
            GUILayout.FlexibleSpace(); 
            GUILayout.EndHorizontal();
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("Multi-Selection of Characters");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(5); 
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Space(5);

            Type type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.InspectorWindow");
            PropertyInfo propertyInfo = type.GetProperty("isLocked");
            SerializedProperty buttonText = property.FindPropertyRelative("buttonText");
            //string textbutton = "Lock Inspector and Select Characters";

            clicked = GUILayout.Toggle(clicked, new GUIContent(buttonText.stringValue), EditorStyles.miniButton, GUILayout.ExpandWidth(false), GUILayout.Width(220));
            if (clicked == true && clickedB == false)
            {
                clickedB = true;       
                if (EditorWindow.focusedWindow != null && EditorWindow.focusedWindow.GetType().Name == "InspectorWindow" && clickedB == true)
                {
                    propertyInfo.SetValue(EditorWindow.focusedWindow, true, null);
                    actualWindow = Selection.activeGameObject;
                    buttonText.stringValue = "Unlock Inspector after Adding Characters";
                }
            }

            if (clicked == false && clickedB == true) {
                clickedB = false;
                if (EditorWindow.focusedWindow != null && EditorWindow.focusedWindow.GetType().Name == "InspectorWindow" && clickedB == false)
                {
                    Selection.activeGameObject = actualWindow; 
                    propertyInfo.SetValue(EditorWindow.focusedWindow, false, null);
                    buttonText.stringValue = "Lock Inspector and Select Characters";

                }
            }

            if (clickedB == true)
                GUI.enabled = true;
            else
                GUI.enabled = false;

            if (GUILayout.Button("Add selected Characters", EditorStyles.miniButton, GUILayout.ExpandWidth(false), GUILayout.Width(150)))
            {
                selectNPC = Selection.gameObjects;
                Array.Sort(selectNPC, CompareObNames);
                for (int i = 0; i < charact.Length; i++)
                {
                    for (int j = 0; j < selectNPC.Length; j++)
                    {
                        if (selectNPC[j].name == charact[i].name)
                        {
                            int alreadyLS = 0;
                            LifeSceneRoleNames lsr3 = entities.GetArrayElementAtIndex(i).objectReferenceValue as LifeSceneRoleNames;
                            for (int k = 0; k < lsr3.LifeScenes.Length; k++)
                                if (scene == lsr3.LifeScenes[k])
                                    alreadyLS = 1;
                            if (alreadyLS == 0)
                            {
                                List<string> list1 = new List<string>(lsr3.LifeScenes);
                                list1.Insert(lsr3.LifeScenes.Length, scene);
                                lsr3.LifeScenes = list1.ToArray();
                                List<string> list2 = new List<string>(lsr3.Roles);
                                list2.Insert(lsr3.Roles.Length, lsr3.allowedRole); ;
                                lsr3.Roles = list2.ToArray();
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

			EditorGUI.indentLevel -= 1;


		}
		EditorGUI.EndProperty();

	}

 	
}