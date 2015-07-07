using UnityEngine;
using System.Collections;
using System;

namespace mlv
{
    
    public class DebugParam 
        : StaticObject
    {
        public string outputMessage;
        
    }

    
    [AddComponentMenu( "LIFE/Standard Actions/debug" )]
    public class DebugAction : Action<DebugParam, StaticObject>
    {
        void Reset()
        {
            actionName = "debug";
        }
        public override Status start(EntityKnowledgeFacade entity, UInt32 request, DebugParam parameters, StaticObject outParam)
        {
            Debug.Log ( parameters.outputMessage );
            return Status.succeeded;
        }

        public override Status update(EntityKnowledgeFacade entity, UInt32 request, DebugParam parameters, StaticObject outParam, float dt)
        {
            return Status.succeeded; 
        }
        
        public override Status cancel( EntityKnowledgeFacade entity, UInt32 request, DebugParam parameters, float dt )
        {
            return Status.canceled;
        }
    }
}
