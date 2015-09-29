using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class LookForFreeSpot : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        //Debug.Log(ai.Body.name + " : LookForFreeSpot  ");
    }
    // Action RAIN pour tester si il y a une place de libre dans une des scènes à portée du passant
    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        if (ai.Body.GetComponent<Passant>().IsThereAtLeastOneFreeSpot())
        {
            return ActionResult.SUCCESS;
        }
        else
        {
            return ActionResult.FAILURE;
        }
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}