using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class PassantSelectAndEnterScene : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        //Debug.Log(ai.Body.name + " : PassantSelectAndEnterScene");
    }
    // Action RAIN pour faire appel à la fonction correspondante du passant
    public override ActionResult Execute(RAIN.Core.AI ai)
    {

        ai.Body.GetComponent<Passant>().SelectAndEnterScene();
        ai.WorkingMemory.SetItem<bool>("destinationReached", true);// si le passant entre dans une scène on considère qu'il a atteint sa destination
        ai.WorkingMemory.SetItem<bool>("moving", false); // booléen moving utilisé pour les mouvements hors scene
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}