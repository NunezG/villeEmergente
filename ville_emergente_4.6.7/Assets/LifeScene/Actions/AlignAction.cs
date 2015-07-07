using UnityEngine;
using System.Collections;
using System;
using mlv;

// declare a class for holding the goto action parameters
// which is used in the BT
// deriving from StaticObject takes care of automatic registration
// the name of the member should match the name of the action parameters in the BT
public class AlignParam
	: StaticObject
{
	public float orientation;
	public float threshold = 30f;
}

public class AlignAction : mlv.Action<AlignParam, StaticObject>
{

	void Reset()
	{
		actionName = "alignOrientation";
	}

    public override Status start(EntityKnowledgeFacade entity, UInt32 request, AlignParam parameters, StaticObject paramsOut)
	{
		// just call the update function as all the code is in it
		entity.entity.SendMessage("AlignStart", parameters.orientation, SendMessageOptions.DontRequireReceiver);
        return update(entity, request, parameters, paramsOut, 0f);
	}

    public override Status update(EntityKnowledgeFacade entity, UInt32 request, AlignParam parameters, StaticObject paramsOut, float dt)
	{ 
	
		Quaternion actual = entity.entity.transform.rotation;
		float dif = Mathf.Abs( Mathf.DeltaAngle( actual.eulerAngles.y , parameters.orientation) );
		if( dif <= parameters.threshold)
		{
			entity.entity.SendMessage("AlignStop", SendMessageOptions.DontRequireReceiver);
			return Status.succeeded;
		}
		entity.entity.transform.rotation = Quaternion.Lerp(actual, Quaternion.Euler(0, parameters.orientation, 0), 2f*dt);
		return Status.running;
	}
	
	// called when the action is cancelled
	// nothing special to do
    public override Status cancel(EntityKnowledgeFacade entity, UInt32 request, AlignParam parameters, float dt)
	{
		if( entity.entity == null )
			return Status.canceled;
		entity.entity.SendMessage("AlignStop", SendMessageOptions.DontRequireReceiver);		
		return Status.canceled;		
	}

}
