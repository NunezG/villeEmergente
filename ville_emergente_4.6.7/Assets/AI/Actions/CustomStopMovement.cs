using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class CustomStopMovement : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }
    //action RAIN pour arrêter le déplacement d'un PNJ
    public override ActionResult Execute(RAIN.Core.AI ai)
    {		
		NavMeshAgent agent = ai.Body.GetComponent<NavMeshAgent>();

        //Debug.Log("STOP");
		if (agent.enabled)
		{
			agent.Stop();
			agent.ResetPath();
		}

		ai.WorkingMemory.SetItem<bool> ("moving", false);
        return ActionResult.SUCCESS;
    }
	


    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}