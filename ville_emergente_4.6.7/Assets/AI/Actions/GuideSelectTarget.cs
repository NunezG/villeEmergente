using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class GuideRandomSelectTarget : RAINAction
{
    GameObject target;
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        target = ai.WorkingMemory.GetItem<GameObject>("target");
        Debug.Log(ai.Body.name + " : SELECT TARGET ");
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
            //Debut de la partie
            int targetIndex = Random.Range(0, Guide.allPointsOfView.Length);
            Debug.Log(ai.Body.name + " : targetIndex : " + targetIndex);
        target = Guide.allPointsOfView[targetIndex];
        ai.WorkingMemory.SetItem<bool>("destinationReached", false);
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        ai.WorkingMemory.SetItem<GameObject>("target", target);
        base.Stop(ai);
    }
}