using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;


[RAINAction]
public class CustomMoveTo : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		ai.Body.GetComponent<NavMeshAgent> ().SetDestination(ai.WorkingMemory.GetItem<GameObject> ("target").transform.position);
		//ai.Body.GetComponent<NavMeshAgent> ().Resume ();
		//ai.WorkingMemory.SetItem<bool> ("OnMyWay", true);
		
		if (ai.Body.transform.position.x == ai.WorkingMemory.GetItem<GameObject> ("target").transform.position.x
			&& ai.Body.transform.position.z == ai.WorkingMemory.GetItem<GameObject> ("target").transform.position.z) {
				
			//private void RotateTowards (Transform target) {
			Vector3 direction = (ai.WorkingMemory.GetItem<GameObject> ("target").transform.position - ai.Body.transform.position).normalized;
			Quaternion lookRotation = Quaternion.LookRotation(direction);
			ai.Body.transform.rotation = Quaternion.Slerp(ai.Body.transform.rotation, lookRotation, Time.deltaTime * 5);
			//}
			return ActionResult.SUCCESS;

		}
		
        return ActionResult.RUNNING;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }

	/*
    private bool isArrived(Vector3 p_currentPosition, NavMeshAgent p_Agent)
    {
		Vector3 target = ai.Body.WorkingMemory.GetItem<GameObject> ("target");
        p_currentPosition.y = 0f;
        target.y = 0f;

        if (p_Agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            if (Vector3.Distance(p_currentPosition, p_Agent.pathEndPosition) < 0.1f)
            {
                if (Vector3.Distance(p_currentPosition, target) > 4)
                {
                    Debug.LogError(gameObject.name + " Problem goto path not close at all " + p_currentPosition + " " + target);
                    p_Agent.enabled = false;
                    return false;
                }
                return true;
            }
            return false;
        }

		else if (Vector3.Distance(p_currentPosition, target) < 0.1f)
            return true;
        return false;
    }
*/
}