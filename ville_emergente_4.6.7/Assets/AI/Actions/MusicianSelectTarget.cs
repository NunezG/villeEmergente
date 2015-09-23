using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class MusicianSelectTarget : RAINAction
{
    GameObject target;
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        target = ai.WorkingMemory.GetItem<GameObject>("target");
        //Debug.Log(ai.Body.name + " : SELECT TARGET ");

		if ( ai.WorkingMemory.GetItem<bool>("isFragmentComplete"))
			ai.Body.transform.FindChild("mesh").GetComponent<AnimationManager> ().Marche ();
		else 
		ai.Body.transform.FindChild("mesh").GetComponent<AnimationManager> ().Errance();

		//ai.Body.GetComponent<AudioEventManager> ().soundSon ();
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        //Debug.Log ("SELECTTARG");
        if (target == null)
        {
            //Debut de la partie
            int targetIndex = Random.Range(0, ai.Body.GetComponent<Musicien>().targets.Count);
            //Debug.Log(ai.Body.name + " : targetIndex : " + targetIndex);
            target = ai.Body.GetComponent<Musicien>().targets[targetIndex];

        }
        else
        {
            //pourcentage pour retour au target précédent
            float percent = Random.Range(0, 100);

            //choix random du target suivant
            int targetIndex = Random.Range(0, target.GetComponent<navigationScript>().targets.Count);

            //si le target choisi est le précédent, on a un 75% de probabilités de recommencer cette action
            if (ai.Body.GetComponent<Musicien>().previousTarget == target.GetComponent<navigationScript>().targets[targetIndex] && percent < 75)
                return ActionResult.RUNNING;

            //set le target précédent
            ai.Body.GetComponent<Musicien>().previousTarget = target;

            //set le target actuel
            target = target.GetComponent<navigationScript>().targets[targetIndex];
        }
        ai.WorkingMemory.SetItem<bool>("destinationReached", false);
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        ai.WorkingMemory.SetItem<GameObject>("target", target);
        base.Stop(ai);
    }
}