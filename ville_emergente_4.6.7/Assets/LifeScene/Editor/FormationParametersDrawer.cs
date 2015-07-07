using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(FormationParameters))]
public class FormationParametersDrawer 
: PropertyDrawer 
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) 
	{
		label = EditorGUI.BeginProperty(position, label, property);
		property.isExpanded = EditorGUILayout.Foldout( property.isExpanded, label );
		if( property.isExpanded )
		{

			EditorGUI.indentLevel += 1;
			SerializedProperty formations = property.FindPropertyRelative("Formations");
            SerializedProperty ambientDist = property.FindPropertyRelative("distanceAmbient");

            EditorGUILayout.PropertyField(ambientDist, new GUIContent("Stop Distance/Zone"), true);
            
            for (int i = 0; i < formations.arraySize; ++i)
			{
				GUILayout.BeginHorizontal();
				string name = "Placement "+(i+1);
				GUILayout.BeginVertical();
				GUILayout.Space(-15);
				EditorGUILayout.PropertyField( formations.GetArrayElementAtIndex(i), new GUIContent(name), true);
				GUILayout.Space(5);
				GUILayout.EndVertical();
				GUILayout.Space(-10);
				if( GUILayout.Button ("-", EditorStyles.miniButton, GUILayout.ExpandWidth(true), GUILayout.Width(20) ) )
				{
					if(i >= 0){
						formations.DeleteArrayElementAtIndex(i);
					}
					GUILayout.EndHorizontal();
					break;
				}
				GUILayout.EndHorizontal();

			}

			GUILayout.Space(5);
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if( GUILayout.Button("Add Formation", EditorStyles.miniButton, GUILayout.ExpandWidth(false), GUILayout.Width(200) ) )
			{
				formations.InsertArrayElementAtIndex(formations.arraySize);
                formations.GetArrayElementAtIndex(formations.arraySize - 1).FindPropertyRelative("number").intValue = formations.arraySize;
                formations.GetArrayElementAtIndex(formations.arraySize - 1).FindPropertyRelative("ConcernedRole").stringValue = "none";
				formations.GetArrayElementAtIndex(formations.arraySize-1).FindPropertyRelative("GenerationOfPlace").boolValue = false;
				formations.GetArrayElementAtIndex(formations.arraySize-1).FindPropertyRelative("UnityPlace").boolValue = true;
				formations.GetArrayElementAtIndex(formations.arraySize-1).FindPropertyRelative("TypeForm").enumValueIndex = 0;
				formations.GetArrayElementAtIndex(formations.arraySize-1).FindPropertyRelative("UnityPlaces").arraySize = 0;
				formations.GetArrayElementAtIndex(formations.arraySize-1).FindPropertyRelative("PlaceGeneration").FindPropertyRelative("PlaceNumber").intValue = 0;
				formations.GetArrayElementAtIndex(formations.arraySize-1).FindPropertyRelative("PlaceGeneration").FindPropertyRelative("Position").objectReferenceValue = null;
				formations.GetArrayElementAtIndex(formations.arraySize-1).FindPropertyRelative("PlaceGeneration").FindPropertyRelative("Alignment").floatValue = 0;
				formations.GetArrayElementAtIndex(formations.arraySize-1).FindPropertyRelative("PlaceGeneration").FindPropertyRelative("LeaderDistance").floatValue = 0;
				formations.GetArrayElementAtIndex(formations.arraySize-1).FindPropertyRelative("PlaceGeneration").FindPropertyRelative("AgentDistance").floatValue = 0;
				formations.GetArrayElementAtIndex(formations.arraySize-1).FindPropertyRelative("PlaceGeneration").FindPropertyRelative("AgentOrientation").floatValue = 0;

			}

			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
            EditorGUI.indentLevel -= 1;


		}
		EditorGUI.EndProperty();
	}


}
