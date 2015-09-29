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
		NavMeshAgent agent = ai.Body.GetComponent<NavMeshAgent>();

        //Debug.Log("MOVE TO");
        if (ai.WorkingMemory.GetItem<GameObject>("target") == null)
        {
            return ActionResult.FAILURE;
        }
        else if (!ai.WorkingMemory.GetItem<bool>("moving") && 
            !ai.WorkingMemory.GetItem<bool>("destinationReached"))// si on est pas déjà en mouvement, et qu'on a pas atteint notre destination
        {
            // on se met en marche
            ai.WorkingMemory.SetItem<bool>("moving", true);
            agent.SetDestination(ai.WorkingMemory.GetItem<GameObject>("target").transform.position);
            return ActionResult.SUCCESS;
        }
        else if (ai.Body.transform.position.x == ai.WorkingMemory.GetItem<GameObject>("target").transform.position.x
                && ai.Body.transform.position.z == ai.WorkingMemory.GetItem<GameObject>("target").transform.position.z)
        {// si on atteint la destination
            Vector3 direction = (ai.WorkingMemory.GetItem<GameObject>("target").transform.position - ai.Body.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            ai.Body.transform.rotation = Quaternion.Slerp(ai.Body.transform.rotation, lookRotation, Time.deltaTime * 5);
            ai.WorkingMemory.SetItem<bool>("moving", false);
            ai.WorkingMemory.SetItem<bool>("destinationReached", true);
            ai.WorkingMemory.SetItem<GameObject>("target", null);
            return ActionResult.SUCCESS;
        }
        else { 
            return ActionResult.SUCCESS; 
        }

    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }

}