using UnityEngine;
using System.Collections;
using System;
using mlv;

// declare a class for holding the face action parameters
// which is used in the BT
// deriving from StaticObject takes care of automatic registration
// the name of the member should match the name of the action parameters in the BT
public class FaceEntityParam
	: StaticObject
{
	public string targetEntity = "none";
	public int targetID = -1;
	public Vector3 pos = Vector3.zero;
}

// the face action implementation
// it will  (180° per seconds) turn the transform of the entity toward the position
// when facing, the action succeed
// until then, the action is running
public class FaceEntityAction : mlv.Action<FaceEntityParam, StaticObject>
{
	
	void Reset()
	{
		actionName = "faceEntity";
	}	

	private Vector3 position;
	private uint index;
	private string Aname;
	private int nbrEntity;

    public override Status start(EntityKnowledgeFacade entity, UInt32 request, FaceEntityParam parameters, StaticObject paramsOut)
	{
		// just call the update function as all the code is in it
        GameObject[] charact = GameObject.FindGameObjectsWithTag("Character");
        nbrEntity = charact.Length;
        return update(entity, request, parameters, paramsOut, 0f);
	}

    public override Status update(EntityKnowledgeFacade entity, UInt32 request, FaceEntityParam parameters, StaticObject paramsOut, float dt)
	{
		if(parameters.targetEntity != "none"){
			for(uint i = 0; i < nbrEntity; i++){
				if(i != entity.entity.entityID){
					Aname = entity.getEntityKnowledge(i).getString("name");
					if(Aname == parameters.targetEntity){
						entity.getEntityKnowledge(i).retrieve(entity.getEntityKnowledge(i).getProperty("position"), out position);
					}
				}
			}
		}
		
		if(parameters.targetID != -1){
			index = (uint) parameters.targetID;
			entity.getEntityKnowledge(index).retrieve(entity.getEntityKnowledge(index).getProperty("position"), out position);
		}

		if(parameters.pos != Vector3.zero){
			position = parameters.pos;
		}

		// get the transform of the entity asked to face 
		Transform self = entity.entity.transform;
		// create a quaternion to look along the direction of the targetPosition
		Quaternion lookAt = Quaternion.LookRotation( (position - self.position).normalized );
		// slowly rotate toward it
		self.rotation = Quaternion.RotateTowards( self.rotation, lookAt, 180f*dt );
		
		// if almost facing ...
		if( RotationEquals( self.rotation, lookAt ) )
		{
			// then it is time to end the facing
			return Status.succeeded;
		}
		// else we will continue rotating
		return Status.running; 
	}
	
	// called when the action is cancelled
	// nothing special to do
	public override Status cancel( EntityKnowledgeFacade entity, UInt32 request, FaceEntityParam parameters, float dt )
	{
		return Status.canceled;
	}
	
	// internal helper to check that 2 quaternions are almost equals ( modulo threshold )
	private static bool RotationEquals(Quaternion r1, Quaternion r2, float threshold = .99f )
	{
		float abs = Mathf.Abs(Quaternion.Dot(r1, r2));
		if (abs >= threshold)
			return true;
		return false;
	}
	
}
