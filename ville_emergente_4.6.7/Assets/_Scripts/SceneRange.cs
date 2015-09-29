using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// vague imitation des scènes de vie de masa
public class SceneRange : MonoBehaviour {

    public GameObject mainActor; // l'acteur principale de la scène
    public List<GameObject> spots = new List<GameObject>(); // les places pour les spectateurs de la scène

    public List<bool> availablesSpots = new List<bool>(); // liste des places disponibles
    public List<Passant> passantsInRange = new List<Passant>(); // liste des passants à portée de la scène

	void Start () {
        foreach (Transform child in transform) // remplissage de la scène
        {
            if (child.gameObject.name == "spot")
            {
                spots.Add(child.gameObject);
                availablesSpots.Add(true);
            }
        }
	}
	
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "NPC" && other.GetComponent<Passant>()!=null  && IsThereAvailableSpot())
        {
            Passant passant = other.GetComponent<Passant>(); // ajout des passants à la liste des passants à portée 
            passantsInRange.Add(passant);
            if (passant != null)
            {
                passant.SetInRangeOfScene(true); // ajout de la scène à la liste des scènes disponible du passant
                passant.availableScenes.Add(this);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "NPC" && other.GetComponent<Passant>() != null)
        {
            Passant passant = other.GetComponent<Passant>();
            passantsInRange.Remove(passant); // retrait des passants de la liste des passants à portée 
            if (passant != null)
            {
                passant.availableScenes.Remove(this);// retrait de la scène de la liste des passants
                passant.SetInRangeOfScene(false);
            }
        }
    }
    //Désactivation de la scène ( pour le Guide )
    public void DeactivateScene()
    {
        for (int i = 0; i < passantsInRange.Count; i++) // retrait de la scène désactivée des listes de passants
        {
            passantsInRange[i].availableScenes.Remove(this);
            passantsInRange[i].SetInRangeOfScene(false);
        }
        passantsInRange = new List<Passant>(); // retrait de tout les passants de la liste des passants à portée
        this.gameObject.SetActive(false);
    }

    // retourne vrai si il y a une place de libre de la scène
    public bool IsThereAvailableSpot()
    {
        for (int i = 0; i < availablesSpots.Count; i++)
        {
            if (availablesSpots[i])
                return true;
        }
        return false;
    }

}
