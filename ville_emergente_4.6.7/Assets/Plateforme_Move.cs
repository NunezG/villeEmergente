using UnityEngine;
using System.Collections;

public class Plateforme_Move : MonoBehaviour 
{
    public Vector3 min;
    public Vector3 max;

    public float time;
    
    private Vector3 dir;

    private bool fall = true;
    public float ratio = 0.0f;

	// Use this for initialization
	void Start () 
    {
        if (ratio > 1.0f)
            ratio = 1.0f;
        if (ratio < 0.0f)
            ratio = 0.0f;

        dir = (max - min) * 1.0f/time;
	}
	
	// Update is called once per frame
	void Update () 
    {
       
        if(fall)
        {
            ratio += Time.deltaTime / time;

            if (ratio >= 1.0f)
                fall = false;
        }
       
        else
        {
            ratio -= Time.deltaTime / time;

            if (ratio <= 0.0f)
                fall = true;
        }

        transform.position = min + ratio * (max - min); ;

	}
}
