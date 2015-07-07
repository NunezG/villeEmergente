using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class TimeLineBT
{
    public TimeLineBT()
    {
        probability = 1f;
    }
	public TimeLineBT( string uri )
	{
		uriBT = uri;
        probability = 1f;
	}
    public string uriBT;
    public bool interuptible;
    public bool loopable;
    public float probability;
	// ??? parameters
}

[System.Serializable]
public class TimeLineItem
{
	// for multiple only !
	public bool interuptible;
	public bool loopable;
    public bool isRandom;

	public TimeLineItem()
    {
        _kind = Kind.Empty;
        isRandom = true;
    }

    public enum Kind
    {
        Syncro,
        Single,
        Multiple,
        Empty,
    }

    public TimeLineItem( string ID )
    {
        _kind = Kind.Syncro;
        _syncID = ID;
        isRandom = true;
    }
    public TimeLineItem(Kind what)
    {
        _kind = what;
        isRandom = true;
    }

    [SerializeField]
    Kind _kind;
    public Kind kind
    {
        get { return _kind; }
    }

    [SerializeField]
    string _syncID;
    public string syncID
    {
        get { return _syncID; }
    }

    public TimeLineBT behavior;
    public TimeLineBT[] behaviors;

	public void SwitchToMultiple()
	{
		behaviors = new TimeLineBT[1];
		behaviors [0] = new TimeLineBT( behavior.uriBT );
		_kind = Kind.Multiple;
        isRandom = true;
	}
	public void SwitchToSingle()
	{

		behavior = new TimeLineBT (behaviors [0].uriBT);
		behaviors = null;
		_kind = Kind.Single;
	}


	public bool isLoopable
	{
		get {
			if( loopable )
				return true;
			if( kind == Kind.Single )
				return behavior.loopable;
			if( kind == Kind.Multiple )
			{
				if( loopable )
					return true;
				for( int i = 0; i < behaviors.Length; ++i )
					if( behaviors[i].loopable )
						return true;
			}
			return false;
		}
	}
}


[System.Serializable]
public class LifeSceneRoleTimeLine 
{
    [SerializeField]
    public string roleName = "none";
	// a timeline for one role
    [SerializeField]
	public TimeLineItem[] sequence;

	void MakeLoopableUntilSyncro( int from )
	{
		for (int k = from + 1; k < sequence.Length; ++k)
		{
			if (sequence[k].kind == TimeLineItem.Kind.Syncro)
				break;
			if (sequence[k].kind == TimeLineItem.Kind.Single)
				sequence[k].behavior.loopable = true;
			if (sequence[k].kind == TimeLineItem.Kind.Multiple)
			{
				bool done = false;
				for( int j = 0; j < sequence[k].behaviors.Length; ++j )
				{
					if( sequence[k].behaviors[j].loopable == true )
					{
						done = true;
						break;
					}
				}
				if( !done )	
					sequence[k].loopable = true;
			}
		}
	}
	public bool Validate()
	{
		if( sequence.Length == 0 )
			return false;
		if( sequence[0].kind != TimeLineItem.Kind.Syncro )
			return false;
		if( sequence[sequence.Length-1].kind != TimeLineItem.Kind.Syncro )
			return false;
		for( int i = 0; i < sequence.Length; ++i )
		{
			TimeLineItem item = sequence[i];
			if( item.kind == TimeLineItem.Kind.Single )
			{
				if( item.behavior.loopable )
				{
					MakeLoopableUntilSyncro( i );
				}
			}
			if( item.kind == TimeLineItem.Kind.Multiple )
			{
				if( item.loopable )
				{
					MakeLoopableUntilSyncro( i );
				}
				else
				{
					for( int j = 0; j < item.behaviors.Length; ++j )
					{
						if( item.behaviors[j].loopable )
						{
							MakeLoopableUntilSyncro( i );
							break;
						}
					}
				}
			}
		}
		return true;
	}
}

[System.Serializable]
public class LifeSceneTimeLine 
{

    [SerializeField]
    public LifeSceneRoleTimeLine mainTimeline;       // main timeline, for syncro

    [SerializeField]
	public LifeSceneRoleTimeLine[] timeLineForRoles; // all timeline for each participating role    


    public LifeSceneRoleTimeLine FindTimeLineForRole( string roleName )
    {
        foreach (LifeSceneRoleTimeLine tl in timeLineForRoles)
        {
            if (tl.roleName == roleName)
                return tl;
        }
        return null;
    }


	public bool Validate( RoleParameters rp )
	{
		if( mainTimeline == null )
			return false;
		if( mainTimeline.sequence.Length == 0 )
			return false;
		if( mainTimeline.sequence[0].kind != TimeLineItem.Kind.Syncro )
			return false;
		if( mainTimeline.sequence[mainTimeline.sequence.Length-1].kind != TimeLineItem.Kind.Syncro )
			return false;

		bool success = true;
		foreach( LifeSceneRoleTimeLine tl in timeLineForRoles )
			success = tl.Validate() && success;
		return success;
	}
}
