using UnityEngine;
using System.Collections;

public class InterestPointGizmo : MonoBehaviour {

    void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position, "wayPoint.png", true);
    }
}
