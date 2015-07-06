using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractibleObject : MonoBehaviour {


    public InteractibleType type = InteractibleType.SettingPiece; // par defaut un element du decor
    public AudioSource audioSource; // placeholder
    public Material material; //placeholder

    public List<InteractibleObject> fragments = new List<InteractibleObject>();

    public float timer = 0, maxTimer=15,endTimer;


	// Use this for initialization
	void Start () {

        this.gameObject.layer = 8; // mis dans la layer InteractibleObject pour limiter les raycast avec un layermask
        endTimer =(int) Random.Range(0, 15);
	}
	
	// Update is called once per frame
	void Update () {
        switch (type)
        {
            case InteractibleType.Fragment:
                break;
            case InteractibleType.NPC:
                if (timer < endTimer)
                {
                    timer = timer + Time.deltaTime;
                }
                else
                {
                    timer = 0;
                    endTimer = (int)Random.Range(0, 15);
                    //audioSource.Play();
                }
                break;
            case InteractibleType.SettingPiece:
                break;
        }
	
	}


    public void OnPickUp()
    {
        //audioSource.Play();
    }

    public void OnInteract()
    {

    }


    public void OnAddingFragment(GameObject fragment)
    {
        fragments.Add(fragment.GetComponent<InteractibleObject>());
    }

}
