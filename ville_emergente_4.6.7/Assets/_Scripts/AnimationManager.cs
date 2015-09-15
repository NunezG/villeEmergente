using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationManager : MonoBehaviour {

	public static AnimationManager instance;
   // uint bankID;
	// Use this for initialization

	//public string[] listeAnimations = {"Complete","Marche","MarcheFiere","Sautille","Danse","MontreUne","MontreDeux","Errance","CriErrance","Satisfait"};



	public void Awake()
	{
		instance = this;
		//LoadBank();
		//PlayFiniteEvent("ville_calme", this.gameObject);
	}

    public void Start()
    {
		Marche ();
    }

	
	// Update is called once per frame
	void Update () {
	/*
		if (Input.GetKeyDown (KeyCode.Space))
		{
			ClearTriggers ();
			this.GetComponentInChildren<Animator> ().SetBool (listeAnimations[Random.Range(0,listeAnimations.Length)], true);
			//transform.FindChild("mesh").GetComponent<AudioEventManager>().soundOuverture();

		} 
	*/
	}
    
/*	public void ClearTriggers()
	{
		this.GetComponentInChildren<Animator> ().SetBool ("Complete",false);
		this.GetComponentInChildren<Animator> ().SetBool ("Marche",false);
		this.GetComponentInChildren<Animator> ().SetBool ("MarcheFiere",false);
		this.GetComponentInChildren<Animator> ().SetBool ("Sautille",false);
		this.GetComponentInChildren<Animator> ().SetBool ("Danse",false);
		//this.GetComponentInChildren<Animator> ().SetBool ("MontreUne",false);
		this.GetComponentInChildren<Animator> ().SetBool ("MontreDeux",false);
		this.GetComponentInChildren<Animator> ().SetBool ("Errance",false);
		this.GetComponentInChildren<Animator> ().SetBool ("CriErrance",false);
		this.GetComponentInChildren<Animator> ().SetBool ("Satisfait",false);
	}

*/
	public void Complete()
	{

		//if (!this.GetComponent<Animator> ().GetBool ("Complete")) 
		//{
			//ClearTriggers ();
			this.GetComponent<Animator> ().SetTrigger ("Complete");
		//}
	}

	public void Marche()
	{
		//if (!this.GetComponent<Animator> ().GetBool ("Marche")) 
		//{
			//ClearTriggers ();
			this.GetComponent<Animator> ().SetTrigger ("Marche");
		//}
	}


	public void MarcheFiere()
	{
		//if (!this.GetComponent<Animator> ().GetBool ("MarcheFiere")) 
		//{
			//ClearTriggers ();
			this.GetComponent<Animator> ().SetTrigger ("MarcheFiere");
		//}
	}


	public void Sautille()
	{
		//if (!this.GetComponent<Animator> ().GetBool ("Sautille")) 
		///{
			//ClearTriggers ();
			this.GetComponent<Animator> ().SetTrigger ("Sautille");
		//}
	}


	public void Danse()
	{
		//if (!this.GetComponent<Animator> ().GetBool ("Danse")) 
		//{
			//ClearTriggers ();
			this.GetComponent<Animator> ().SetTrigger ("Danse");
		//}
	}


/*	public void MontreUne()
	{
		ClearTriggers ();
		this.GetComponent<Animator> ().SetBool ("MontreUne",true);
	}
*/

	public void MontreDeux()
	{
		//if (!this.GetComponent<Animator> ().GetBool ("MontreDeux")) 
		//{
			//ClearTriggers ();
			this.GetComponent<Animator> ().SetTrigger ("MontreDeux");
		//}
	}


	public void Errance()
	{

		//if (!this.GetComponent<Animator> ().GetBool ("Errance"))
		//{
			//ClearTriggers ();
			this.GetComponent<Animator> ().SetTrigger ("Errance");
		//}
	}


	public void CriErrance()
	{
		//Debug.Log ("this.GetComponent<Animator> ().GetCurrentAnimatorStateInfo().IsName: " + this.GetComponent<Animator> ().GetCurrentAnimationClipState(0)[0].);
		//if (this.GetComponent<Animator> ().GetCurrentAnimatorStateInfo(0).IsName("CriErrance")) 
		//{
			//ClearTriggers ();
			this.GetComponent<Animator> ().SetTrigger ("CriErrance");
		//}
	}

	public void Satisfait()
	{
		//if (!this.GetComponent<Animator> ().GetBool ("Satisfait")) {
			//ClearTriggers ();
			this.GetComponent<Animator> ().SetTrigger ("Satisfait");
		//}
	}
}
