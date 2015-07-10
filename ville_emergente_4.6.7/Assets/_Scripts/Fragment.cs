using UnityEngine;
using System.Collections;

public class Fragment : MonoBehaviour{


    public Material material;
    public  AudioSource audioSource;
    public AudioClip defaultClip;
	// Use this for initialization
    public void Awake()
    {
        this.tag = "Fragment";
    }

    public void Start()
    {
        print("FragmentStart");
        audioSource.clip = defaultClip;
        this.renderer.material = material;
	}
	
	// Update is called once per frame
    public void Update()
    {
	
	}

    public void Play()
    {
        audioSource.Play();
    }

    public AudioClip GetClip()
    {
        return audioSource.clip;
    }

    public void AnswerTheCall()
    {
        print("Fragment:AnswerTheCall");
        audioSource.Play();
    }

    public GameObject OnPickUp()
    {
        print("Fragment:OnPickUp");
        this.gameObject.SetActive(true);
        audioSource.Play();
        return this.gameObject;
    }

}
