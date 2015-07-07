using UnityEngine;
using System.Collections;
using System;
using mlv;

// declare a class for holding the goto action parameters
// which is used in the BT
// deriving from StaticObject takes care of automatic registration
// the name of the member should match the name of the action parameters in the BT
public class GotoEntityParam
	: StaticObject
{
	public string target = "none";
	public int targetID = -1;
	public float threshold = 2f;
	public bool location = false;
}

public class GotoEntityAction : mlv.Action<GotoEntityParam, StaticObject>
{
	
	void Reset()
	{
		actionName = "gotoEntity";
	}	
	
	private Vector3 position;
	private uint index;
	private string Aname;
	private int nbrEntity;

    public override Status start(EntityKnowledgeFacade entity, UInt32 request, GotoEntityParam parameters, StaticObject paramsOut)
	{
		// just call the update function as all the code is in it
		nbrEntity = Simulation.instance.knowledgeFramework.getGlobalWorkingKnowledge().getInt("nbrEntity");
        return update(entity, request, parameters, paramsOut, 0f);
	}

    public override Status update(EntityKnowledgeFacade entity, UInt32 request, GotoEntityParam parameters, StaticObject paramsOut, float dt)
	{
		if(parameters.target != "none"){
			for(uint i = 0; i < nbrEntity; i++){
				if(i != entity.entity.entityID){
					Aname = entity.getEntityKnowledge(i).getString("name");
					if(Aname == parameters.target){
						entity.getEntityKnowledge(i).retrieve(entity.getEntityKnowledge(i).getProperty("position"), out position);
						if(parameters.location == true){
							entity.getKnowledge().setString("location", "none");
							entity.getKnowledge().setString("interactName", "none");
							entity.getKnowledge().setString("destination", Aname);
						}
					}
				}
			}
		}

		if(parameters.targetID != -1){
			index = (uint) parameters.targetID;
			entity.getEntityKnowledge(index).retrieve(entity.getEntityKnowledge(index).getProperty("position"), out position);
			if(parameters.location == true){
				entity.getKnowledge().setString("location", "none");
				entity.getKnowledge().setString("interactName", "none");
				//entity.getKnowledge().setString("destination", Aname);
			}
		}


		//index = (uint)parameters.target;
		//Debug.Log(index);
		//entity.getEntityKnowledge(index).retrieve(entity.getEntityKnowledge(index).getProperty("position"), out position);

		GameObject self = entity.entity.gameObject;
		NavMeshAgent nav = entity.entity.GetComponent<NavMeshAgent>();
		
		// Create a vector from the enemy to the last sighting of the player.
		Vector3 sightingDeltaPos = position - self.transform.position;
		
		// If the the last personal sighting of the player is not close...
		if(sightingDeltaPos.sqrMagnitude > 4f)
		{
			// ... set the destination for the NavMeshAgent to the last personal sighting of the player.
			nav.destination = position;
			
		}
		if(sightingDeltaPos.sqrMagnitude < parameters.threshold)
		{
			nav.Stop();
			if(parameters.location == true){
				entity.getKnowledge().setString("location", Aname);
				entity.getKnowledge().setString("interactName", Aname);
				entity.getKnowledge().setString("destination", "none");
			}
			return Status.succeeded;
		}

		/*for(uint i = 0; i < nbrEntity; i++){
			if(entity.getEntityKnowledge(i).getBool("interruption") ==  true)
			{
				nav.Stop();
				return Status.succeeded;
			}
		}*/

		return Status.running;
	}
	
	// called when the action is cancelled
	// nothing special to do
	public override Status cancel( EntityKnowledgeFacade entity, UInt32 request, GotoEntityParam parameters, float dt )
	{
		NavMeshAgent nav = entity.entity.GetComponent<NavMeshAgent>();
		nav.Stop();
		return Status.canceled;
	}
}
