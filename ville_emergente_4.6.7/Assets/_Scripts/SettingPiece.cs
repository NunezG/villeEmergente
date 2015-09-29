using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SettingPiece : MonoBehaviour{
    
    // Use this for initialization
    public void Awake()
    {
        this.tag = "SettingPiece";
    }
    public void Start()
	{
	}

	public GameObject OnTouch()
	{		
		return GetComponent<ConvolutionObject> ().OnTouch ();		
	}

	// Update is called once per frame
    public void Update()
    {
	}


}
