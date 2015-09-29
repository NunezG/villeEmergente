using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class PassantLeaveScene : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        Debug.Log(ai.Body.name + " : PassantLeaveScene");
    }
    // action RAIN correspondant  la sortie d'une scène des passants 
    public override ActionResult Execute(RAIN.Core.AI ai)
    {

        ai.Body.transform.FindChild("mesh").GetComponent<AnimationManager>().Marche(); // retour à l'animation de marche
        ai.Body.GetComponent<Passant>().LeaveSelectedScene(); // et sortie de la scène
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}