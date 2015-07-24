using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneRange : MonoBehaviour {

    public GameObject mainActor;
    public List<GameObject> spots = new List<GameObject>();

    public List<bool> availablesSpots = new List<bool>();

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

    void OnTriggerEnter(Collider other)
    {
        //print("SOnTriggerEnterOnTriggerEnterOnTriggerEnter"+other.name);

        if (other.tag == "NPC" && IsThereAvailableSpot())
        {
            Passant passant = other.GetComponent<Passant>();
            if (passant != null)
            {
                passant.SetInRangeOfScene(true);
                passant.availableScenes.Add(this);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "NPC")
        {
            Passant passant = other.GetComponent<Passant>();
            if (passant != null)
            {
                passant.SetInRangeOfScene(false);
                passant.availableScenes.Remove(this);
            }
        }
    }


    public bool IsThereAvailableSpot()
    {
        for (int i = 0; i < availablesSpots.Count; i++)
        {
            if (availablesSpots[i])
                return true;
        }
        return false;
    }

}
