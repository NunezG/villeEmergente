using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class ProtoSelectTarget : RAINAction
{
    GameObject target;
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
		target = ai.WorkingMemory.GetItem<GameObject> ("target");
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {

		if (target == null) {
			int targetIndex = Random.Range (0, NPC.targets.Length - 1);
			//Debug.Log("targetIndex :" + targetIndex);
			//Debug.Log("NPC.targets.Length :" + NPC.targets.Length);

			target = NPC.targets [targetIndex];

		} else {
			int targetIndex = Random.Range (0, target.GetComponent<navigationScript>().targets.Count - 1);

			target = target.GetComponent<navigationScript>().targets [targetIndex];
			 
		}



        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        ai.WorkingMemory.SetItem<GameObject>("target", target);
        base.Stop(ai);
    }
}