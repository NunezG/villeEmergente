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

    public override ActionResult Execute(RAIN.Core.AI ai)
    {

		NavMeshAgent Agent = ai.Body.GetComponent<NavMeshAgent>();
			
		StopMoving(Agent);
		//ai.WorkingMemory.SetItem<bool> ("OnMyWay", false);
        return ActionResult.SUCCESS;
    }


	
	
	public void StopMoving(NavMeshAgent p_agent)
	{			
		if (p_agent.enabled)
		{
			p_agent.Stop();
			p_agent.ResetPath();
		}
	}


    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}