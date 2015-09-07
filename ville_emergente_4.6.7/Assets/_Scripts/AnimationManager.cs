using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationManager : MonoBehaviour {

	public static AnimationManager instance;
   // uint bankID;
	// Use this for initialization

	public string[] listeAnimations = {"Complete","Marche","MarcheFiere","Sautille","Danse","MontreUne","MontreDeux","Errance","CriErrance","Satisfait"};



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
	
		if (Input.GetKeyDown (KeyCode.Space))
		{
			ClearTriggers ();
			this.GetComponentInChildren<Animator> ().SetBool (listeAnimations[Random.Range(0,listeAnimations.Length)], true);

		} 

	}
    
	public void ClearTriggers()
	{
		this.GetComponentInChildren<Animator> ().SetBool ("Complete",false);
		this.GetComponentInChildren<Animator> ().SetBool ("Marche",false);
		this.GetComponentInChildren<Animator> ().SetBool ("MarcheFiere",false);
		this.GetComponentInChildren<Animator> ().SetBool ("Sautille",false);
		this.GetComponentInChildren<Animator> ().SetBool ("Danse",false);
		this.GetComponentInChildren<Animator> ().SetBool ("MontreUne",false);
		this.GetComponentInChildren<Animator> ().SetBool ("MontreDeux",false);
		this.GetComponentInChildren<Animator> ().SetBool ("Errance",false);
		this.GetComponentInChildren<Animator> ().SetBool ("CriErrance",false);
		this.GetComponentInChildren<Animator> ().SetBool ("Satisfait",false);
	}


	public void Complete()
	{
		ClearTriggers ();
		this.GetComponent<Animator> ().SetBool ("Complete",true);
	}

	public void Marche()
	{
		ClearTriggers ();
		this.GetComponent<Animator> ().SetBool ("Marche",true);
	}


	public void MarcheFiere()
	{

		ClearTriggers ();
		this.GetComponent<Animator> ().SetBool ("MarcheFiere",true);
	}


	public void Sautille()
	{
		ClearTriggers ();
		this.GetComponent<Animator> ().SetBool ("Sautille",true);
	}


	public void Danse()
	{
		ClearTriggers ();
		this.GetComponent<Animator> ().SetBool ("Danse",true);
	}


	public void MontreUne()
	{
		ClearTriggers ();
		this.GetComponent<Animator> ().SetBool ("MontreUne",true);
	}


	public void MontreDeux()
	{
		ClearTriggers ();
		this.GetComponent<Animator> ().SetBool ("MontreDeux",true);
	}


	public void Errance()
	{
		ClearTriggers ();
		this.GetComponent<Animator> ().SetBool ("Errance",true);
	}


	public void CriErrance()
	{
		ClearTriggers ();
		this.GetComponent<Animator> ().SetBool ("CriErrance",true);
	}

	public void Satisfait()
	{
		ClearTriggers ();
		this.GetComponent<Animator> ().SetBool ("Satisfait",true);
	}


}
