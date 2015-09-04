using UnityEngine;
using System.Collections;
using System;
using mlv;


public class BubbleInteractionParam 
	: StaticObject
{
	public string text;
	public float threshold = 2f;

	
}

public class BubbleInteractionAction : mlv.Action<BubbleInteractionParam, StaticObject>
{
    private TextMesh tm;
	
	void Reset()
	{
		actionName = "bubbleInteract";
	}


    public override Status start(EntityKnowledgeFacade entity, UInt32 request, BubbleInteractionParam parameters, StaticObject paramsOut)
	{
		mlv.Simulation.instance.knowledgeFramework.getGlobalWorkingKnowledge().setString("LifeScene", "none");
        return update(entity, request, parameters, paramsOut, 0f);
	}

    public override Status update(EntityKnowledgeFacade entity, UInt32 request, BubbleInteractionParam parameters, StaticObject paramsOut, float dt)
	{

        if (entity.entity.GetComponentInChildren<MessageDisplayer>() != null && entity.entity.GetComponentInChildren<MessageDisplayer>().enabled)
            entity.entity.GetComponentInChildren<MessageDisplayer>().SetMessage(parameters.text);
        if (entity.entity.GetComponentInChildren<Message3DDisplayer>() != null && entity.entity.GetComponentInChildren<Message3DDisplayer>().enabled)
        {
            if (parameters.text == "none")
                parameters.text = "";
            tm = entity.entity.GetComponentInChildren<TextMesh>();
            tm.text = parameters.text;
        }
        return Status.succeeded; 
	}
	
	// called when the action is cancelled
	// nothing special to do
	public override Status cancel( EntityKnowledgeFacade entity, UInt32 request, BubbleInteractionParam parameters, float dt )
	{
		if( entity.entity == null )
			return Status.canceled;

        if (entity.entity.GetComponentInChildren<MessageDisplayer>() != null && entity.entity.GetComponentInChildren<MessageDisplayer>().enabled)
            entity.entity.GetComponentInChildren<MessageDisplayer>().SetMessage("none");
        if (entity.entity.GetComponentInChildren<Message3DDisplayer>() != null && entity.entity.GetComponentInChildren<Message3DDisplayer>().enabled)
            entity.entity.GetComponentInChildren<TextMesh>().text = "";
        return Status.canceled;
	}
	
}