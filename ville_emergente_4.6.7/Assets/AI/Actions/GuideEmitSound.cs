using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class GuideEmitSound : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        //Debug.Log(ai.Body.name + " : IfMusicien");
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		//ai.Body.transform.FindChild("mesh").GetComponent<AnimationManager> ().CriErrance ();
		//ai.Body.transform.FindChild("mesh").GetComponent<AudioEventManager> ().soundSon ();

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}