using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;


[RAINAction]
public class PassantGoToSpot : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        //ai.WorkingMemory.SetItem<bool> ("moving", true);
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        NavMeshAgent agent = ai.Body.GetComponent<NavMeshAgent>();
            agent.SetDestination(ai.WorkingMemory.GetItem<GameObject>("target").transform.position);
            return ActionResult.SUCCESS;

    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}