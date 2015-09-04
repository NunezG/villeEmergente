using UnityEngine;
using System.Collections;

public class Plateforme_Move : MonoBehaviour 
{
    public Vector3 min;
    public Vector3 max;

    public float time;
    
    private Vector3 pos;
    private Vector3 dir;

    private bool fall = false;
    private float currentTime = 0.0f;

	// Use this for initialization
	void Start () 
    {
        pos = transform.position;
        dir = (max - min) * 1.0f/time;
	}
	
	// Update is called once per frame
	void Update () 
    {
       
        if(fall)
        {
            pos = pos - dir * Time.deltaTime;

            currentTime += Time.deltaTime / time;

            if (currentTime >= 1.0f)
                fall = false;
        }
       
        else
        {
            pos = pos + dir * Time.deltaTime;

            currentTime -= Time.deltaTime / time;

            if (currentTime <= 0.0f)
                fall = true;
        }

        transform.position = pos;

	}
}
