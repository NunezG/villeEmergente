using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;

[RAINAction]
public class AiPrint : RAINAction
{
    public Expression toPrintExp = new Expression();


    public override void Start(RAIN.Core.AI ai)
    {
        if ((toPrintExp != null) && (toPrintExp.IsValid))
        {
            string toPrint = toPrintExp.Evaluate<string>(ai.DeltaTime, ai.WorkingMemory);
            Debug.Log("print : " + toPrint+"");
        }




        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {

            return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}