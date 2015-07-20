using UnityEngine;
using System.Collections;

public class FootStepManager : MonoBehaviour {

    public CharacterController controller;
    public bool isWalking=false, stoppedWalking=false;
    public float isWalkingThreshold = 1;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (controller.isGrounded && controller.velocity.magnitude > isWalkingThreshold) // si le joueur est sur le sol et a une vitesse supérieur au seuil
        {
            isWalking = true; // alors c'est qu'il marche
        }
        else if (isWalking && (!controller.isGrounded || controller.velocity.magnitude < isWalkingThreshold)) // si le joueur était en train de marcher et qu'il n'est pas au sol, ou a une vitesse inférieur au seuil
        {
            isWalking=false;
            stoppedWalking = true; // alors c'est qu'il s'est arrêté de marcher
        }

        if (isWalking)
        {
            WwiseAudioManager.instance.PlayLoopEvent("footsteps", this.gameObject);
        }
        if(stoppedWalking)
        {
            WwiseAudioManager.instance.StopLoopEvent("footsteps", this.gameObject);
        }
	}
}
