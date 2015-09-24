using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class EmitSound : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		if(ai.Body.transform.GetComponent<Passant>() != null)
		{
			ai.Body.transform.FindChild("mesh").GetComponent<AnimationManager> ().Satisfait();
			ai.Body.transform.FindChild("mesh").GetComponent<AudioEventManager> ().soundSon();
		}

		if(ai.Body.transform.GetComponent<Musicien>() != null)
		{
			
			if (ai.WorkingMemory.GetItem<bool> ("isFragmentComplete")) {
				ai.Body.transform.FindChild ("mesh").GetComponent<AudioEventManager> ().soundDanse ();
				ai.Body.transform.FindChild ("mesh").GetComponent<AnimationManager> ().Sautille ();
			} else {
				ai.Body.transform.FindChild ("mesh").GetComponent<AudioEventManager> ().soundSon();
				ai.Body.transform.FindChild ("mesh").GetComponent<AnimationManager> ().CriErrance ();
				if (ai.Body.GetComponent<Musicien>()!=null)
					ai.Body.GetComponent<Musicien>().EmitSound();
			}

		}

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}