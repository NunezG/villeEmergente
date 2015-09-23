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
        //Debug.Log(ai.Body.name + " : SELECT TARGET ");


    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        List<GameObject> completedTargets = new List<GameObject>();
        for (int i = 0; i < ai.Body.GetComponent<Guide>().targets.Count; i++)
        {
            if (ai.Body.GetComponent<Guide>().targets[i].GetComponent<PointDeVue>().batimentAVisiter.GetComponent<ConvolutionObject>().fragment != null // si le batiment a un fragment
                && ai.Body.GetComponent<Guide>().targets[i].GetComponent<PointDeVue>().isBeingVisited == false) // et qu'un autre guide n'est pas en train de le visiter
            {
                completedTargets.Add(ai.Body.GetComponent<Guide>().targets[i]); // on l'ajoute à la liste
            }
        }
        if (ai.Body.GetComponent<Guide>().pdv != null)// si on était sur un point de vue
            ai.Body.GetComponent<Guide>().pdv.isBeingVisited = false; // on le libere 

        int targetIndex = Random.Range(0, completedTargets.Count); //puis on en prend un au hasard parmi ceux selectionnés

        target = completedTargets[targetIndex]; // on l'ajoute en tant que destination

        target.GetComponent<PointDeVue>().isBeingVisited = true; // on le réserve 

        ai.WorkingMemory.SetItem<bool>("destinationReached", false); // on débloque la branche mouvement dans le BT
        ai.WorkingMemory.SetItem<GameObject>("target", target);
        ai.Body.GetComponent<Guide>().pdv = ai.WorkingMemory.GetItem<GameObject>("target").GetComponent<PointDeVue>(); // on assigne le nouveau point de vue au guide
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}