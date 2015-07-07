using UnityEngine;
using System;

[Serializable]
public class RoleParameters
{

	[System.Serializable]
	public class RoleAffect
	{
		public string Role;
		public float typeRole;
	}

	public string[] Roles;
	public string[] RolesType;
	public bool[] follow;
}


