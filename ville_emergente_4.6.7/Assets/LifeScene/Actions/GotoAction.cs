using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using mlv;

// declare a class for holding the goto action parameters
// which is used in the BT
// deriving from StaticObject takes care of automatic registration
// the name of the member should match the name of the action parameters in the BT
public class GotoParam
	: StaticObject
{
	public Vector3 target;
	public float threshold = .5f;
	public int targetID = -1;
}

public class GotoAction : mlv.Action<GotoParam, StaticObject>
{

	void Reset()
	{
		actionName = "goto";
	}

    public override Status start(EntityKnowledgeFacade entity, UInt32 request, GotoParam parameters, StaticObject paramsOut)
	{
		// just call the update function as all the code is in it
		
		NavMeshAgent Agent = entity.entity.GetComponent<NavMeshAgent>();
		
		if(!Agent.enabled){
			Agent.enabled = true;
		}
		
		Agent.SetDestination(parameters.target);
		Agent.Resume();
		entity.entity.SendMessage("GotoStart", parameters.target, SendMessageOptions.DontRequireReceiver);
		
		return Status.running;
	}

    public override Status update(EntityKnowledgeFacade entity, UInt32 request, GotoParam parameters, StaticObject paramsOut, float dt)
	{
		NavMeshAgent Agent = entity.entity.GetComponent<NavMeshAgent>();
		
		if( isArrived(entity.entity.transform.position, parameters, Agent)  )//|| finished[request] )
		{
            Stop(Agent); 
            entity.entity.SendMessage("GotoStop", SendMessageOptions.DontRequireReceiver);				
			return Status.succeeded;
		}

        /*if (entity.getKnowledge().getInt("Events.inEvent") == 0 && entity.getKnowledge().getBool("Positioning.begin") == false) 
        {
            entity.entity.SendMessage("GotoStop", SendMessageOptions.DontRequireReceiver);
            return Status.succeeded;
        }*/
        
        //if (entity.entity.GetComponentInChildren<MessageDisplayer>() != null)
		//	entity.entity.GetComponentInChildren<MessageDisplayer>().SetMessage("none");

//		Debug.Log ("set dest at update");
		Agent.SetDestination(parameters.target);
		return Status.running;
	}
	
	// called when the action is cancelled
	// nothing special to do
	public override Status cancel( EntityKnowledgeFacade entity, UInt32 request, GotoParam parameters, float dt )
	{
		if (!entity.entity)
		{
			return Status.canceled;
		}

        NavMeshAgent Agent = entity.entity.GetComponent<NavMeshAgent>();
		Stop(Agent);
		entity.entity.SendMessage("GotoStop", SendMessageOptions.DontRequireReceiver);
		return Status.canceled;
		
	}
	
	public void Stop(NavMeshAgent p_agent)
	{			
		if (p_agent.enabled)
		{
			p_agent.Stop();
			p_agent.ResetPath();
		}
	}
	
	private bool isArrived(Vector3 p_currentPosition, GotoParam p_parameters, NavMeshAgent p_Agent){
		Vector3 target = p_parameters.target;
		p_currentPosition.y = 0f;
		target.y = 0f; 

		if( p_Agent.pathStatus == NavMeshPathStatus.PathPartial )
		{
			if( Vector3.Distance(p_currentPosition, p_Agent.pathEndPosition) < p_parameters.threshold) 
			{
				if( Vector3.Distance(p_currentPosition, target) > 4 )
				{
					Debug.LogError( gameObject.name + " Problem goto path not close at all "+ p_currentPosition + " "+target );
					p_Agent.enabled = false;
					return false;
				}
				return true;
			}
			return false;
		}
		
		else if (Vector3.Distance(p_currentPosition, target) < p_parameters.threshold)
			return true;
		return false;
	}
}
