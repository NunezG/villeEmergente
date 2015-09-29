using UnityEngine;
using System.Collections;

//Script simple pour qu'un objet fasse constamment face à la caméra
public class Billboarding : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(Camera.main.transform.position, Vector3.up);
	}
}
