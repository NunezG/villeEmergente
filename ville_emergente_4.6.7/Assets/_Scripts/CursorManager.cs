using UnityEngine;
using System.Collections;

public class CursorManager : MonoBehaviour {

	public Sprite normalCursor;
	public Sprite interactibleCursor;
	public Sprite failCursor;
	public Sprite fragmentCursor;
	public CursorType cursorT = CursorType.normal;

	/*
	 * Setters pour le curseur intéractif
	 */
	public void setNormalCursor()
	{
		cursorT = CursorType.normal;
		GetComponent<UnityEngine.UI.Image> ().sprite = normalCursor;

	}

	public void setInteractibleCursor()
	{
		cursorT = CursorType.interactible;
		GetComponent<UnityEngine.UI.Image> ().sprite = interactibleCursor;
	}

	public void setFailCursor()
	{
		cursorT = CursorType.fail;
		GetComponent<UnityEngine.UI.Image> ().sprite = failCursor;
		
	}

	public void setFragmentCursor()
	{
		cursorT = CursorType.fragment;
		GetComponent<UnityEngine.UI.Image> ().sprite = fragmentCursor;
	}

}
