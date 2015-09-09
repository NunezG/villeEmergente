using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class MusicianDance : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {

        Debug.Log("Musician dance !");

        ai.WorkingMemory.SetItem<bool>("isDancing", true);
		ai.Body.GetComponent<AnimationManager> ().Danse();
		ai.Body.GetComponent<AudioEventManager> ().soundDanse ();
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}