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


		if (ai.Body.transform.position.x != ai.WorkingMemory.GetItem<GameObject> ("target").transform.position.x
		    || ai.Body.transform.position.z != ai.WorkingMemory.GetItem<GameObject> ("target").transform.position.z)
			return ActionResult.RUNNING;
		
		//private void RotateTowards (Transform target) {
		Vector3 direction = (ai.WorkingMemory.GetItem<GameObject> ("target").transform.position - ai.Body.transform.position).normalized;
			Quaternion lookRotation = Quaternion.LookRotation(direction);
		ai.Body.transform.rotation = Quaternion.Slerp(ai.Body.transform.rotation, lookRotation, Time.deltaTime * 5);
		//}

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }

}