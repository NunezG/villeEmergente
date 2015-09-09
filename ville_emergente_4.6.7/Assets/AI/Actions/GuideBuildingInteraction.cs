using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class GuideBuildingInteraction : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        //Debug.Log(ai.Body.name + " : IfMusicien");
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {

		ai.Body.transform.FindChild("mesh").GetComponent<AnimationManager> ().MontreDeux ();
		ai.Body.transform.FindChild ("mesh").GetComponent<AudioEventManager> ().soundNouveauSon ();

        ai.Body.GetComponent<Guide>().InteractWithBuilding();
        ai.WorkingMemory.SetItem<bool>("hasInteracted", true);
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}