using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MotivationParameters))]
public class MotivationParametersDrawer 
: PropertyDrawer 
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) 
	{
		label = EditorGUI.BeginProperty(position, label, property);
		property.isExpanded = EditorGUILayout.Foldout( property.isExpanded, label );
		if( property.isExpanded )
		{
			EditorGUI.indentLevel += 1;

			EditorGUILayout.PropertyField( property.FindPropertyRelative("Motivations"), new GUIContent("Motivations"), true);

			EditorGUI.indentLevel -= 1;

		}
		EditorGUI.EndProperty();
	}
}
