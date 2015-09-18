using UnityEngine;
using System.Collections;

public class CursorManager : MonoBehaviour {

	public Sprite normalCursor;
	public Sprite interactibleCursor;
	public Sprite failCursor;
	public Sprite fragmentCursor;



	// Use this for initialization
	void Start () {
	


		//GetComponent<UnityEngine.UI.Image> ().sprite = NPCCursor;




	}

	public void setNormalCursor()
	{
		GetComponent<UnityEngine.UI.Image> ().sprite = normalCursor;

	}

	public void setInteractibleCursor()
	{
		GetComponent<UnityEngine.UI.Image> ().sprite = interactibleCursor;
		
	}

	public void setFailCursor()
	{
		GetComponent<UnityEngine.UI.Image> ().sprite = failCursor;
		
	}

	public void setFragmentCursor()
	{
		GetComponent<UnityEngine.UI.Image> ().sprite = fragmentCursor;
		
	}


	// Update is called once per frame
	void Update () {



	
	}
}
