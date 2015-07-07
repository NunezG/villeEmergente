using UnityEngine;
using UnityEditor;
using UnityExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LifeSceneTimeLineWindow 
    : EditorWindow 
{
    static Texture2D randomIcon = null;
	static Texture2D loopIcon = null;
	static Texture2D breakIcon = null;

    static GUIStyle titleStyle = null;
    static GUIStyle roleStyle = null;
    static GUIStyle btStyle = null;
    static GUIStyle oddStyle = null;
    static GUIStyle evenStyle = null;
    static GUIStyle syncStyle = null;
    static GUIStyle popupStyle = null;
    static GUIStyle dropStyle = null;
	static GUIStyle loopStyle = null;

	string msgProblematic = string.Empty;
    Rect selectedDropZone = new Rect();
    TimeLineItem hoverItem = null;
	TimeLineItem configItem = null;
	int configSubItem = -1;
    int subItem = -1;
    LifeSceneRoleTimeLine selectedTimeLine = null;
    Vector2 scrollPosition = Vector2.zero;
    Dictionary<string, List<string>> btsForRoles = new Dictionary<string, List<string>>();
    Dictionary<string, int> defaultBtForRoles = new Dictionary<string, int>();
    bool skipNext = false;
    LifeSceneParameters lastEdit = null;

    [MenuItem("Window/LIFE/TimeLine")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        LifeSceneTimeLineWindow window = (LifeSceneTimeLineWindow)EditorWindow.GetWindow(typeof(LifeSceneTimeLineWindow), false, "Time line");

        // Listen for updates - use this if you want real time graphs
        // if (EditorApplication.update != window.EditorUpdate) EditorApplication.update += window.EditorUpdate;

        window.Focus();
        window.OnSelectionChange();
    }

    static void Reinit()
    {
        randomIcon = EditorGUIUtility.Load("TimeLine/random.png") as Texture2D;
		loopIcon = EditorGUIUtility.Load( "TimeLine/loop.png" ) as Texture2D;
		breakIcon = EditorGUIUtility.Load( "TimeLine/break.png" ) as Texture2D;
        titleStyle = new GUIStyle(EditorStyles.boldLabel);
        titleStyle.fontSize = 50;
        titleStyle.fontStyle = FontStyle.BoldAndItalic;

        roleStyle = new GUIStyle(GUI.skin.box);
        roleStyle.fixedHeight = 50;
        roleStyle.fixedWidth = 195;
        roleStyle.alignment = TextAnchor.MiddleCenter;
        roleStyle.normal.textColor = Color.white;

        btStyle = new GUIStyle(GUI.skin.button);
        btStyle.fixedHeight = 40;
        btStyle.alignment = TextAnchor.MiddleCenter;
        btStyle.normal.textColor = Color.white;


        oddStyle = new GUIStyle(GUI.skin.button);

        syncStyle = new GUIStyle(GUI.skin.button);
        //syncStyle.fixedHeight = 100;
        syncStyle.fixedWidth = 20;

        popupStyle = new GUIStyle (EditorStyles.popup);
        popupStyle.border.top = 3;
        popupStyle.border.bottom = 3;
        popupStyle.alignment = TextAnchor.MiddleCenter;
        popupStyle.fixedHeight = 40;

        dropStyle = new GUIStyle (EditorStyles.toolbarTextField);
        dropStyle.alignment = TextAnchor.MiddleCenter;
        dropStyle.fixedHeight = 15;

		loopStyle = new GUIStyle (EditorStyles.toolbarTextField);
		loopStyle.alignment = TextAnchor.MiddleCenter;
		loopStyle.fixedHeight = 0;

	}
	void OnProjectChange()
    {
        Repaint();
        Setup();
    }
    void OnHierarchyChange()
    {
        Repaint();
        Setup();
    }
    void OnFocus()
    {
        Repaint();
        Setup();
    }
    public void OnSelectionChange()
    {
        Repaint();
        Setup();
    }


    public void Setup()
    {

		msgProblematic = string.Empty;
        skipNext = false;
        selectedTimeLine = null;
        hoverItem = null;
		configItem = null;
        if (Selection.activeGameObject == null)
            return;
        LifeSceneParameters target = Selection.activeGameObject.GetComponent<LifeSceneParameters>();
        lastEdit = target;
        if (target == null)
            return;
        
        // make sure we have one and only one timeLine per role 
        RoleParameters rp = target.roleParameters;
        LifeSceneTimeLine timeLine = target.timeLine;
        if (timeLine == null)
            timeLine = new LifeSceneTimeLine();
        target.timeLine = timeLine;
        ValidateTimeLine(rp, timeLine);

        btsForRoles.Clear();
        defaultBtForRoles.Clear ();

		if( target.roleParameters.Roles.Length == 0 )
		{
			msgProblematic = "No role defined\nCheck the Roles tab";
			return;
		}
		if( target.behaviorParameters.Behaviors.Length == 0 )
		{
			msgProblematic = "No behaviors defined\nCheck the Behaviors tab";
			return;
		}

        foreach (string role in target.roleParameters.Roles)
        {
			if( role == "" || role == null || role == string.Empty)
			{
				msgProblematic = "Empty role found\nCheck the Roles tab";
				return;
			}
			else if( btsForRoles.ContainsKey(role) == false )
			{
				btsForRoles.Add( role, new List<string>() );
				defaultBtForRoles.Add( role, 0 );
			} 
			else
			{
				msgProblematic = "Duplicated roles found :"+role+"\nCheck the Roles tab";
				return;
			}
        }
        
        for( int i = 0; i< target.behaviorParameters.Behaviors.Length;++i )
        {

            string bt = target.behaviorParameters.Behaviors[i];
            string role = target.behaviorParameters.Roles[i];
			if( bt == "" || bt == null || bt == string.Empty)
			{
				msgProblematic = "Empty behavior found\nCheck the Behaviors tab";
				return;
			}
			if( role == "" || role == null || role == string.Empty)
			{
				msgProblematic = "Empty role associated with behavior "+bt+"\nCheck the Behaviors tab";
				return;
			}
			if( btsForRoles.ContainsKey(role) == true )
            	btsForRoles[role].Add( bt );
			else
			{
				msgProblematic = "Inconsistent role found between the Roles tab and Behaviors tab\n"+role+" not found";
				return;
			}

        }
        foreach (string role in target.roleParameters.Roles)
        {
            btsForRoles[role] = btsForRoles[role].Distinct().ToList();
			if( btsForRoles[role].Count == 0 )
			{
				msgProblematic = "No Behaviors found for "+role;
				return;
			}
        }

		TypeRoleManager typeRole = TypeRoleManager.Instance;
		List<string> typeRoles = typeRole.typeRoles.ToList();

		timeLine.timeLineForRoles = timeLine.timeLineForRoles.OrderBy( tl => 
		                                  { 
			var idRole = target.roleParameters.Roles.ToList().FindIndex ( tr => tr == tl.roleName );
			var namedType = target.roleParameters.RolesType[idRole];
			var id = typeRoles.FindIndex( tr => tr == namedType );
			return id == -1 ? typeRoles.Count : id;
		} ).ToArray();
		
    }

    void OnGUI()
    {
        if (randomIcon == null)
            Reinit();
		if( loopIcon == null )
			Reinit();
		if( breakIcon == null )
			Reinit();
        if (titleStyle == null)
            Reinit();
        if (roleStyle== null)
            Reinit();
        if (btStyle == null)
            Reinit();
        if (oddStyle == null)
            Reinit();
        if (evenStyle == null)
            Reinit();
        if (syncStyle == null)
            Reinit();

        if (popupStyle == null)
            Reinit();

        if (dropStyle == null)
            Reinit ();
		if (loopStyle == null)
			Reinit ();
		//Setup();
		if (Selection.activeGameObject == null)
            return;
        LifeSceneParameters target = Selection.activeGameObject.GetComponent<LifeSceneParameters>();
        if (target == null)
            return;
        if (target != lastEdit)
            Setup();
        if (skipNext == true && Event.current.type != EventType.layout )
        {
            return;
        }
        if (skipNext == true && Event.current.type == EventType.layout)
        {
            skipNext = false; 
        }
        string name = target.name;
        GUI.skin.font = EditorStyles.boldFont;
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(name,titleStyle);
        GUILayout.EndHorizontal();
        LifeSceneTimeLine timeLine = target.timeLine;
        if (timeLine == null)
            timeLine = new LifeSceneTimeLine();
        target.timeLine = timeLine;


		if( msgProblematic != string.Empty )
		{
			EditorGUILayout.HelpBox( msgProblematic, MessageType.Error );
			return;
		}

		timeLine.Validate( target.roleParameters );
        Color current = Color.cyan;


        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        float fullHeight = -3;
        foreach (LifeSceneRoleTimeLine tl in target.timeLine.timeLineForRoles)
        {
            float h = GetTimeLineHeight(tl);
            if (h > 100)
                h += 20;
            fullHeight += h + 3;
        }
        
        foreach (LifeSceneRoleTimeLine tl in target.timeLine.timeLineForRoles)
        {
            GUI.backgroundColor = current;
            try
            {
                DrawTimeLine(target.timeLine, tl, fullHeight);
            }
            catch (System.ArgumentException)
            {
                break;
            }
            
            if (current == Color.cyan)
                current = Color.green;
            else
                current = Color.cyan;
            if( skipNext )
                break;
        }
        GUILayout.EndScrollView();

        if (Event.current.type == EventType.mouseUp )
        {
			Rect rc = GUILayoutUtility.GetLastRect ();
			if( rc.Contains( Event.current.mousePosition ) )
                if (hoverItem == null)
                {
                    if (configItem != null)
                        NormalizeProbas(configItem);

                    configItem = null;
                }
            selectedDropZone = new Rect();
            selectedTimeLine = null;
            hoverItem = null;
            RemoveEmpty(target.timeLine);
            RemoveEmptyColumns( target.timeLine );
            MergeSyncros(target.timeLine);
            CheckStartEnd( target.timeLine );
			Repaint();
        }
		if (hoverItem != null && Event.current.type == EventType.layout)
		{
            if (configItem != hoverItem)
                NormalizeProbas(configItem);
            configItem = hoverItem;
			configSubItem = subItem;
		}

		GUILayout.FlexibleSpace ();
        //GUILayout.Label(GUI.tooltip, titleStyle);
        if( Event.current.type == EventType.mouseDown )
            Repaint();
            
        if (Event.current.type == EventType.mouseDrag)
            Repaint();

		try
		{
			DrawSettings ();
		}
		catch( System.ArgumentException )
		{
		}
		catch( System.IndexOutOfRangeException )
		{
			configItem = null;
			configSubItem = -1;
		}
		if (PrefabUtility.GetPrefabParent (target.gameObject) != null)
			PrefabUtility.RecordPrefabInstancePropertyModifications (target);
    }

    void NormalizeProbas( float newValue, TimeLineItem item, int sub )
    {
        item.behaviors[sub].probability = Mathf.Clamp01(newValue);
        float reminder = 1f - newValue;
        float sum = 0f;

        for (int l = 0; l < item.behaviors.Length; ++l)
        {
            if (sub == l)
                continue;
            sum += item.behaviors[l].probability;
        }
        for (int l = 0; l < item.behaviors.Length; ++l)
        {
            if (sub == l)
                continue;
            if (sum == 0f)
                item.behaviors[l].probability = reminder / (float)item.behaviors.Length;
            else
                item.behaviors[l].probability = reminder * item.behaviors[l].probability / sum;
            item.behaviors[l].probability = Mathf.Clamp01(item.behaviors[l].probability);
        }

        sum = 0f;
        for (int l = 0; l < item.behaviors.Length; ++l)
        {
            sum += item.behaviors[l].probability;
        }
        for (int l = 0; l < item.behaviors.Length; ++l)
        {
            if (sum == 0f)
                item.behaviors[l].probability = 1f / (float)item.behaviors.Length;
            else
                item.behaviors[l].probability /= sum;
            item.behaviors[l].probability = Mathf.Clamp01(item.behaviors[l].probability);
        } 
    }

    void NormalizeProbas(TimeLineItem item)
    {
        if (item == null)
            return;
        if (item.kind != TimeLineItem.Kind.Multiple)
            return;
        if (item.isRandom)
            return;
        float sum = 0f;
        for (int l = 0; l < item.behaviors.Length; ++l)
        {
            sum += item.behaviors[l].probability;
        }
        for (int l = 0; l < item.behaviors.Length; ++l)
        {
            if (sum == 0f)
                item.behaviors[l].probability = 1f / (float)item.behaviors.Length;
            else
                item.behaviors[l].probability /= sum;
            item.behaviors[l].probability = Mathf.Clamp01(item.behaviors[l].probability);
        }
    }

	void DrawSettings()
	{
		GUI.backgroundColor = Color.white;
		Rect rc = EditorGUILayout.BeginVertical (EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).window, GUILayout.Width (200), GUILayout.ExpandWidth (false), GUILayout.Height(200), GUILayout.ExpandHeight(false) );
		GUI.Label (rc, "Settings", EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).window);
		if( configItem == null )
		{
			GUILayout.FlexibleSpace();
			rc = GUILayoutUtility.GetLastRect();
			rc.width = 190;
			EditorGUI.HelpBox(rc, "To configure\nSelect an item", MessageType.Info );
		}
		else
		{
		//	GUILayout.Space (200);
			if( configItem.kind == TimeLineItem.Kind.Syncro )
			{
				GUILayout.FlexibleSpace();
				rc = GUILayoutUtility.GetLastRect();
				rc.width = 190;
				EditorGUI.HelpBox(rc, "No settings for syncro point", MessageType.Info );
			}
			else if( configItem.kind == TimeLineItem.Kind.Single )
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel( "Interruptible" );
				configItem.behavior.interuptible = EditorGUILayout.Toggle( configItem.behavior.interuptible );
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel( "Loop" );
				configItem.behavior.loopable = EditorGUILayout.Toggle( configItem.behavior.loopable, GUILayout.ExpandWidth(false) );
				EditorGUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			}
			else if( configItem.kind == TimeLineItem.Kind.Multiple && configSubItem == -1 )
			{
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Random");
                configItem.isRandom = EditorGUILayout.Toggle(configItem.isRandom);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel( "Interruptible" );
				configItem.interuptible = EditorGUILayout.Toggle( configItem.interuptible );
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel( "Loop" );
				configItem.loopable = EditorGUILayout.Toggle( configItem.loopable, GUILayout.ExpandWidth(false) );
				EditorGUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			}
			else if( configItem.kind == TimeLineItem.Kind.Multiple && configSubItem != -1 )
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel( "Interruptible" );
				configItem.behaviors[configSubItem].interuptible = EditorGUILayout.Toggle( configItem.behaviors[configSubItem].interuptible );
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel( "Loop" );
				configItem.behaviors[configSubItem].loopable = EditorGUILayout.Toggle( configItem.behaviors[configSubItem].loopable, GUILayout.ExpandWidth(false) );
				EditorGUILayout.EndHorizontal();

                if (configItem.isRandom == false)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Proba");
                    float newVal = EditorGUILayout.FloatField(configItem.behaviors[configSubItem].probability);
                    configItem.behaviors[configSubItem].probability = Mathf.Clamp01(newVal);
                    //NormalizeProbas(newVal, configItem, configSubItem);
                    EditorGUILayout.EndHorizontal();
                    GUILayout.FlexibleSpace();

                }
                GUILayout.FlexibleSpace();
            }
			else
			{
				GUILayout.FlexibleSpace();
			}
		}
		EditorGUILayout.EndVertical ();

	}


    string CreateUID()
    {
        return System.Guid.NewGuid().ToString();
    }

    void ValidateTimeLine( RoleParameters rp, LifeSceneTimeLine timeLine)
    {
        // at least 1 sync point
        if( timeLine.mainTimeline == null )
        {
            timeLine.mainTimeline = new LifeSceneRoleTimeLine();
			timeLine.mainTimeline.sequence = new TimeLineItem[0];
        }
        if( timeLine.mainTimeline.sequence.Length == 0 )
        {
            ArrayUtility.Add(ref timeLine.mainTimeline.sequence, new TimeLineItem( CreateUID() ));
        }
        // remove timeline not associated with a role
        timeLine.timeLineForRoles = timeLine.timeLineForRoles.Where(tl => rp.Roles.Contains( tl.roleName) ).ToArray();
        // check each role has a timeLine
        foreach (string role in rp.Roles)
        {
            LifeSceneRoleTimeLine tl = timeLine.FindTimeLineForRole(role);
            // add one if none exists
            if (tl == null)
            {
                tl = new LifeSceneRoleTimeLine();
                tl.roleName = role;
                ArrayUtility.Add(ref timeLine.timeLineForRoles, tl);
            }
            if( tl.sequence == null )
                tl.sequence = new TimeLineItem[0];
         
            if( tl.sequence.Length == 0 )
            {
                foreach ( TimeLineItem main in timeLine.mainTimeline.sequence)
                {
                    TimeLineItem item;
                    if (main.kind == TimeLineItem.Kind.Syncro)
                        item = new TimeLineItem(main.syncID);
                    else
                        item = new TimeLineItem();
                    ArrayUtility.Add(ref tl.sequence, item);
                }
            }
        }
    }

    bool IsColumnEmpty(LifeSceneTimeLine timeLine, int id)
    {
        if (timeLine.mainTimeline.sequence[id].kind == TimeLineItem.Kind.Syncro)
            return false;
        foreach (LifeSceneRoleTimeLine tl in timeLine.timeLineForRoles)
            if (tl.sequence[id].kind != TimeLineItem.Kind.Empty)
                return false;
        return true;
    }

    void RemoveEmptyColumns(LifeSceneTimeLine timeLine)
    {
        List< int > empty = new List<int>();
        for (int i = 0; i < timeLine.mainTimeline.sequence.Length; ++i)
        {
            if (IsColumnEmpty(timeLine, i))
                empty.Add(i);
        }
        empty.Reverse();
        foreach (int col in empty)
        {
            ArrayUtility.RemoveAt(ref timeLine.mainTimeline.sequence, col);
            foreach( LifeSceneRoleTimeLine tl in timeLine.timeLineForRoles )
                ArrayUtility.RemoveAt(ref tl.sequence, col);
        }
    }

    void Swap(LifeSceneRoleTimeLine tl, int id0, int id1)
    {
        TimeLineItem item = tl.sequence[id0];
        tl.sequence[id0] = tl.sequence[id1];
        tl.sequence[id1] = item;
    }

    bool RemoveEmpty(LifeSceneRoleTimeLine tl)
    {
        int lastSync = 0;
        int lastEmpty = 0;
        for (int i = 0; i < tl.sequence.Length; ++i)
        {
            if (tl.sequence[i].kind == TimeLineItem.Kind.Syncro)
            {
                lastSync = i;
            }
            else if (tl.sequence[i].kind == TimeLineItem.Kind.Empty)
            {
                lastEmpty = i;
            }
            else if (tl.sequence[i].kind != TimeLineItem.Kind.Empty)
            {
                if (lastEmpty > lastSync)
                {
                    Swap(tl, i, lastEmpty);
                    lastEmpty = i;
                    return true;
                }
            }
        }
        return false;
    }

    void RemoveEmpty(LifeSceneTimeLine timeLine)
    {
        foreach (LifeSceneRoleTimeLine tl in timeLine.timeLineForRoles)
        {
            while (RemoveEmpty(tl)) ;
        }
    }

    void MergeSyncros(LifeSceneTimeLine timeLine)
    {
        List< int > merge = new List<int>();
        TimeLineItem.Kind last = TimeLineItem.Kind.Empty;
        for (int i = 0; i < timeLine.mainTimeline.sequence.Length; ++i)
        {
            if (last == TimeLineItem.Kind.Syncro && timeLine.mainTimeline.sequence[i].kind == TimeLineItem.Kind.Syncro)
                merge.Add(i);
            last = timeLine.mainTimeline.sequence[i].kind;
        }
        merge.Reverse();
        foreach (int col in merge)
        {
            ArrayUtility.RemoveAt(ref timeLine.mainTimeline.sequence, col);
            foreach (LifeSceneRoleTimeLine tl in timeLine.timeLineForRoles)
                ArrayUtility.RemoveAt(ref tl.sequence, col);
        }
    }

    void CheckStartEnd(LifeSceneTimeLine timeLine)
    {
        if (timeLine.mainTimeline.sequence[0].kind != TimeLineItem.Kind.Syncro)
        {
            string syncID = CreateUID();
            ArrayUtility.Insert(ref timeLine.mainTimeline.sequence, 0, new TimeLineItem(syncID));
            foreach( LifeSceneRoleTimeLine tl in timeLine.timeLineForRoles )
                ArrayUtility.Insert(ref tl.sequence, 0, new TimeLineItem(syncID));

        }
        if (timeLine.mainTimeline.sequence[timeLine.mainTimeline.sequence.Length-1].kind != TimeLineItem.Kind.Syncro)
        {
            string syncID = CreateUID();
            ArrayUtility.Add(ref timeLine.mainTimeline.sequence,  new TimeLineItem(syncID));
            foreach (LifeSceneRoleTimeLine tl in timeLine.timeLineForRoles)
                ArrayUtility.Add(ref tl.sequence, new TimeLineItem(syncID));
        }
    }

    void AddItem(LifeSceneTimeLine timeLine, LifeSceneRoleTimeLine which, TimeLineItem item)
    {
        if( item.kind == TimeLineItem.Kind.Syncro )
            ArrayUtility.Add(ref timeLine.mainTimeline.sequence, new TimeLineItem( item.syncID ));
        else
            ArrayUtility.Add(ref timeLine.mainTimeline.sequence, new TimeLineItem());
        foreach (LifeSceneRoleTimeLine tl in timeLine.timeLineForRoles)
        {
            if( tl != which )
            {
                if (item.kind == TimeLineItem.Kind.Syncro)    
                    ArrayUtility.Add(ref tl.sequence, new TimeLineItem( item.syncID ));
                else
                    ArrayUtility.Add(ref tl.sequence, new TimeLineItem());
            }
            else
                ArrayUtility.Add(ref tl.sequence, item );
        }

    }
    bool IsSyncBetween(LifeSceneTimeLine timeLine, int id0, int id1)
    {
        if (id0 < id1)
        {
            for (int i = id0; i <= id1; ++i)
            {
                if (timeLine.mainTimeline.sequence[i].kind == TimeLineItem.Kind.Syncro)
                    return true;
            }
        }
        else
        {
            for (int i = id0; i >= id1; --i)
            {
                if (timeLine.mainTimeline.sequence[i].kind == TimeLineItem.Kind.Syncro)
                    return true;
            }
        }
        return false;
    }

    void InsertRight(LifeSceneTimeLine timeLine, LifeSceneRoleTimeLine roleTimeLine, int idSelected, int id)
    {
        if (idSelected == id + 1)
            return;
        TimeLineItem selectedItem = timeLine.mainTimeline.sequence[idSelected];
        if (selectedItem.kind == TimeLineItem.Kind.Syncro)
        {
            timeLine.mainTimeline.sequence[idSelected] = null;
            ArrayUtility.Insert(ref timeLine.mainTimeline.sequence, id + 1, selectedItem);
            ArrayUtility.Remove(ref timeLine.mainTimeline.sequence, null);

            foreach (LifeSceneRoleTimeLine tl in timeLine.timeLineForRoles)
            {
                selectedItem = tl.sequence[idSelected];
                tl.sequence[idSelected] = null;
                ArrayUtility.Insert(ref tl.sequence, id + 1, selectedItem);
                ArrayUtility.Remove(ref tl.sequence, null);
            }
        }
        else
        {
            selectedItem = roleTimeLine.sequence[idSelected];
            if (selectedItem.kind != TimeLineItem.Kind.Empty)
            {
                // is there a syncro between both ?                
                if (IsSyncBetween(timeLine, idSelected, id))
                {
                    // grow the new sync
                    ArrayUtility.Insert(ref timeLine.mainTimeline.sequence, id + 1, new TimeLineItem() );
                    foreach (LifeSceneRoleTimeLine tl in timeLine.timeLineForRoles)
                    {
                        if (tl != roleTimeLine)
                            ArrayUtility.Insert(ref tl.sequence, id + 1, new TimeLineItem());
                    }
                    roleTimeLine.sequence[idSelected] = new TimeLineItem();
                    ArrayUtility.Insert(ref roleTimeLine.sequence, id + 1, selectedItem);
                }
                else
                {
                    roleTimeLine.sequence[idSelected] = null;
                    ArrayUtility.Insert(ref roleTimeLine.sequence, id + 1, selectedItem);
                    ArrayUtility.Remove(ref roleTimeLine.sequence, null);
                }
            }
            RemoveEmpty(timeLine);
            RemoveEmptyColumns(timeLine);
        }

    }
    void InsertLeft(LifeSceneTimeLine timeLine, LifeSceneRoleTimeLine roleTimeLine, int idSelected, int id)
    {
        if (idSelected == id - 1)
            return;
        TimeLineItem selectedItem = timeLine.mainTimeline.sequence[idSelected];
        if (selectedItem.kind == TimeLineItem.Kind.Syncro)
        {
            timeLine.mainTimeline.sequence[idSelected] = null;
            ArrayUtility.Remove(ref timeLine.mainTimeline.sequence, selectedItem);
            ArrayUtility.Insert(ref timeLine.mainTimeline.sequence, id, selectedItem);
            ArrayUtility.Remove(ref timeLine.mainTimeline.sequence, null);

            foreach (LifeSceneRoleTimeLine tl in timeLine.timeLineForRoles)
            {
                selectedItem = tl.sequence[idSelected];
                tl.sequence[idSelected] = null;
                ArrayUtility.Remove(ref tl.sequence, selectedItem);
                ArrayUtility.Insert(ref tl.sequence, id, selectedItem);
                ArrayUtility.Remove(ref tl.sequence, null);
            }
        }
        else
        {
            selectedItem = roleTimeLine.sequence[idSelected];
            if (selectedItem.kind != TimeLineItem.Kind.Empty)
            {
                // is there a syncro between both ?                
                if (IsSyncBetween(timeLine, idSelected, id))
                {
                    ArrayUtility.Insert(ref timeLine.mainTimeline.sequence, id, new TimeLineItem());
                    foreach (LifeSceneRoleTimeLine tl in timeLine.timeLineForRoles)
                    {
                        if (tl != roleTimeLine)
                            ArrayUtility.Insert(ref tl.sequence, id , new TimeLineItem());
                    }
                    roleTimeLine.sequence[idSelected] = new TimeLineItem();
                    ArrayUtility.Insert(ref roleTimeLine.sequence, id, selectedItem);
                }
                else
                {
                    roleTimeLine.sequence[idSelected] = null;
                    ArrayUtility.Remove(ref roleTimeLine.sequence, selectedItem);
                    ArrayUtility.Insert(ref roleTimeLine.sequence, id, selectedItem);
                    ArrayUtility.Remove(ref roleTimeLine.sequence, null);
                }
                RemoveEmpty(timeLine);
                RemoveEmptyColumns(timeLine);
            }
        }
    }

    int ChoosenIdFromBtRole( string role, string bt )
    {
        for (int i = 0; i < btsForRoles[role].Count; ++i)
        {
            if (btsForRoles[role][i] == bt)
                return i;
        }
        return -1;
    }

    float GetTimeLineHeight( LifeSceneRoleTimeLine tl )
    {
        float maxHeight = 100;
        for (int i = 0; i < tl.sequence.Length; ++i)
        {
            if (tl.sequence[i].kind == TimeLineItem.Kind.Multiple)
            {
                float h = 40 * tl.sequence[i].behaviors.Length;
                maxHeight = Mathf.Max(h, maxHeight);
            }
        }        
        return maxHeight;
    }

    void DrawTimeLine( LifeSceneTimeLine timeLine, LifeSceneRoleTimeLine tl, float totalHeight )
    {

        float maxHeight = GetTimeLineHeight(tl);


        Color oldColor = GUI.backgroundColor;
        GUILayout.BeginHorizontal(oddStyle);
        float bottom = 100;


        GUILayout.BeginVertical(GUILayout.Width(200), GUILayout.ExpandWidth(false));
            Rect rcBox = GUILayoutUtility.GetRect( new GUIContent( tl.roleName ), roleStyle);
            GUI.Box( rcBox, tl.roleName, roleStyle);
            
            rcBox.x += rcBox.width - 18;
            rcBox.y += 1;
            rcBox.width = 18;
            rcBox.height = 18;
            GUI.backgroundColor = Color.red;
            if (GUI.Button( rcBox, "\u2718", EditorStyles.miniButton))
            {
                ArrayUtility.Clear(ref tl.sequence);
                foreach (TimeLineItem main in timeLine.mainTimeline.sequence)
                {
                    TimeLineItem item;
                    if (main.kind == TimeLineItem.Kind.Syncro)
                        item = new TimeLineItem(main.syncID);
                    else
                        item = new TimeLineItem();
                    ArrayUtility.Add(ref tl.sequence, item);
                }
                hoverItem = null;
                if (configItem != null)
                    NormalizeProbas(configItem);
                configItem = null;
	            subItem = -1;
				configSubItem = -1;
                RemoveEmpty(timeLine);
                RemoveEmptyColumns(timeLine);
                MergeSyncros(timeLine);
                CheckStartEnd(timeLine);
                return;
            }
            GUI.backgroundColor = oldColor;

            List< string > bts = btsForRoles[tl.roleName];
            Rect btChoice = GUILayoutUtility.GetRect(new GUIContent(""), btStyle, GUILayout.Width(200), GUILayout.ExpandWidth(false));
            btChoice.width -= 20;
            if( Event.current.type == EventType.mouseDown && btChoice.Contains( Event.current.mousePosition ) && bts.Count > 0 )
            {
                selectedTimeLine = tl;
                hoverItem = new TimeLineItem(TimeLineItem.Kind.Single);
                hoverItem.behavior = new TimeLineBT(bts[defaultBtForRoles[tl.roleName]]);
                subItem = -1;
                if (configItem != null)
                    NormalizeProbas(configItem);
                configItem = null;
				configSubItem = -1;
            }	
            if ( selectedTimeLine == tl && hoverItem != null && hoverItem.kind == TimeLineItem.Kind.Single && hoverItem.behavior.uriBT == bts [defaultBtForRoles [tl.roleName]])
            {
                GUI.backgroundColor = Color.magenta;
                if( Event.current.type == EventType.mouseUp && btChoice.Contains( Event.current.mousePosition ) )
                {
                    selectedTimeLine = null;
                    hoverItem = null;
                    subItem = -1;
                    if (configItem != null)
                        NormalizeProbas(configItem);
                    configItem = null;
					configSubItem = -1;
                    TimeLineItem item = new TimeLineItem(TimeLineItem.Kind.Single);
                    item.behavior = new TimeLineBT(bts[defaultBtForRoles[tl.roleName]]);
                    AddItem(timeLine, tl, item);
                    RemoveEmpty(timeLine);
                    RemoveEmptyColumns(timeLine);
                    MergeSyncros(timeLine);
                    Event.current.Use();
                    return;
                }
            }
            if (bts.Count > 0)
            {
                GUI.Label(btChoice, bts[defaultBtForRoles[tl.roleName]], btStyle);
                btChoice.x += btChoice.width - 2;
                btChoice.width = 18;
                defaultBtForRoles[tl.roleName] = EditorGUI.Popup(btChoice, defaultBtForRoles[tl.roleName], bts.ToArray(), popupStyle);
            }
            else
            {
                GUI.Label(btChoice, "No BTs", btStyle);
            }
            
//			GUI.backgroundColor = Color.red;
            if (maxHeight > 100)
            {
                bottom = rcBox.y+maxHeight+20;
                GUILayout.Space ( maxHeight-100+20);
            }
            else
                bottom = rcBox.y+maxHeight;
                //				GUILayout.Box( "", oddStyle, GUILayout.Height ( maxHeight-100+20)) ;
            GUI.backgroundColor = oldColor;
        GUILayout.EndVertical();

        if (hoverItem != null && hoverItem.kind == TimeLineItem.Kind.Syncro)
        {
            hoverItem = tl.sequence.Where(it => it.kind == TimeLineItem.Kind.Syncro && it.syncID == hoverItem.syncID).ToArray()[0];
			configItem = hoverItem;			
        }


        for (int i = 0; i < tl.sequence.Length; ++i )
        {
            Rect dropZone = new Rect ();
            int iOrig = i;
            TimeLineItem item = tl.sequence[i];
            Rect rc = new Rect();
			int idSelected = ArrayUtility.IndexOf(tl.sequence, hoverItem);
            if ( item.kind == TimeLineItem.Kind.Syncro )
            {
                GUI.backgroundColor = Color.yellow;
                GUILayout.BeginVertical(GUILayout.Width(20));
                rc = GUILayoutUtility.GetRect(new GUIContent(""), syncStyle);
                rc.y = 6;
                //rc.height = 102 * timeLine.timeLineForRoles.Length + 3 * (timeLine.timeLineForRoles.Length-1);
                rc.height = totalHeight;
				if( configItem == item )
					GUI.backgroundColor = Colors.PastelOrange;
                if (hoverItem == item)
                    GUI.backgroundColor = Color.red;
                GUI.Label(rc, new GUIContent("", item.syncID ), syncStyle);

                GUILayout.EndVertical();
                if(! (i == 0 || i == tl.sequence.Length - 1) )
                {
                    Rect b = rc;
                    b.x += 1;
                    b.y += 1;
                    b.width -= 2;
                    b.height = 20;
                    GUI.backgroundColor = Color.red;
                    if (GUI.Button(b, "\u2718", EditorStyles.miniButton))
                    {
                        hoverItem = null;
                        if (configItem != null)
                            NormalizeProbas(configItem);
                        configItem = null;
                        subItem = -1;
						configSubItem = -1;
                        timeLine.mainTimeline.sequence[i] = new TimeLineItem();
                        foreach (LifeSceneRoleTimeLine t in timeLine.timeLineForRoles)
                            t.sequence[i] = new TimeLineItem();
                        RemoveEmpty(timeLine);
                        RemoveEmptyColumns(timeLine);
                        MergeSyncros(timeLine);
                    }
                    rc.y += 20;
                    rc.height -= 20;
                }
                if (Event.current.type == EventType.mouseDown)
                    if (rc.Contains(Event.current.mousePosition))
                        hoverItem = item;
                GUI.backgroundColor = oldColor;
            }
            else if (item.kind == TimeLineItem.Kind.Single)
            {
                
                GUILayout.BeginVertical(GUILayout.Width(200));
                float width = 200f;
                for (int k = i + 1; k < tl.sequence.Length; ++k)
                {
                    if (tl.sequence[k].kind == TimeLineItem.Kind.Empty)
                    {
                        i = k;
                        width += 4 + 200;
                    }
                    else
                        break;
                }

				rc = GUILayoutUtility.GetRect(new GUIContent(""), btStyle, GUILayout.Width(width));

				if( item.isLoopable )
				{
					if( i == 0 || !tl.sequence[i-1].isLoopable )
					{
						float widthLoop = width;
						for (int k = i + 1; k < tl.sequence.Length; ++k)
						{
							if (tl.sequence[k].kind == TimeLineItem.Kind.Syncro)
								break;
							widthLoop += 4 + 200;
						}
						if( widthLoop != width )
						{
							Rect loop = rc;
							loop.width = widthLoop;
							loop.height = 16;
							loop.y = bottom - 18;
							GUI.backgroundColor = GUI.backgroundColor.Darken();
							GUI.Label ( loop, loopIcon, loopStyle );
							GUI.backgroundColor = oldColor;
						}
					}
				}
						    
						    
                    
				if( configItem == item )
					GUI.backgroundColor = Colors.PastelOrange;
				if (hoverItem == item )
                    GUI.backgroundColor = Color.red;
                GUI.Label(rc,item.behavior.uriBT, btStyle);
                GUI.backgroundColor = oldColor;

				if( hoverItem != null && hoverItem != item && (hoverItem.kind == TimeLineItem.Kind.Single || (hoverItem.kind == TimeLineItem.Kind.Multiple && subItem != -1 ) ) )
				{
					if( idSelected != -1 || selectedTimeLine == tl )
					{
		                dropZone = rc;
		                dropZone.y = bottom-15;
		                dropZone.x += 40;
		                dropZone.width -= 80;
		                dropZone.height = 20;
		                if( selectedDropZone == dropZone )
		                    GUI.backgroundColor = Color.red;
		                GUI.backgroundColor = GUI.backgroundColor.Darken(.5f);
		                GUI.Label( dropZone, "Drop to add", dropStyle );
					}
				}
                GUI.backgroundColor = oldColor;

                GUILayout.EndVertical();

                Rect b = rc;
                b.height = 20;
                b.width = 18;
                b.x += rc.width - 20;
                b.y += 1;
                GUI.backgroundColor = Color.red;
                if (GUI.Button(b, "\u2718", EditorStyles.miniButton))
                {
                    hoverItem = null;
                    if (configItem != null)
                        NormalizeProbas(configItem);
                    configItem = null;
                    subItem = -1;
					configSubItem = -1;
                    tl.sequence[iOrig] = new TimeLineItem();
                    RemoveEmpty( timeLine );
                    RemoveEmptyColumns( timeLine );
                    MergeSyncros(timeLine);
                    break;
                }

                GUI.backgroundColor = oldColor;
                b.y += 22;
                b.x += 1;
                b.width = 18;
                int currentIdx = ChoosenIdFromBtRole(tl.roleName, item.behavior.uriBT);
                int newIdx = EditorGUI.Popup(b, currentIdx, btsForRoles[tl.roleName].ToArray());
                if (newIdx == -1 || newIdx != currentIdx)
                {
                    if (newIdx == -1)
                        newIdx = 0;
                    item.behavior.uriBT = btsForRoles[tl.roleName][newIdx];
                }

				b= rc;
				b.width=16;
				b.x += 2;
				b.y += 2;
				if( item.behavior.loopable )
				{
					GUI.Label(b , loopIcon );
					b.y += 18;
				}
				if( item.behavior.interuptible )
				{
					GUI.Label(b , breakIcon );
				}

                rc.width -= 21;

                
                if (Event.current.type == EventType.mouseDown)
                    if (rc.Contains(Event.current.mousePosition))
                        hoverItem = item;
                if (hoverItem != null && hoverItem.kind == TimeLineItem.Kind.Syncro)
                    rc.height = maxHeight;
                rc.width += 21;
                //rc.height = 100;
            }
            else if (item.kind == TimeLineItem.Kind.Empty)
            {
                GUILayout.BeginVertical(GUILayout.Width(200));

                float width = 200f;
                for (int k = i + 1; k < tl.sequence.Length; ++k)
                {
                    if (tl.sequence[k].kind == TimeLineItem.Kind.Empty)
                    {
                        i = k;
                        width += 4 + 200;
                    }
                    else
                        break;
                }

                if (Event.current.type == EventType.mouseDrag && hoverItem != null && hoverItem.kind == TimeLineItem.Kind.Syncro)
                {
                    rc = GUILayoutUtility.GetRect(new GUIContent(""), btStyle, GUILayout.Width(width));
                    GUI.Label(rc, "", btStyle);
                    rc.height = maxHeight;
                }
                else
                {
                    //rc = GUILayoutUtility.GetRect(new GUIContent(""), btStyle);
                //    GUILayout.Label("", btStyle, GUILayout.Width(width));
					rc = GUILayoutUtility.GetRect(new GUIContent(""), btStyle, GUILayout.Width(width));
					GUI.Label(rc, "", btStyle);
				}

                GUILayout.EndVertical();
            }
            else if (item.kind == TimeLineItem.Kind.Multiple)
            {

                GUI.backgroundColor = Color.grey;
				if( configItem == item && subItem == -1 )
					GUI.backgroundColor = Colors.PastelOrange;
				if (hoverItem == item && subItem == -1)
                    GUI.backgroundColor = Color.red;

                float width = 180;
                for (int k = i + 1; k < tl.sequence.Length; ++k)
                {
                    if (tl.sequence[k].kind == TimeLineItem.Kind.Empty)
                    {
                        i = k;
                        width += 4 + 200;
                    }
                    else
                        break;
                }

				GUI.backgroundColor = oldColor;
				GUILayout.BeginVertical(oddStyle, GUILayout.Width(width+20f));
				GUILayout.Space (-2);
				rc = GUILayoutUtility.GetLastRect();

				if( item.isLoopable )
				{
					if( i == 0 || !tl.sequence[i-1].isLoopable )
					{
						float widthLoop = width;
						for (int k = i + 1; k < tl.sequence.Length; ++k)
						{
							if (tl.sequence[k].kind == TimeLineItem.Kind.Syncro)
								break;
							widthLoop += 4 + 200;
						}
						if( widthLoop != width )
						{
							Rect loop = rc;
							loop.x -= 4;
							loop.width = widthLoop+16;
							loop.height = 16;
							loop.y = bottom - 18;
							GUI.backgroundColor = GUI.backgroundColor.Darken();
							GUI.Label ( loop, loopIcon, loopStyle );
							GUI.backgroundColor = oldColor;
						}
					}
				}
                

                

                for( int k = 0; k < item.behaviors.Length; ++k )
                {
                    Rect t = GUILayoutUtility.GetRect(new GUIContent(""), btStyle, GUILayout.Width(width));

                    t.x -= 6;
                    if( k == 0 )
                        rc = t;

                    GUI.backgroundColor = oldColor;
					if( configItem == item && ( subItem == k || subItem == -1 ) )
						GUI.backgroundColor = Colors.PastelOrange;
					if (hoverItem == item && ( subItem == k || subItem == -1 ) )
                        GUI.backgroundColor = Color.red;
                    GUI.Label(t,item.behaviors[k].uriBT, btStyle);
                    Rect b = t;
                    b.height = 20;
                    b.width = 18;
                    b.x += t.width - 20;
                    b.y += 1;
                    GUI.backgroundColor = Color.red;
                    if (GUI.Button(b, "\u2718", EditorStyles.miniButton))
                    {
                        ArrayUtility.RemoveAt( ref item.behaviors, k );
                        if( item.behaviors.Length == 1 )
                            item.SwitchToSingle();
                        break;
                    }
                    GUI.backgroundColor = oldColor;
                    b.y += 22;
                    b.x += 1;
                    b.width = 18;
                    int currentIdx = ChoosenIdFromBtRole(tl.roleName, item.behaviors[k].uriBT);
                    int newIdx = EditorGUI.Popup(b, currentIdx, btsForRoles[tl.roleName].ToArray());
                    if (newIdx == -1 || newIdx != currentIdx)
                    {
                        if (newIdx == -1)
                            newIdx = 0;
                        item.behaviors[k].uriBT = btsForRoles[tl.roleName][newIdx];
                    }
                    
					b= t;
					b.width=16;
					b.x += 2;
					b.y += 2;
					if( item.behaviors[k].loopable )
					{
						GUI.Label(b , loopIcon );
						b.y += 16;
					}
					if( item.behaviors[k].interuptible )
					{
						GUI.Label(b , breakIcon );
                        b.y += 16;
					}

                    if (!item.isRandom)
                    {
                        b.width = t.width - 4 - 16;
                        b.y = t.y + 26;
                        b.height = 10;
                        float newVal = GUI.HorizontalSlider(b, item.behaviors[k].probability, 0f, 1f);
                        if (newVal != item.behaviors[k].probability)
                        {
                            if (configItem != null && configItem != item)
                                NormalizeProbas(configItem);
                            hoverItem = item;
                            subItem = k;
                            NormalizeProbas(newVal, item, k);
                        }
                        
                    }
                    dropZone = t;

                    t.width -= 21;
                    if (Event.current.type == EventType.mouseDown)
                        if (t.Contains(Event.current.mousePosition))
                        {
                            hoverItem = item;
                            subItem = k;
                        }

                    GUILayout.Space(-3);
                }

                if( item.kind == TimeLineItem.Kind.Multiple ) // in case reverted to single !
                {
                    Rect old = rc;
                    rc.x += rc.width;
                    rc.height = 40*item.behaviors.Length;
                    rc.width = 20;

                    Rect bq = rc;
                    bq.height = 20;
                    bq.width = 18;
                    bq.y += 1;
                    GUI.backgroundColor = Color.red;
                    if (GUI.Button(bq, "\u2718", EditorStyles.miniButton))
                    {
                        hoverItem = null;
                        subItem = -1;
                        if (configItem != null )
                            NormalizeProbas(configItem);
						configItem = null;
						configSubItem = -1;
                        tl.sequence[iOrig] = new TimeLineItem();
                        RemoveEmpty( timeLine );
                        RemoveEmptyColumns( timeLine );
                        MergeSyncros(timeLine);
                        break;
                    }

					bq.width=16;
					//bq.x += 2;
					bq.y += 20;
					if( item.loopable )
					{
						GUI.Label(bq , loopIcon );
						bq.y += 18;
					}
					if( item.interuptible )
					{
						GUI.Label(bq , breakIcon );
                        bq.y += 18;
                    }
                    if (item.isRandom)
                    {
                        GUI.Label(bq, randomIcon);
                        bq.y += 18;
                    }

                    
                    rc.y+=20;
                    rc.height-=20;
                    if (Event.current.type == EventType.mouseDown)
                        if (rc.Contains(Event.current.mousePosition))
                        {
                            hoverItem = item;
                            subItem = -1;
                            if (configItem != null)
                                NormalizeProbas(configItem);
                            configItem = null;
							configSubItem = -1;
						}

                    rc = old;
                    rc.height = 40*item.behaviors.Length;
                    rc.width += 20;

                    GUI.backgroundColor = oldColor;
					if( hoverItem != null && hoverItem != item && (hoverItem.kind == TimeLineItem.Kind.Single || (hoverItem.kind == TimeLineItem.Kind.Multiple && subItem != -1 ) ) )
					{
						if( idSelected != -1 || selectedTimeLine == tl )
						{
		                    dropZone.y = bottom-15;
		                    dropZone.x += 40;
		                    dropZone.width -= 60;
		                    dropZone.height = 20;
		                    if( selectedDropZone == dropZone )
		                        GUI.backgroundColor = Color.red;
		                    GUI.backgroundColor = GUI.backgroundColor.Darken(.5f);
		                    GUI.Label( dropZone, "Drop to add", dropStyle );
						}
					}
                    GUI.backgroundColor = oldColor;
                    GUILayout.EndVertical();
                }
                else
                {
                    GUI.backgroundColor = oldColor;
                    GUILayout.EndVertical();
                }
            }

			if (Event.current.type == EventType.mouseDrag && hoverItem != null && selectedDropZone == dropZone && dropZone.Contains( Event.current.mousePosition) )
            {
                if( item.kind == TimeLineItem.Kind.Single )
                {
                    item.SwitchToMultiple();
                }
                if (hoverItem.kind == TimeLineItem.Kind.Multiple && subItem != -1)
                {
                    ArrayUtility.Add(ref item.behaviors, hoverItem.behaviors[subItem] );
                    ArrayUtility.RemoveAt(ref hoverItem.behaviors, subItem);
                    if (hoverItem.behaviors.Length == 1)
                        hoverItem.SwitchToSingle();
                }
                else
                {
                    ArrayUtility.Add(ref item.behaviors, hoverItem.behavior);
                    idSelected = ArrayUtility.IndexOf(tl.sequence, hoverItem);
					if( idSelected != -1 )
                    	tl.sequence[idSelected] = new TimeLineItem();
                }
                RemoveEmpty(timeLine);
                RemoveEmptyColumns(timeLine);
                MergeSyncros(timeLine);
				hoverItem = null;
				subItem=-1;
                hoverItem = item;
				if( hoverItem.kind == TimeLineItem.Kind.Multiple )
                	subItem = hoverItem.behaviors.Length-1;
                if (configItem != null)
                    NormalizeProbas(configItem);
                configItem = null;
				configSubItem = -1;					

            }
            if (Event.current.type == EventType.mouseDrag && hoverItem != null )
            {
                if( dropZone.Contains( Event.current.mousePosition ) && hoverItem.kind == TimeLineItem.Kind.Single && (hoverItem != item) )
                {
                    idSelected = ArrayUtility.IndexOf(tl.sequence, hoverItem);
                    int id = ArrayUtility.IndexOf(tl.sequence, item);
                    if( idSelected != -1 || selectedTimeLine == tl )
                        if( idSelected != id )
                            selectedDropZone = dropZone;
                }
                else if (dropZone.Contains(Event.current.mousePosition) && hoverItem.kind == TimeLineItem.Kind.Multiple && subItem != -1 && (hoverItem != item))
                {
                    idSelected = ArrayUtility.IndexOf(tl.sequence, hoverItem);
                    int id = ArrayUtility.IndexOf(tl.sequence, item);
                    if (idSelected != -1 || selectedTimeLine == tl)
                        if (idSelected != id)
                            selectedDropZone = dropZone;
                }
                if (!selectedDropZone.Contains(Event.current.mousePosition))
                    selectedDropZone = new Rect();
            }
            if (Event.current.type == EventType.mouseDrag && (hoverItem != item))
            {
				idSelected = ArrayUtility.IndexOf(tl.sequence, hoverItem);
				if (idSelected != -1 || selectedTimeLine == tl )
                {
                    Rect left = rc;
                    left.width /= 2;
                    Rect right = rc;
                    right.width /= 2;
                    right.x += right.width;
                    if (right.Contains(Event.current.mousePosition))
                    {
                        if (idSelected == -1 )
                        {
                            AddItem(timeLine, tl, hoverItem);
                            skipNext = true;
                            RemoveEmpty(timeLine);
                            RemoveEmptyColumns(timeLine);
                            MergeSyncros(timeLine);
                            idSelected = ArrayUtility.IndexOf(tl.sequence, hoverItem);
                        }
						if (subItem != -1&& hoverItem.kind == TimeLineItem.Kind.Multiple)
                        {
                            TimeLineItem single = new TimeLineItem( TimeLineItem.Kind.Single );
							single.behavior = new TimeLineBT(hoverItem.behaviors[subItem].uriBT);
							single.behavior.loopable = hoverItem.behaviors[subItem].loopable;
							single.behavior.interuptible = hoverItem.behaviors[subItem].interuptible;
                            ArrayUtility.RemoveAt(ref hoverItem.behaviors, subItem);
                            if (hoverItem.behaviors.Length == 1)
                                hoverItem.SwitchToSingle();

                            hoverItem = single;
                            subItem = -1;
                            AddItem(timeLine, tl, hoverItem);                            
                            RemoveEmpty(timeLine);
                            RemoveEmptyColumns(timeLine);
                            MergeSyncros(timeLine);
                            idSelected = ArrayUtility.IndexOf(tl.sequence, hoverItem);
                        }
						InsertRight(timeLine, tl, idSelected, i);

                        break;
                    }
                    if (left.Contains(Event.current.mousePosition))
                    {
                        if (idSelected == -1 )
                        {
                            AddItem(timeLine, tl, hoverItem);
                            skipNext = true;
                            RemoveEmpty(timeLine);
                            RemoveEmptyColumns(timeLine);
                            MergeSyncros(timeLine);
                            idSelected = ArrayUtility.IndexOf(tl.sequence, hoverItem);
                        }
						if (subItem != -1 && hoverItem.kind == TimeLineItem.Kind.Multiple)
						{
							TimeLineItem single = new TimeLineItem( TimeLineItem.Kind.Single );
							single.behavior = new TimeLineBT(hoverItem.behaviors[subItem].uriBT);
							single.behavior.loopable = hoverItem.behaviors[subItem].loopable;
							single.behavior.interuptible = hoverItem.behaviors[subItem].interuptible;

							ArrayUtility.RemoveAt(ref hoverItem.behaviors, subItem);
							if (hoverItem.behaviors.Length == 1)
								hoverItem.SwitchToSingle();
							
							hoverItem = single;
							subItem = -1;
							AddItem(timeLine, tl, hoverItem);                            
							RemoveEmpty(timeLine);
							RemoveEmptyColumns(timeLine);
							MergeSyncros(timeLine);
							idSelected = ArrayUtility.IndexOf(tl.sequence, hoverItem);
						}
                        InsertLeft(timeLine, tl, idSelected, i);
                        break;
                    }
                }
            }

        }
        GUILayout.EndHorizontal();
    }
}


