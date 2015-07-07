using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TriggerParameters))]
public class TriggerParametersDrawer 
: PropertyDrawer 
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) 
	{
		label = EditorGUI.BeginProperty(position, label, property);
		property.isExpanded = EditorGUILayout.Foldout( property.isExpanded, label );
		if( property.isExpanded )
		{
			EditorGUI.indentLevel += 1;

			//EditorGUILayout.PropertyField( property.FindPropertyRelative("type"), new GUIContent("Trigger Type"));

			//EditorGUILayout.PropertyField( property.FindPropertyRelative("behavior"), new GUIContent("Behavior"), true);
			EditorGUILayout.PropertyField( property.FindPropertyRelative("begining"), new GUIContent("Begining"), true);
            GUILayout.Space(5);
            EditorGUILayout.PropertyField(property.FindPropertyRelative("duration"), new GUIContent("Duration"), true);
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField( property.FindPropertyRelative("repetition"), new GUIContent("Repetition"), true);
			GUILayout.FlexibleSpace();
			EditorGUILayout.PrefixLabel(new GUIContent("Time"));
			GUILayout.Space(-60);
			EditorGUILayout.PropertyField( property.FindPropertyRelative("repetitionTime"), new GUIContent(""), GUILayout.Width(100));
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
            GUILayout.Space(5);
            SerializedProperty entityTrigger = property.FindPropertyRelative("entity");
            EditorGUILayout.PropertyField(entityTrigger, new GUIContent("Entity"), true);
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel(new GUIContent("Place"));
			GUILayout.Space(-20);
			EditorGUILayout.PropertyField( property.FindPropertyRelative("Place"), new GUIContent(""), true);
			GUILayout.EndHorizontal();

			/*GUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel(new GUIContent( "Place"));
			GUILayout.Space(-22);
			EditorGUILayout.PropertyField( property.FindPropertyRelative("Place"), new GUIContent("Place"), true);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.PropertyField( property.FindPropertyRelative("repet"), new GUIContent("Repetition"), true);
			EditorGUILayout.PropertyField( property.FindPropertyRelative("repetitionTime"), new GUIContent("Repetition Time"), true);*/

			GUILayout.Space(5);

			SerializedProperty conditionsBool = property.FindPropertyRelative("conditionsBool");
			SerializedProperty conditionsInt = property.FindPropertyRelative("conditionsInt");
			SerializedProperty places = property.FindPropertyRelative("Places");
            SerializedProperty so = property.FindPropertyRelative("SO");

			for( int i = 0 ; i < conditionsBool.arraySize; ++i )
			{
				SerializedProperty nameBool = conditionsBool.GetArrayElementAtIndex(i).FindPropertyRelative( "nameBool" );
				SerializedProperty triggerBool = conditionsBool.GetArrayElementAtIndex(i).FindPropertyRelative( "triggerBool" );

				GUILayout.BeginHorizontal();
				string nameCond = "Boolean "+(i+1);
				EditorGUILayout.PrefixLabel(new GUIContent(nameCond));
				EditorGUILayout.PropertyField(nameBool, new GUIContent(""));
				EditorGUILayout.PropertyField(triggerBool, new GUIContent(""));
                GUILayout.Space(5); 
                if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
				{
					if(i >= 0){
						conditionsBool.DeleteArrayElementAtIndex(i);
					}
					GUILayout.EndHorizontal();
					break;
				}
				
				GUILayout.Space(5);
				GUILayout.EndHorizontal();
                GUILayout.Space(5);
				
			}
			
			for( int i = 0 ; i < conditionsInt.arraySize; ++i )
			{
				SerializedProperty nameInt = conditionsInt.GetArrayElementAtIndex(i).FindPropertyRelative( "nameInt" );
				SerializedProperty triggerInt = conditionsInt.GetArrayElementAtIndex(i).FindPropertyRelative( "triggerInt" );
				
				GUILayout.BeginHorizontal();
				string nameCond = "Float "+(i+1);
				EditorGUILayout.PrefixLabel(new GUIContent(nameCond));
                //GUILayout.Space(-20); 
                EditorGUILayout.PropertyField(nameInt, new GUIContent(""));
				EditorGUILayout.PropertyField(triggerInt, new GUIContent(""));
                GUILayout.Space(5); 
                if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
				{
					if(i >= 0){
						conditionsInt.DeleteArrayElementAtIndex(i);
					}
					GUILayout.EndHorizontal();
					break;
				}
				
				GUILayout.Space(5);
				GUILayout.EndHorizontal();
                GUILayout.Space(5);
				
			}

			for( int i = 0 ; i < places.arraySize; ++i )
			{
				string name = "Place "+(i+1);
				GUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel(new GUIContent( name));
                GUILayout.Space(-15); 
                EditorGUILayout.PropertyField(places.GetArrayElementAtIndex(i));
				GUILayout.Space(5);
				if( GUILayout.Button ("-", EditorStyles.miniButton, GUILayout.ExpandWidth(false) ) )
				{
					if(i >= 0){
						places.GetArrayElementAtIndex(i).objectReferenceValue = null;
						places.DeleteArrayElementAtIndex(i);
					}
					GUILayout.EndHorizontal();
					break;
				}
				
				GUILayout.Space(5);
				GUILayout.EndHorizontal();
                GUILayout.Space(5);
            }

            for (int i = 0; i < so.arraySize; ++i)
            {
                string name = "GameObject " + (i + 1);
                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(new GUIContent(name));
                GUILayout.Space(-15);
                EditorGUILayout.PropertyField(so.GetArrayElementAtIndex(i), new GUIContent(""));
                GUILayout.Space(5);
                if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
                {
                    if (i >= 0)
                    {
                        so.GetArrayElementAtIndex(i).objectReferenceValue = null;
                        so.DeleteArrayElementAtIndex(i);
                    }
                    GUILayout.EndHorizontal();
                    break;
                }

                GUILayout.Space(5);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            }

            GUILayout.Space(5);
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if( GUILayout.Button("Add Boolean", EditorStyles.miniButton, GUILayout.ExpandWidth(false), GUILayout.Width(75) ) )
			{
				conditionsBool.InsertArrayElementAtIndex(conditionsBool.arraySize);
				conditionsBool.GetArrayElementAtIndex(conditionsBool.arraySize-1).FindPropertyRelative( "nameBool" ).stringValue = "";
				conditionsBool.GetArrayElementAtIndex(conditionsBool.arraySize-1).FindPropertyRelative( "triggerBool" ).boolValue = false;
			}
			if( GUILayout.Button("Add Float", EditorStyles.miniButton, GUILayout.ExpandWidth(false), GUILayout.Width(75) ) )
			{
				conditionsInt.InsertArrayElementAtIndex(conditionsInt.arraySize);
				conditionsInt.GetArrayElementAtIndex(conditionsInt.arraySize-1).FindPropertyRelative( "nameInt" ).stringValue = "";
				conditionsInt.GetArrayElementAtIndex(conditionsInt.arraySize-1).FindPropertyRelative( "triggerInt" ).floatValue = 0;
			}
			if( GUILayout.Button("Add Place", EditorStyles.miniButton, GUILayout.ExpandWidth(false), GUILayout.Width(75) ) )
			{
				places.InsertArrayElementAtIndex(places.arraySize);
				places.GetArrayElementAtIndex(places.arraySize-1).objectReferenceValue = null;
			}
            if (GUILayout.Button("Add GameObject", EditorStyles.miniButton, GUILayout.ExpandWidth(false), GUILayout.Width(100)))
            {
                so.InsertArrayElementAtIndex(so.arraySize);
                so.GetArrayElementAtIndex(so.arraySize - 1).objectReferenceValue = null;
            }

			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();	
			EditorGUI.indentLevel -= 1;

			
		}
		EditorGUI.EndProperty();
	}
}
