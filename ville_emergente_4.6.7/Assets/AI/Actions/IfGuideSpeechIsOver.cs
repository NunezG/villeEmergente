﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class IfGuideSpeechIsOver : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        Debug.Log(ai.Body.name + " : ifGuide  ");
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        if (ai.Body.GetComponent<Passant>().sceneLeader.GetComponent<Guide>().tMemory.GetItem<bool>("speechIsOver"))
            return ActionResult.SUCCESS;
        else
            return ActionResult.FAILURE;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}