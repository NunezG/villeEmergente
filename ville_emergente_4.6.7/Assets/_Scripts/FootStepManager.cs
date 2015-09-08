using UnityEngine;
using System.Collections;

public class FootStepManager : MonoBehaviour {

    CharacterController controller;
    public bool startWalking=false,isWalking=false, stoppedWalking=false;
    public float isWalkingThreshold = 1;
	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
        if (controller.isGrounded && controller.velocity.magnitude > isWalkingThreshold && !isWalking) // si le joueur est sur le sol et a une vitesse supérieur au seuil
        {
            isWalking = true; // alors c'est qu'il marche
            stoppedWalking = false;
            startWalking = true;
        }
        else if (isWalking && (!controller.isGrounded || controller.velocity.magnitude < isWalkingThreshold)) // si le joueur était en train de marcher et qu'il n'est pas au sol, ou a une vitesse inférieur au seuil
        {
            isWalking=false;
            stoppedWalking = true; // alors c'est qu'il s'est arrêté de marcher
        }
        if (startWalking)
        {
            //print("start walking");
            WwiseAudioManager.PlayLoopEvent("footsteps", this.gameObject);
            startWalking = false;
        }
        if(stoppedWalking)
        {
            stoppedWalking = false;
            //print("stopped walking");
            WwiseAudioManager.StopLoopEvent("footsteps", this.gameObject);
        }
	}
}
