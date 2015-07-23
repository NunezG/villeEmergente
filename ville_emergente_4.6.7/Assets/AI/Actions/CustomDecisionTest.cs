using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINDecision]
public class CustomDecisionTest : RAINDecision
{
    private int _lastRunning = 0;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);

        _lastRunning = 0;
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        ActionResult tResult = ActionResult.SUCCESS;

		Debug.Log ("EXECUTE CUSTOM DECISION: "+ _lastRunning);


		if (!ai.WorkingMemory.GetItem<bool> ("OnMyWay")) {


			for (; _lastRunning < _children.Count; _lastRunning++) {
				tResult = _children [_lastRunning].Run (ai);
				if (tResult != ActionResult.SUCCESS)
					break;
			}


		}



		Debug.Log ("STATEEE "+ tResult);




        return tResult;
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
		Debug.Log ("END CUSTOM DECISION");
		
		NavMeshAgent Agent = ai.Body.GetComponent<NavMeshAgent>();
		StopMoving(Agent);
		Debug.Log ("STOPPPPPPPPPPPPPP CUSTOM DECISION");
        base.Stop(ai);
    }
}