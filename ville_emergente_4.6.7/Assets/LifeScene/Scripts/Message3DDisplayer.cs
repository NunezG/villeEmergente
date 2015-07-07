using UnityEngine;
using System.Collections;

public class Message3DDisplayer : MonoBehaviour {
	
	//string msg = "none";
	//float maxDist = 400;
	/*Ray ray = new Ray();
	Rect rc = new Rect();
    private Camera[] cameras;
    private Camera CurrentCam;*/

	void Start()
	{

	}
	/*void OnGUI()
	{
		if( msg != "none"  )
		{
			//Debug.Log("Message");
                cameras = Camera.allCameras;
                for (int i = 0; i < cameras.Length; i++)
                    if (cameras[i].enabled)
                        CurrentCam = cameras[i];
                Vector3 start = CurrentCam.transform.position + CurrentCam.transform.forward * 0.2f;
			    ray.origin = start;
			    ray.direction = transform.position-start;
			    //float maxLen = (transform.position-start).magnitude;
			    //if( maxLen < maxDist && !Physics.Raycast( ray, out hit, maxLen ) )
			    //{
				Vector2 size = GUI.skin.GetStyle("Box").CalcSize(new GUIContent(msg));
                Vector3 v = CurrentCam.WorldToScreenPoint(transform.position);
				rc.x = v.x-size.x/2;
				rc.y = Screen.height-v.y-size.y;
				rc.height = size.y;
				rc.width = size.x;
				
				GUI.Box( rc, msg );
			//}
		}
	}*/

	public void SetMessage( string mesg )
	{
		//msg = mesg;
	}
}