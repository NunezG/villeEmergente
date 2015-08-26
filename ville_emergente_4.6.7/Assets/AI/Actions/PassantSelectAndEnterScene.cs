using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class PassantSelectAndEnterScene : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        Debug.Log(ai.Body.name + " : PassantSelectAndEnterScene");
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {

        ai.Body.GetComponent<Passant>().SelectAndEnterScene();
        ai.WorkingMemory.SetItem<bool>("destinationReached", true);
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}