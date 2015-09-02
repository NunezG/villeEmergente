using UnityEngine;
using System.Collections;

public class NextScene : MonoBehaviour {

	public string m_scene;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LoadScene(){
		Application.LoadLevel(m_scene);
	}
}
