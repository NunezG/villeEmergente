using UnityEngine;
using System.Collections;

public class levitation : MonoBehaviour {

    
    public float speed;
    public float size;
    public float angle;

    private Vector3 pos;
    private Vector3 initial;
    private Vector3 target;
    private Vector3 angle_initial;
    private float angle_current;
    private float angle_target;
    private float ratio = 0.0f;
    private float length;
    private float time;
    private bool hold = false;
    

	// Use this for initialization
	void Start () 
    {
        initial = transform.position;
        pos = initial;
        target = initial + new Vector3(Random.Range(-size, size), Random.Range(-size, size), Random.Range(-size, size));

        angle_initial = transform.localEulerAngles;
        angle_current = angle_initial.y;
        angle_target = angle_initial.y + Random.Range(-angle, angle);

        time = (target - pos).magnitude / speed;
	}

    public void IsHeld( bool state )
    {
        hold = state;

        if(hold == false)
        {
            transform.position += new Vector3(0, size, 0);

            Start();

            ratio = 0.0f;
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (!hold)
        {
            ratio += Time.deltaTime / time;

            if (ratio >= 1.0f)
            {
                ratio = 0.0f;

                pos = target;
                target = initial + new Vector3(Random.Range(-size, size), Random.Range(-size, size), Random.Range(-size, size));

                angle_current = angle_target;
                angle_target = angle_initial.y + Random.Range(-angle, angle);

                time = (target - pos).magnitude / speed;
            }

            transform.position = pos + ratio * (target - pos);
            transform.localEulerAngles = new Vector3(angle_initial.x, angle_current + ratio * (angle_target - angle_current), angle_initial.z);
        }
	}

}
