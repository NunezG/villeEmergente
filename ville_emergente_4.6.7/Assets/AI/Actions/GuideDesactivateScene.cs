using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class GuideDesactivateScene : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }
    //action RAIN de désactivation de la scène du guide
    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		ai.Body.transform.FindChild("mesh").GetComponent<AnimationManager> ().Marche (); // reprise de la marche
		ai.Body.transform.FindChild ("mesh").GetComponent<AudioEventManager> ().soundSon ();// émission du son

        //ai.Body.GetComponent<Guide>().scene.gameObject.SetActive(false);
        ai.Body.GetComponent<Guide>().scene.DeactivateScene(); // appel de la fonction du guide de désactivation de la scène
        ai.WorkingMemory.SetItem<bool>("followMe", false);

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}