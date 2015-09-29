using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class StartDance : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }
    // Action RAIN de début de la danse pour les passants
    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        Debug.Log(ai.Body.name + " : Passant dance !");

        ai.WorkingMemory.SetItem<bool>("isDancing", true);

		ai.Body.transform.FindChild("mesh").GetComponent<AnimationManager> ().Danse ();
		ai.Body.transform.FindChild ("mesh").GetComponent<AudioEventManager> ().soundDanse ();

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}