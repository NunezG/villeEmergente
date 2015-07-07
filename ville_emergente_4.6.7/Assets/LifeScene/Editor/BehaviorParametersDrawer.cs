using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BehaviorParameters))]
public class BehaviorParametersDrawer 
: PropertyDrawer 
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) 
	{
		BTNameManager BTnamesManag = BTNameManager.Instance;
		RoleManager roleNames = RoleManager.Instance;
		label = EditorGUI.BeginProperty(position, label, property);
		property.isExpanded = EditorGUILayout.Foldout( property.isExpanded, label );

		if( property.isExpanded )
		{
			EditorGUI.indentLevel += 1;

			SerializedProperty Behaviors = property.FindPropertyRelative("Behaviors");
			SerializedProperty Roles = property.FindPropertyRelative("Roles");

			if(Behaviors.arraySize == 0){
				Behaviors.InsertArrayElementAtIndex(0);
				Roles.InsertArrayElementAtIndex(0);
				Behaviors.GetArrayElementAtIndex(0).stringValue = "";
				Roles.GetArrayElementAtIndex(0).stringValue = "";
			}
			if(Roles.arraySize != Behaviors.arraySize)
				Roles.arraySize = Behaviors.arraySize;

			for( int i = 0 ; i < Behaviors.arraySize; ++i )
			{
				GUILayout.BeginHorizontal();

				SerializedProperty item = Behaviors.GetArrayElementAtIndex(i);
				string BTName = item.stringValue;

				int idx = -1;
				for( int j = 0 ; j < BTnamesManag.BTNames.Length; ++j )
					if( BTnamesManag.BTNames[ j ] == BTName )
						idx = j;
				int newIdx = EditorGUILayout.Popup( idx, BTnamesManag.BTNames );
				if( newIdx != idx )
					item.stringValue = BTnamesManag.BTNames[ newIdx ];

				int roleSelected = -1;
				SerializedProperty item1 = Roles.GetArrayElementAtIndex(i);
				string RoleName = item1.stringValue;

				for( int j = 0 ; j < roleNames.roleNames.Length; ++j )
					if( roleNames.roleNames[j] == RoleName )
						roleSelected = j;

				int newRoleSelected = EditorGUILayout.Popup( roleSelected, roleNames.roleNames);
				if( newRoleSelected != roleSelected )
					item1.stringValue = roleNames.roleNames[ newRoleSelected ];

				GUILayout.Space(5);
				if( GUILayout.Button ("-", EditorStyles.miniButton, GUILayout.ExpandWidth(false) ) )
				{
					if(i >= 0){
						Behaviors.DeleteArrayElementAtIndex(i);
						Roles.DeleteArrayElementAtIndex(i);
					}
					GUILayout.EndHorizontal();
					break;
				}
				GUILayout.Space(5);
				GUILayout.EndHorizontal();

			}

			GUILayout.Space(5);
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if( GUILayout.Button("Add Behavior", EditorStyles.miniButtonLeft, GUILayout.ExpandWidth(false), GUILayout.Width(100) ) )
			{
				Behaviors.InsertArrayElementAtIndex(Behaviors.arraySize-1);
				Behaviors.GetArrayElementAtIndex( Behaviors.arraySize-1 ).stringValue = string.Empty;
				Roles.InsertArrayElementAtIndex(Behaviors.arraySize-1);
				Roles.GetArrayElementAtIndex( Behaviors.arraySize-1 ).stringValue = string.Empty;
			}
			GUILayout.Space(0);
			if( GUILayout.Button("Edit Behaviors", EditorStyles.miniButtonRight, GUILayout.ExpandWidth(false), GUILayout.Width(100) ) )
			{
				Selection.activeObject = BTnamesManag;
			}

			EditorGUI.indentLevel -= 1;
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();


		}
		EditorGUI.EndProperty();
	}


}
