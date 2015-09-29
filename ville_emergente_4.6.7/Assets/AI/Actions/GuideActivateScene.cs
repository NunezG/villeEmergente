using UnityEngine;
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
    // action RAIN de réactivation de la scène
    public override ActionResult Execute(RAIN.Core.AI ai)
    {

        //Debug.Log(ai.Body.name + " : follow me");
		ai.Body.transform.FindChild("mesh").GetComponent<AnimationManager> ().MarcheFiere (); // animation
		ai.Body.transform.FindChild ("mesh").GetComponent<AudioEventManager> ().soundOrdre ();// son

        ai.Body.GetComponent<Guide>().scene.gameObject.SetActive(true); //activation de la scène
        ai.WorkingMemory.SetItem<bool>("followMe", true);// booleen pour indiquer aux passants de suivre le guide
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}