using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TriggerTypeParameters))]
public class TriggerTypeParametersDrawer 
: PropertyDrawer 
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) 
	{
		label = EditorGUI.BeginProperty(position, label, property);
		property.isExpanded = EditorGUILayout.Foldout( property.isExpanded, label );
		if( property.isExpanded )
		{
				
			EditorGUI.indentLevel += 1;
			SerializedProperty triggerType = property.FindPropertyRelative("triggerParameters");
			if(triggerType.arraySize == 0){
				triggerType.InsertArrayElementAtIndex(0);
			}
			for( int i = 0 ; i < triggerType.arraySize; ++i )
			{
				GUILayout.BeginHorizontal();
				string name = "Trigger "+(i+1);
				GUILayout.BeginVertical();
				GUILayout.Space(-15);
				EditorGUILayout.PropertyField( triggerType.GetArrayElementAtIndex(i), new GUIContent(name), true);
				GUILayout.Space(5);
				GUILayout.EndVertical();
				GUILayout.Space(-10);
				if( GUILayout.Button ("-", EditorStyles.miniButton, GUILayout.ExpandWidth(true), GUILayout.Width(20) ) )
				{
					if(i > 0){
						triggerType.DeleteArrayElementAtIndex(i);
					}
					GUILayout.EndHorizontal();
					break;
				}
				GUILayout.EndHorizontal();
				
				
			}
			
			GUILayout.Space(5);
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if( GUILayout.Button("Add Trigger", EditorStyles.miniButton, GUILayout.ExpandWidth(false), GUILayout.Width(200) ) )
			{
				triggerType.InsertArrayElementAtIndex(triggerType.arraySize-1);
				triggerType.GetArrayElementAtIndex(triggerType.arraySize-1).FindPropertyRelative("type").enumValueIndex = 0;
				triggerType.GetArrayElementAtIndex(triggerType.arraySize-1).FindPropertyRelative("conditionsBool").arraySize = 0;
				triggerType.GetArrayElementAtIndex(triggerType.arraySize-1).FindPropertyRelative("conditionsInt").arraySize = 0;
				triggerType.GetArrayElementAtIndex(triggerType.arraySize-1).FindPropertyRelative("Places").arraySize = 0;
				triggerType.GetArrayElementAtIndex(triggerType.arraySize-1).FindPropertyRelative("behavior").stringValue = "none";
				triggerType.GetArrayElementAtIndex(triggerType.arraySize-1).FindPropertyRelative("begining").floatValue = 0;

			}
			
			EditorGUI.indentLevel -= 1;
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			
			
		}
		EditorGUI.EndProperty();

	}
}
