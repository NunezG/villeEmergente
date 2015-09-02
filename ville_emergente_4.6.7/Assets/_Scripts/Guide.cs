﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using RAIN.Core;
using RAIN.Navigation.Targets;
using RAIN.Minds;
using RAIN.Serialization;
using RAIN.Motion;

public class Guide : MonoBehaviour
{
    public static GameObject[] allPointsOfView = null;
    public GameObject inHandPosition, inHandObject;

    public GameObject startingFragment;
    public bool handsFull = false;

    public PointDeVue pdv;
    public SceneRange scene;
    public RAIN.Memory.BasicMemory tMemory;

    public float timer = 0, endTimer = 5;

    public void Awake()
    {
        tag = "NPC";
        if (allPointsOfView == null)
            allPointsOfView = GameObject.FindGameObjectsWithTag("PointDeVue");
    }

    // Use this for initialization
    void Start()
    {
        AIRig aiRig = GetComponentInChildren<AIRig>();
        tMemory = aiRig.AI.WorkingMemory as RAIN.Memory.BasicMemory;
        PickUpObject(startingFragment);
    }

    // Update is called once per frame
    void Update()
    {
        if (tMemory.GetItem<bool>("destinationReached"))
        {
            timer = timer + Time.deltaTime;
        }
        else if (timer != 0)
        {
            timer = 0;
        }
        if (timer > endTimer)
        {
            tMemory.SetItem<bool>("timerIsOver", true);
        }



    }

    public void InteractWithBuilding()
    {
        if (pdv.batimentAVisiter.fragment == null)
        {
            Fragment fragment = inHandObject.GetComponent<Fragment>();
            pdv.batimentAVisiter.OnAddingFragment(fragment);
            fragment.gameObject.SetActive(false);
            AddingFragment();
        }
        else
        {
            PickUpObject(pdv.batimentAVisiter.OnPickUp());
        }
    }

    public void EmitSound()
    {

    }

    public void DropInHandObject()
    {
        if (inHandObject != null)
        {
            print("Interactor:DropInHandObject");
            inHandObject.GetComponent<Fragment>().Drop();
            handsFull = false;
            inHandObject = null;
            tMemory.SetItem<bool>("hasFragment", false);
        }
    }

    public void PickUpObject(GameObject toPickUp)
    {
        tMemory.SetItem<bool>("hasFragment", true);
        print("Interactor:PickUpObject");
        inHandObject = toPickUp;
        inHandObject.transform.parent = inHandPosition.transform;
        inHandObject.transform.position = inHandPosition.transform.position;
        inHandObject.transform.rotation = inHandPosition.transform.rotation;
        inHandObject.rigidbody.isKinematic = true;
        handsFull = true;
    }
    public void AddingFragment()
    {
        print("Interactor:AddingFragment");
        inHandObject.rigidbody.isKinematic = false;
        inHandObject.transform.parent = null;
        SettingPiece.fragmentsOfZeWorld.Remove(inHandObject.GetComponent<Fragment>());
        DropInHandObject();
    }
}
