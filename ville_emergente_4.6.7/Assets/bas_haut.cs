using UnityEngine;
using System.Collections;

public class bas_haut : MonoBehaviour {

    public float min;
    public float max;

    public float time;

    private Vector3 pos;

    private bool fall = false;

    

	// Use this for initialization
	void Start () {

        pos = transform.position;
        pos.y = min;
	
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
