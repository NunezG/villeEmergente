using UnityEngine;
using System.Collections;

public class PassantDetectPlayer : MonoBehaviour
{

    public Passant npc;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        //print("SOnTriggerEnterOnTriggerEnterOnTriggerEnter"+other.name);

        if (other.tag == "Player")
        {
            npc.SetPlayerIsInRange(true);
            npc.SetTargetLookAt(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //print("exit");
            npc.SetPlayerIsInRange(false);
        }
    }

}
