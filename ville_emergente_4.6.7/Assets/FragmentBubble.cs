using UnityEngine;
using System.Collections;

// Classe pour les bulles s'affichant à intervalle régulier au dessus des musiciens non complet
public class FragmentBubble : MonoBehaviour {

    public Sprite[] sprites; // l'ensemble des sprites pouvant être sélectionnés aléatoirement
    public SpriteRenderer spriteRenderer;
    public float timer = 0, fadeInTime = 2,waitTime=2,fadeOutTime=2; // timer et variables de durée du fadeIn,de la transition, et du fadeOut
    public bool fadeIn =false,wait=false, fadeOut=false;

	// Use this for initialization
	void Start () {
        spriteRenderer.color = new Color(1, 1, 1, 0); // sprite complétement transparent initialement
	}
	
	// Update is called once per frame
	void Update () {
        if (fadeIn)
        {
            timer = timer + Time.deltaTime;
            if (timer < fadeInTime)
            {
                float fadeValue = 0 + (timer / fadeInTime);
                spriteRenderer.color = new Color(1, 1, 1, fadeValue);
            }
            else
            {
                spriteRenderer.color = new Color(1, 1, 1, 1);
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
                spriteRenderer.color = new Color(1, 1, 1, 0);
                fadeOut = false;
                timer = 0;
            }
        }	
	}

    public void BubblingIn() { // lance l'enchainement fadeIn/wait/fadeOut
        int rdmIndex = Random.Range(0, sprites.Length);
        spriteRenderer.sprite = sprites[rdmIndex];
        timer = 0;
        fadeIn = true;
    }
}
