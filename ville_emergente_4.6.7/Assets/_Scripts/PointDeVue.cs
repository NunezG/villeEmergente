using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointDeVue : MonoBehaviour {



    public ConvolutionObject batimentAVisiter;
    public List<GameObject> spots = new List<GameObject>();
    public List<bool> availablesSpots = new List<bool>();
    public bool isBeingVisited = false;

	// Use this for initialization
	void Start () {
        foreach (Transform child in transform)
        {
            //child is your child transform
            if (child.gameObject.name == "spot")
            {
                spots.Add(child.gameObject);
                availablesSpots.Add(true);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
