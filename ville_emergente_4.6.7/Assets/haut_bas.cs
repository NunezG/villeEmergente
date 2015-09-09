using UnityEngine;
using System.Collections;

public class haut_bas : MonoBehaviour {

    public float min;
    public float max;

    public float time;

    private Vector3 pos;

    private bool fall = true;

    

	// Use this for initialization
	void Start () {

        pos = transform.position;
        pos.y = max;
	
	}
	
	// Update is called once per frame
	void Update () {

        if (fall)
        {
            pos.y -= (max - min) * Time.deltaTime / time;

            if (pos.y <= min)
                fall = false;
        }

        else
        {
            pos.y += (max - min) *  Time.deltaTime / time;

            if (pos.y >= max)
                fall = true;
        }

        transform.position = pos;
	}

}
