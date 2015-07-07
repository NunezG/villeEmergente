using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(LifeSceneRoleNames))]
public class LifeSceneRoleNamesDrawer : PropertyDrawer  {
	
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) 
	{
		RoleManager roleNames = RoleManager.Instance;
		label = EditorGUI.BeginProperty(position, label, property);

		EditorGUI.PropertyField( new Rect( position.x, position.y, 1f*position.width/2f, position.height ), property, new GUIContent(""));
		
		if( property.isArray == false && property.objectReferenceValue != null )
		{
			LifeSceneRoleNames lsp = property.objectReferenceValue as LifeSceneRoleNames;
			int roleSelected = -1;
			for (int i = 0; i < roleNames.roleNames.Length; ++i) 
			{
				if( roleNames.roleNames[i] == lsp.allowedRole )
					roleSelected = i;
			}
			int newRoleSelected = EditorGUI.Popup ( new Rect( position.x+(1f*position.width/2f), position.y, position.width/2f, position.height ), roleSelected, roleNames.roleNames);
			if( newRoleSelected != roleSelected )
				lsp.allowedRole = roleNames.roleNames[ newRoleSelected ];
			
		}
		EditorGUI.EndProperty();
	}
	
	
	
}