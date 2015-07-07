using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditorInternal;

[CustomPropertyDrawer(typeof(RoleParameters))]
public class RoleParametersDrawer 
	: PropertyDrawer 
{
	
	private ReorderableList list;
	private List<int> idx = new List<int>();
	private List<int> roleTypeSelected = new List<int>();
	private bool followMain;
	
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) 
	{
		RoleManager roleNames = RoleManager.Instance;
		TypeRoleManager typeRole = TypeRoleManager.Instance;
		label = EditorGUI.BeginProperty(position, label, property);
		property.isExpanded = EditorGUILayout.Foldout( property.isExpanded, label );
		if( property.isExpanded )
		{
			EditorGUI.indentLevel += 1;
			SerializedProperty Roles = property.FindPropertyRelative("Roles");
			SerializedProperty rolesType = property.FindPropertyRelative("RolesType");
			SerializedProperty follow = property.FindPropertyRelative("follow");
			if(Roles.arraySize == 0){
				Roles.InsertArrayElementAtIndex(0);
				rolesType.InsertArrayElementAtIndex(0);
				Roles.GetArrayElementAtIndex(0).stringValue = "";
				rolesType.GetArrayElementAtIndex(0).stringValue = "";
			}
			if(rolesType.arraySize != Roles.arraySize){
				rolesType.arraySize = Roles.arraySize;
			}
			follow.arraySize = Roles.arraySize;

			idx.Clear();
			roleTypeSelected.Clear();
			for( int i = 0 ; i < Roles.arraySize; ++i ){
				GUILayout.BeginHorizontal();
				SerializedProperty item = Roles.GetArrayElementAtIndex(i);
				string RoleName = item.stringValue;
				idx.Add(-1);
				
				for( int j = 0 ; j < roleNames.roleNames.Length; ++j ){
					if( roleNames.roleNames[ j ] == RoleName ){
						idx[i] = j;
					}
				}
				
				for( int k = 0 ; k < Roles.arraySize; ++k ){
					if(i != k && RoleName == Roles.GetArrayElementAtIndex(k).stringValue && RoleName != ""){
						idx[i] = 0;
						item.stringValue = "";
					}
					
				}
				int NewIdx = EditorGUILayout.Popup( idx[i], roleNames.roleNames, GUILayout.Width(185) );
				if( NewIdx != idx[i] ){
					item.stringValue = roleNames.roleNames[ NewIdx ];
				}
				
				roleTypeSelected.Add(-1);
				SerializedProperty item1 = rolesType.GetArrayElementAtIndex(i);
				string RoleTypeName = item1.stringValue;
				
				for( int j = 0 ; j < typeRole.typeRoles.Length; ++j )
					if( typeRole.typeRoles[j] == RoleTypeName )
						roleTypeSelected[i] = j;
				
				/*for( int k = 0 ; k < rolesType.arraySize; ++k ){
					if(i != k && RoleTypeName == rolesType.GetArrayElementAtIndex(k).stringValue && RoleTypeName != "" ){
						roleTypeSelected[i] = 0;
						item1.stringValue = "";
					}
					
				}*/
				int newroleTypeSelected = EditorGUILayout.Popup( roleTypeSelected[i], typeRole.typeRoles);
				if( newroleTypeSelected != roleTypeSelected[i] )
					item1.stringValue = typeRole.typeRoles[newroleTypeSelected];

				GUILayout.Space(5);
                if (RoleTypeName == "Actor")
					follow.GetArrayElementAtIndex(i).boolValue = GUILayout.Toggle(follow.GetArrayElementAtIndex(i).boolValue, new GUIContent("Follow Lead")) ;

				GUILayout.Space(5);
				if( GUILayout.Button ("-", EditorStyles.miniButton, GUILayout.ExpandWidth(false) ) )
				{
					if(i >= 0){
						Roles.DeleteArrayElementAtIndex(i);
						rolesType.DeleteArrayElementAtIndex(i);
					}
					GUILayout.EndHorizontal();
					break;
				}
				
				GUILayout.Space(5);
				GUILayout.EndHorizontal();
					
			}
			
			/*list = new ReorderableList(property.serializedObject, property.FindPropertyRelative("Roles"), true, true, true, true);
            list.drawHeaderCallback += rect => GUI.Label(rect, label);
            list.drawElementCallback += (rect, index, active, focused) =>
            {
				SerializedProperty item = list.serializedProperty.GetArrayElementAtIndex( index );
                rect.height = 16;
                rect.y += 2;
                rect.width /= 2;
				string RoleName = item.stringValue;
				
				if (roleNames.roleNames.Length == 0)
					roleNames.roleNames.SetValue("none", 0); 
				int idx = -1;
				for( int j = 0 ; j < roleNames.roleNames.Length; ++j )
					if( roleNames.roleNames[ j ] == RoleName )
						idx = j;
				
				int newIdx = EditorGUI.Popup( rect, idx, roleNames.roleNames );
				if( newIdx != idx )
					item.stringValue = roleNames.roleNames[ newIdx ];
 
				rect.x += rect.width;
				//EditorGUI.PropertyField(rect, rolesType.GetArrayElementAtIndex(index), GUIContent.none);
				int newIdx1 = EditorGUI.Popup( rect, 0, typeRole.enumNames );

            };
			//list.onCanRemoveCallback += lst => true;
			//list.onSelectCallback += index;
			//list.DoList(r);
			list.DoLayoutList();*/
			
			GUILayout.Space(5);
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if( GUILayout.Button("Add Role", EditorStyles.miniButtonLeft, GUILayout.ExpandWidth(false), GUILayout.Width(100) ) )
			{
				Roles.InsertArrayElementAtIndex(Roles.arraySize-1);
				Roles.GetArrayElementAtIndex(Roles.arraySize-1 ).stringValue = string.Empty;
				rolesType.InsertArrayElementAtIndex(Roles.arraySize-1);
				rolesType.GetArrayElementAtIndex(Roles.arraySize-1 ).stringValue = string.Empty;
			}
			if( GUILayout.Button("Edit Roles", EditorStyles.miniButtonRight, GUILayout.ExpandWidth(false), GUILayout.Width(100) ) )
			{
				Selection.activeObject = roleNames;
			}
			
			EditorGUI.indentLevel -= 1;
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			
			
		}
		EditorGUI.EndProperty();
	}
}
