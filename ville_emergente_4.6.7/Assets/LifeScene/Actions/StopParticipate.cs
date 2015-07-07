using UnityEngine;
using System.Collections;
using System;
using mlv;

// declare a class for holding the goto action parameters
// which is used in the BT
// deriving from StaticObject takes care of automatic registration
// the name of the member should match the name of the action parameters in the BT
public class StopParticipateParam
	: StaticObject
{
	//public float orientation;
	//public float threshold = 30f;
}

public class StopParticipate : mlv.Action<StopParticipateParam, StaticObject>
{

	void Reset()
	{
		actionName = "stopParticipate";
	}

    public override Status start(EntityKnowledgeFacade entity, UInt32 request, StopParticipateParam parameters, StaticObject paramsOut)
	{
		// just call the update function as all the code is in it
        return update(entity, request, parameters, paramsOut, 0f);
	}

    public override Status update(EntityKnowledgeFacade entity, UInt32 request, StopParticipateParam parameters, StaticObject paramsOut, float dt)
	{

        if(entity.getKnowledge().getInt("Events.inEvent") == 0)
            entity.getKnowledge().setInt("Events.inEvent", 1);
        if (entity.getKnowledge().getInt("IP.inIP") == 0)
            entity.getKnowledge().setInt("IP.inIP", 1);
        return Status.succeeded;
	}
	
	// called when the action is cancelled
	// nothing special to do
    public override Status cancel(EntityKnowledgeFacade entity, UInt32 request, StopParticipateParam parameters, float dt)
	{
		if( entity.entity == null )
			return Status.canceled;
		return Status.canceled;		
	}

}
