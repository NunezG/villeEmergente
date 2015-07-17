using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class ColorChange : RAINAction
{
	float R;
	float G;
	float B;

    public override void Start(RAIN.Core.AI ai)
    {
		R = Random.Range (0.0f, 1.0f);
		G = Random.Range (0.0f, 1.0f);
		B = Random.Range (0.0f, 1.0f);
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {	
	
		if (ai.Body.renderer.material.color != new Color (R, G, B)) {
			ai.Body.renderer.material.color = Color.Lerp (ai.Body.renderer.material.color, new Color (R, G, B), 0.05f);
			return ActionResult.RUNNING;

		}
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}