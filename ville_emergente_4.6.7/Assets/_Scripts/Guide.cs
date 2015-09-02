using UnityEngine;
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

    public PointDeVue pdv;
    public Fragment fragment;
    public Material defaultMaterial, material;
    public SceneRange scene;
    public RAIN.Memory.BasicMemory tMemory;

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
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void InteractWithBuilding()
    {
		if (pdv.batimentAVisiter.GetComponent<ConvolutionObject>().fragment == null)
        {
			pdv.batimentAVisiter.GetComponent<ConvolutionObject>().OnAddingFragment(fragment);
            tMemory.SetItem<bool>("hasFragment", false);
        }
        else
        {
            fragment = pdv.batimentAVisiter.OnTouch().GetComponent<Fragment>();
            tMemory.SetItem<bool>("hasFragment", true);
        }
    }

    public void EmitSound()
    {

    }

}
