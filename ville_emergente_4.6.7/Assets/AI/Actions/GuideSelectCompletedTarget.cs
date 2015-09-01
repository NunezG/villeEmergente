using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class GuideSelectCompletedTarget : RAINAction
{
    GameObject target;
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        target = ai.WorkingMemory.GetItem<GameObject>("target");
        Debug.Log(ai.Body.name + " : SELECT TARGET ");


    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        List<GameObject> completedTargets = new List<GameObject>();
        for (int i = 0; i < Guide.allPointsOfView.Length; i++)
        {
            if (Guide.allPointsOfView[i].GetComponent<PointDeVue>().batimentAVisiter.fragment != null)
            {
                completedTargets.Add(Guide.allPointsOfView[i]); // on remplit la liste de tous les points de vue liés à des bâtiments complétés
            }
        }
        int targetIndex = Random.Range(0, completedTargets.Count); // on en prend un au hasard
        target = completedTargets[targetIndex];
        ai.WorkingMemory.SetItem<bool>("destinationReached", false);
        ai.WorkingMemory.SetItem<GameObject>("target", target);
        ai.Body.GetComponent<Guide>().pdv = ai.WorkingMemory.GetItem<GameObject>("target").GetComponent<PointDeVue>();
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}