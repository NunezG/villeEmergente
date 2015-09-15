using UnityEngine;
using System.Collections;

public class Plateforme_Move : MonoBehaviour 
{
    public Vector3 min;
    public Vector3 max;
   
    public float angle_min;
    public float angle_max;
    
    public float time;
    public float sleep;

    public float ratio = 0.0f;

    private bool fall = true;
    private bool stop = false;

    private Vector3 currentAngle;
    private float timer = 0.0f;

  

	// Use this for initialization
	void Start () 
    {
        if (ratio > 1.0f)
            ratio = 1.0f;
        if (ratio < 0.0f)
            ratio = 0.0f;

        currentAngle = transform.localEulerAngles;
	}

	// Update is called once per frame
	void Update () 
    {
        if (stop)
        {
            timer += Time.deltaTime;

            if(timer >= sleep)
            {
                timer = 0.0f;
                stop = false;
            }
        }

        else
        {
            if (fall)
            {
                ratio += Time.deltaTime / time;

                if (ratio >= 1.0f)
                {
                    ratio = 1.0f;
                    fall = false;
                    stop = true;
                }
            }

            else
            {
                ratio -= Time.deltaTime / time;

                if (ratio <= 0.0f)
                {
                    ratio = 0.0f;
                    fall = true;
                    stop = true;
                }
            }

        }

        transform.position = min + ratio * (max - min);
        transform.localEulerAngles = new Vector3(currentAngle.x, angle_min + ratio * (angle_max - angle_min), currentAngle.z);
	}
}
