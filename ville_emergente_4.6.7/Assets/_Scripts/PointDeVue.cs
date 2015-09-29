using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// Classe composant pour représenter les points de vue sur lesquels se rendent les guides et les passants qui les suivent
public class PointDeVue : MonoBehaviour {



    public ConvolutionObject batimentAVisiter;// le bâtiment lié au point de vue
    public List<GameObject> spots = new List<GameObject>(); // les places pour les passants 
    public List<bool> availablesSpots = new List<bool>(); // liste de la disponibilité des places
    public bool isBeingVisited = false; // boolean indiquant si un guide est en train de se diriger ou d'occuper le point de vue 

	// Use this for initialization
	void Start () {
        foreach (Transform child in transform)
        {
            //récupération des spots dans les enfants
            if (child.gameObject.name == "spot")
            {
                spots.Add(child.gameObject);
                availablesSpots.Add(true);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
