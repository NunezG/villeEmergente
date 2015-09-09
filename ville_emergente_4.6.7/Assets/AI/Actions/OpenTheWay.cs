using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class OpenTheWay : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		ai.Body.transform.FindChild("mesh").GetComponent<AnimationManager> ().MontreUne ();
		ai.Body.transform.FindChild("mesh").GetComponent<AudioEventManager> ().soundOuverture ();
			
        ai.Body.GetComponent<Musicien>().OpenTheWay();
        //ai.Body.GetComponent<Musicien>().ActiveScene();
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}