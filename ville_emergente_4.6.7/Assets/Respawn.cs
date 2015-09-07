using UnityEngine;
using System.Collections;

public class Respawn : MonoBehaviour {

    public Vector3 respawn;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (transform.position.y < -30.0f)
            transform.position = respawn;
	}
}
