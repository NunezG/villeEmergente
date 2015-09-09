using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class EmitSound : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		if (ai.WorkingMemory.GetItem<bool> ("isFragmentComplete")) {
			ai.Body.transform.FindChild ("mesh").GetComponent<AudioEventManager> ().soundJoie ();
			ai.Body.transform.FindChild ("mesh").GetComponent<AnimationManager> ().Sautille ();
		} else {
			ai.Body.transform.FindChild ("mesh").GetComponent<AudioEventManager> ().soundSon();
			ai.Body.transform.FindChild ("mesh").GetComponent<AnimationManager> ().CriErrance ();
		}

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}