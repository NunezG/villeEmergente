using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Representation;

[RAINAction]
public class BetterTimer : RAINAction
{
    public float timeToWait;
    public System.DateTime startingTime, currentTime;
    public Expression minRangeExp = new Expression();
    public Expression maxRangeExp = new Expression();


    public override void Start(RAIN.Core.AI ai)
    {
        startingTime = System.DateTime.Now;

        if ((minRangeExp != null) && (minRangeExp.IsValid) && (maxRangeExp != null) && (maxRangeExp.IsValid))
        {
            float minRange = minRangeExp.Evaluate<float>(ai.DeltaTime, ai.WorkingMemory);
            float maxRange = maxRangeExp.Evaluate<float>(ai.DeltaTime, ai.WorkingMemory);
            timeToWait = Random.Range(minRange, maxRange);
        }



        Debug.Log("Start Timer");

        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        currentTime = System.DateTime.Now;
        //Debug.Log("TotalSeconds : "+(currentTime - startingTime).TotalSeconds);
        if ((currentTime - startingTime).TotalSeconds < timeToWait)
        {
            return ActionResult.RUNNING;
        }
        else
        {
            Debug.Log("Timer Success");
            return ActionResult.SUCCESS;
        }
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}