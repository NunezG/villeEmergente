using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class IfGuideIsOnPoV : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        if (ai.Body.GetComponent<Passant>().sceneLeader.GetComponent<Guide>().tMemory.GetItem<bool>("destinationReached"))
        {
			ai.Body.transform.FindChild("mesh").GetComponent<AnimationManager> ().Satisfait();
			ai.Body.transform.FindChild("mesh").GetComponent<AudioEventManager> ().soundSon();


            ai.WorkingMemory.SetItem<bool>("guideIsOnPov", true);
            return ActionResult.SUCCESS;
        }
        else
            return ActionResult.FAILURE;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}