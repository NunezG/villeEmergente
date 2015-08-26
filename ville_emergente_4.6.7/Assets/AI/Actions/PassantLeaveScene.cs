﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class PassantLeaveScene : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        Debug.Log(ai.Body.name + " : PassantLeaveScene");
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {

        ai.Body.GetComponent<Passant>().LeaveSelectedScene();
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}