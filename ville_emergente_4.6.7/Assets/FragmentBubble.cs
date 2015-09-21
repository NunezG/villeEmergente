using UnityEngine;
using System.Collections;

public class FragmentBubble : MonoBehaviour {

    public Sprite[] sprites;
    public SpriteRenderer spriteRenderer;
    public float timer = 0, fadeInTile = 2,waitTime=2,fadeOutTime=2;
    public float minimum = 0.0f;
    public float maximum = 1f;
    public float duration = 2.0f;
    private float startTime;
    public bool fadeIn =false,wait=false, fadeOut=false;
	// Use this for initialization
	void Start () {

        spriteRenderer.color = new Color(1, 1, 1, 0);
	}
	
	// Update is called once per frame
	void Update () {
        if (fadeIn)
        {
            timer = timer + Time.deltaTime;
            if (timer < fadeInTile)
            {
                float fadeValue = 0 + (timer / fadeInTile);
                spriteRenderer.color = new Color(1, 1, 1, fadeValue);
            }
            else
            {
                fadeIn = false;
                wait = true;
                timer = 0;
            }
        }
        if (wait)
        {
            timer = timer + Time.deltaTime;
            if (timer > waitTime)
            {
                wait = false;
                timer = 0;
                fadeOut = true;

            }

        }
        if (fadeOut)
        {
            timer = timer + Time.deltaTime;
            if (timer < fadeOutTime)
            {
                float fadeValue = 1 - (timer / fadeOutTime);
                spriteRenderer.color = new Color(1, 1, 1, fadeValue);
            }
            else
            {
                fadeOut = false;
                timer = 0;
            }
        }

        /*if (Input.GetKeyDown("space"))
        {
            BubblingIn();
        }*/
	
	}

    public void BubblingIn() {
        int rdmIndex = Random.Range(0, sprites.Length);
        spriteRenderer.sprite = sprites[rdmIndex];
        timer = 0;
        fadeIn = true;
    }
}
