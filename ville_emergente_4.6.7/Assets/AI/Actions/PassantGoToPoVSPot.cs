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
        Debug.Log(ai.Body.name + " : PassantGoToPoVSpot");
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        NavMeshAgent agent = ai.Body.GetComponent<NavMeshAgent>();
        GameObject povSpot = ai.Body.GetComponent<Passant>().sceneLeader.GetComponent<Guide>().pdv.spots[ai.Body.GetComponent<Passant>().selectedSpotIndex];
        agent.SetDestination(povSpot.transform.position);

        if (ai.Body.transform.position.x == povSpot.transform.position.x
                && ai.Body.transform.position.z == povSpot.transform.position.z)
        {

            ai.WorkingMemory.SetItem<bool>("isOnPovSpot", true);
        }

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}