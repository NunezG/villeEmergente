using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(PlaceParameters))]
public class PlaceParametersDrawer 
: PropertyDrawer 
{ 
	private string name;
	private GameObject PrefabPlace;
	private GameObject PlaceNPC;
	private GameObject Places;
	private List<GameObject> PlacesNPC = new List<GameObject>();
	private SerializedProperty place ;
    private SerializedProperty number;

	void GeneratePlaces(){
		PrefabPlace = AssetDatabase.LoadAssetAtPath("Assets/LifeScene/Prefabs/PlacePrefab.prefab", typeof(GameObject)) as GameObject;
		if(PrefabPlace != null && Selection.activeGameObject != null){
			if(Selection.activeGameObject.transform.FindChild("Places") == null ){
				Places = new GameObject("Places");
				Places.transform.parent = Selection.activeGameObject.transform;
			}
			number.intValue += 1;
			PlacesNPC.Add(GameObject.Instantiate(PrefabPlace) as GameObject);
			PlacesNPC[PlacesNPC.Count-1].transform.parent = Selection.activeGameObject.transform.FindChild("Places");
            string namePlace = "Place" + "-" + Selection.activeGameObject.name + "-" + number.intValue;
			PlacesNPC[PlacesNPC.Count-1].name = namePlace;
			if(place != null){
				place.InsertArrayElementAtIndex(place.arraySize);
				place.GetArrayElementAtIndex(place.arraySize-1 ).objectReferenceValue = PlacesNPC[PlacesNPC.Count-1];
			}
			PlacesNPC[PlacesNPC.Count-1].transform.position = Selection.activeGameObject.transform.position;
		}
		
	}
	
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) 
	{
		label = EditorGUI.BeginProperty(position, label, property);
		property.isExpanded = EditorGUILayout.Foldout( property.isExpanded, label );
		if( property.isExpanded )
		{

            EditorGUI.indentLevel += 1;
			place = property.FindPropertyRelative("UnityPlaces");
            number = property.FindPropertyRelative("Number");
			/*if(place.arraySize == 0){
				place.InsertArrayElementAtIndex(0);
			}*/

			for( int i = 0 ; i < place.arraySize; ++i )
			{
				name = "Place "+(i+1);
				GUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel(new GUIContent( name));
				GUILayout.Space(-22);
				EditorGUILayout.PropertyField( place.GetArrayElementAtIndex(i));
				GUILayout.Space(5);
				if( GUILayout.Button ("-", EditorStyles.miniButton, GUILayout.ExpandWidth(false) ) )
				{
					if(i >= 0){
                        List<GameObject> listDelete = new List<GameObject>();
						for (int j = 0; j < PlacesNPC.Count; j++)
                    	{
							if (PlacesNPC[j] != null && place.GetArrayElementAtIndex(i).objectReferenceValue != null && PlacesNPC[j].name == place.GetArrayElementAtIndex(i).objectReferenceValue.name)
                            	listDelete.Add(PlacesNPC[j]);
                                
                    	}
                    	for(int j = 0; j < listDelete.Count; j++)
                        	GameObject.DestroyImmediate(listDelete[j]);

 						place.GetArrayElementAtIndex(i).objectReferenceValue = null;
						place.DeleteArrayElementAtIndex(i);
                        //PlacesNPC.RemoveAt(i);
                        
					}
					//GUILayout.FlexibleSpace();
					GUILayout.EndHorizontal();
					break;
				}
				
				GUILayout.Space(5);
				//GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();			
				
			}

			GUILayout.Space(5);
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if( GUILayout.Button("Generate Place", EditorStyles.miniButtonLeft, GUILayout.ExpandWidth(false), GUILayout.Width(100) ) )
			{
				GeneratePlaces();

			}
			if( GUILayout.Button("Add Existing Place", EditorStyles.miniButtonRight, GUILayout.ExpandWidth(false), GUILayout.Width(125) ) )
			{
				place.InsertArrayElementAtIndex(place.arraySize);
				place.GetArrayElementAtIndex(place.arraySize-1 ).objectReferenceValue = null;
                PlacesNPC.Add(null);
				number.intValue += 1;

			}

			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			EditorGUI.indentLevel -= 1;


		}
		EditorGUI.EndProperty();
	}
}
