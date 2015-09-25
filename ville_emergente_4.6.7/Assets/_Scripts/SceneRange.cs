﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneRange : MonoBehaviour {

    public GameObject mainActor;
    public List<GameObject> spots = new List<GameObject>();

    public List<bool> availablesSpots = new List<bool>();
    public List<Passant> passantsInRange = new List<Passant>();

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

        if (other.tag == "NPC" && other.GetComponent<Passant>()!=null  && IsThereAvailableSpot())
        {
            Passant passant = other.GetComponent<Passant>();
            passantsInRange.Add(passant);
            if (passant != null)
            {
                passant.SetInRangeOfScene(true);
                passant.availableScenes.Add(this);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "NPC" && other.GetComponent<Passant>() != null)
        {
            Passant passant = other.GetComponent<Passant>();
            passantsInRange.Remove(passant);
            if (passant != null)
            {
                if (passant.availableScenes.Remove(this))
                {
                    print("patate");
                }
                else
                {
                    print("carotte");
                }
                passant.SetInRangeOfScene(false);
            }
        }
    }

    public void DeactivateScene()
    {
        for (int i = 0; i < passantsInRange.Count; i++)
        {
            passantsInRange[i].availableScenes.Remove(this);
            passantsInRange[i].SetInRangeOfScene(false);
        }
        passantsInRange = new List<Passant>();
        this.gameObject.SetActive(false);
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
