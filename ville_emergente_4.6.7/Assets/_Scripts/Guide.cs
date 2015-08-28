using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Guide : MonoBehaviour
{
    public static GameObject[] allPointsOfView = null;
   // public List<GameObject> targets = new List<GameObject>();
    public PointDeVue pdv;
    public Fragment fragment;
    public Material defaultMaterial, material;

    public void Awake()
    {
        tag = "NPC";
        // if(targets==null)
        //Debug.Log ("START MUSICIENN");
        int matriculePassant = int.Parse(this.gameObject.name.Substring(7));
        //print("matriculePassant " + matriculePassant);

        if (allPointsOfView == null)
            allPointsOfView = GameObject.FindGameObjectsWithTag("PointDeVue");
        /*foreach (GameObject gObject in allPointsOfView)
        {
            if (gObject.name == "NT_Passant_" + matriculePassant)
            {
                targets.Add(gObject);
            }
        }*/
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
