using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SettingPiece : MonoBehaviour{

    public static List<Fragment> fragmentsOfZeWorld = new List<Fragment>(); // tout les fragments 
    public float fragmentsCallRadius = 25;

    public Material defaultMaterial, activatedMaterial;
    public AudioSource audioSource;
    public AudioClip defaultClip;
    public Fragment fragment;

	// Use this for initialization
    public void Awake()
    {
        this.tag = "SettingPiece";
    }
    public void Start()
    {
        audioSource.clip = defaultClip;
        this.renderer.material = defaultMaterial;
        if (fragmentsOfZeWorld.Count == 0)
        {
            GameObject[] fragsTab = GameObject.FindGameObjectsWithTag("Fragment");
            for (int i = 0; i < fragsTab.Length; i++)
            {
                fragmentsOfZeWorld.Add(fragsTab[i].GetComponent<Fragment>());
            }
        }
	}

	// Update is called once per frame
    public void Update()
    {
        if (hasBeenActivated && !audioSource.isPlaying) // si le decor a ete active, et que son son n'est plus en en train de jouer, 
        {

            print("has been activated and finished playing");
            for (int i = 0; i < fragmentsOfZeWorld.Count; i++)
            {
                float distance = (fragmentsOfZeWorld[i].transform.position - this.transform.position).magnitude;
                if (distance < fragmentsCallRadius)
                {
                    fragmentsOfZeWorld[i].AnswerTheCall(); // appelle le premier fragment a portee
                    break;
                }
            }
            hasBeenActivated = false;
        }
	}

    bool hasBeenActivated = false;
    public void OnInteract()
    {
        print("SettingPiece:OnInteract ");
        audioSource.Play();
        hasBeenActivated = true;

        this.renderer.material = activatedMaterial;
    }

    public void OnAddingFragment(Fragment fragment)
    {
        print("SettingPiece:OnAddingFragment");
        this.fragment = fragment;
        //activatedMaterial = fragment.material;
        this.renderer.material = fragment.material;
        this.audioSource.clip = fragment.GetClip();
    }

    public GameObject OnPickUp()
    {
        GameObject fragPicked =fragment.OnPickUp();
        fragment = null;
        this.renderer.material = activatedMaterial;
        audioSource.clip = defaultClip;
        return fragPicked;
    }
}
