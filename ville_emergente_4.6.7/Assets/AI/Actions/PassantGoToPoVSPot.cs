using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;


[RAINAction]
public class PassantGoToPoVSpot : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
      //  Debug.Log(ai.Body.name + " : PassantGoToPoVSpot");
    }
    //Action RAIN pour que la passant aille à sa place sur le point de vue
    // 
    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        NavMeshAgent agent = ai.Body.GetComponent<NavMeshAgent>(); 
        if (ai.Body.GetComponent<Passant>().sceneLeader!= null)        {
            //on récupère l'emplacement
            GameObject povSpot = ai.Body.GetComponent<Passant>().sceneLeader.GetComponent<Guide>().pdv.spots[ai.Body.GetComponent<Passant>().selectedSpotIndex];
        
            agent.SetDestination(povSpot.transform.position); // on l'y envoit

            if (ai.Body.transform.position.x == povSpot.transform.position.x
                    && ai.Body.transform.position.z == povSpot.transform.position.z)// et quand il y est
            {

                ai.WorkingMemory.SetItem<bool>("isOnPovSpot", true);// on assigne le booléen correspondant
            }
        }
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}