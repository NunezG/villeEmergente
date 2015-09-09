﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class GuideActivateScene : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		ai.Body.transform.FindChild("mesh").GetComponent<AnimationManager> ().MarcheFiere ();
		ai.Body.transform.FindChild ("mesh").GetComponent<AudioEventManager> ().soundOrdre ();

        ai.Body.GetComponent<Guide>().scene.gameObject.SetActive(true);
        ai.WorkingMemory.SetItem<bool>("followMe", true);
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}