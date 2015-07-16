using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class BetterTimer : RAINAction
{
    public float timeToWait;
    public System.DateTime startingTime, currentTime;

    public override void Start(RAIN.Core.AI ai)
    {
        startingTime = System.DateTime.Now;
        timeToWait = Random.Range(5, 10);

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