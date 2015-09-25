using UnityEngine;
using System.Collections;

public class Blend : MonoBehaviour 
{
    public float startTime;
    public float endTime;
    public bool inverse = false;

    private MeshRenderer rend;
    private Color[] colors;
    private float currentTime;

	void Start () 
    {
        rend = GetComponent<MeshRenderer>();
        colors = new Color[rend.materials.Length];

        for( int index = 0 ; index < rend.materials.Length ; index++ )
        {
            colors[ index ] = rend.materials[ index ].color;
        }
        
	}
	
	void Update () 
    {
        currentTime = Time.time;

        float ratio = ( currentTime - startTime ) / ( endTime - startTime );

        if( inverse )
            ratio = 1.0f - ratio;

        if( ratio < 0.0f )
            ratio = 0.0f;
        if( ratio > 1.0f )
            ratio = 1.0f;

        for (int index = 0; index < rend.materials.Length; index++)
        {
            Color tmp = colors[index];
            tmp.a *= ratio;
            rend.materials[index].color = tmp;
        }
	}
}
