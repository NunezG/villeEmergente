using UnityEngine;
using System.Collections;
using System;
using mlv;

public class DisplayBubbleParam:StaticObject{
	public string BubbleText;
}

public class DisplayBubbleAction : mlv.Action<DisplayBubbleParam, StaticObject>{
	
	void Reset()
	{
		//Used to automatically rename the action
		actionName = "DisplayBubble";
	}
	
	public override Status start(EntityKnowledgeFacade entity, UInt32 request, DisplayBubbleParam parameters, StaticObject paramsOut)
	{
		SpeechBubble bubble = entity.entity.GetComponent<SpeechBubble>();
		bubble.bubbleText = parameters.BubbleText!="none"?parameters.BubbleText:"";
		return Status.succeeded;
	}
	
	public override Status update(EntityKnowledgeFacade entity, UInt32 request, DisplayBubbleParam parameters, StaticObject paramsOut, float dt)
	{
		return Status.failed;
	}
	
	public override Status cancel(EntityKnowledgeFacade entity, UInt32 request, DisplayBubbleParam parameters, float dt)
	{
		return Status.canceled;
	}	
}

