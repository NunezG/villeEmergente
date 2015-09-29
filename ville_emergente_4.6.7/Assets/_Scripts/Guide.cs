using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using RAIN.Core;
using RAIN.Navigation.Targets;
using RAIN.Minds;
using RAIN.Serialization;
using RAIN.Motion;

//Classe principale pour le guide
public class Guide : MonoBehaviour
{
    public static GameObject[] allPointsOfView = null; // stockage de tous les points de vue
    public List<GameObject> targets = new List<GameObject>();// stockages des points de vue spécifiques à ce guide
    public GameObject inHandPosition, inHandObject;// respectiviment la position où le guide tiens les fragments qu'il transporte, et la référence sur le fragment en question

    public GameObject startingFragment; // le fragment avec lequel le guide commence
    public bool handsFull = false; // si le guide à quelque chose dans les mains

    public PointDeVue pdv; // le point de vue visé par ce guide
    public SceneRange scene;// la scène du guide
    public RAIN.Memory.BasicMemory tMemory; // mémoire RAIN

    public float timer = 0, endTimer = 5; // timer d'attente au point de vue

    public void Awake()
    {
        int matriculeGuide = int.Parse(this.gameObject.name.Substring(5)); // récupération du matricule X du guide : "GuideX"


        if (allPointsOfView == null)
            allPointsOfView = GameObject.FindGameObjectsWithTag("PointDeVue");// récupération de tous les points de vue si ce n'est pas déjà fait
        foreach (GameObject gObject in allPointsOfView)
        {
            if (gObject.name == "PointDeVue" + matriculeGuide)
            {
                targets.Add(gObject);// récupération parmi tous les points de vue de ceux ayant le même matricule que le guide
            }
        }
    }


    void Start()
    {
        AIRig aiRig = GetComponentInChildren<AIRig>(); // récupération du composant RAIN
        tMemory = aiRig.AI.WorkingMemory as RAIN.Memory.BasicMemory; // récupération de la mémoire RAIN
        startingFragment.GetComponent<levitation>().IsHeld(true); // désactivation de la lévitation du fragment avec lequel le guide commence
		startingFragment.GetComponent<InteractibleObject> ().OnTouch ();//rammassage du fragment de départ du guide
        PickUpObject(startingFragment);//idem
    }

    void Update()
    {
        if (tMemory.GetItem<bool>("destinationReached"))// gestion du timer d'attente au point de vue et des variables RAIN correspondantes
        {
            timer = timer + Time.deltaTime;
        }
        else if (timer != 0)
        {
            timer = 0;
        }
        if (timer > endTimer)
        {
            tMemory.SetItem<bool>("timerIsOver", true);
        }
    }


    public void InteractWithBuildingBis()
    {
        if (inHandObject == null ) //  interaction du guide au main vide avec un batiment
        {
            PickUpObject(pdv.batimentAVisiter.OnTouch());
        }
        else if (inHandObject != null && pdv.batimentAVisiter.fragment != null) // echange de fragments entre le guide et le batiment
        {
            Fragment inHandFragment = inHandObject.GetComponent<Fragment>(); // recuperer le fragment en main
            GameObject inBuildingFragment = pdv.batimentAVisiter.OnPickUp(); // recuperer le fragment du building
            pdv.batimentAVisiter.OnAddingFragment(inHandFragment); // ajouter le fragment tenu par le guide au batiment
            AddingFragment(); // détacher le fragment du guide
            inHandFragment.gameObject.SetActive(false); // désactiver le fragment
            PickUpObject(inBuildingFragment); // ramasser le fragment du batiment

            tMemory.SetItem<bool>("hasFragment", true); // setting de variable RAIN

        }
        else if (inHandObject != null && pdv.batimentAVisiter.fragment == null) // ajout de fragment au batiment
        {
            Fragment fragment = inHandObject.GetComponent<Fragment>();
            pdv.batimentAVisiter.OnAddingFragment(fragment);
            AddingFragment();
            fragment.gameObject.SetActive(false);

            tMemory.SetItem<bool>("hasFragment", false);
        }

    }

    //Methode correspondant au dépôt d'un fragment par le guide
    public void DropInHandObject()
    {
        if (inHandObject != null)
        {
            inHandObject.GetComponent<Fragment>().Drop(); // appel de la fonction correspondante sur le fragment
            handsFull = false; // setting des variables du guide correspondante
            inHandObject = null; // idem
        }
    }
    //Récupération de l'objet "toPickUp" par le guide 
    public void PickUpObject(GameObject toPickUp)
    {
        if (toPickUp != null)
        {
            tMemory.SetItem<bool>("hasFragment", true);// setting de variable RAIN
            inHandObject = toPickUp; // setting de variable du GUide
            handsFull = true;
            inHandObject.transform.parent = inHandPosition.transform; // on change la place de l'objet dans la hiérarchie afin qu'il suive le guide
            inHandObject.transform.position = inHandPosition.transform.position;
            inHandObject.transform.rotation = inHandPosition.transform.rotation;
            inHandObject.rigidbody.isKinematic = true;
        }
    }
    // quand un guide dépose un fragment sur un batiment
    public void AddingFragment()
    {
        tMemory.SetItem<bool>("hasFragment", false); // on set la variable RAIN 
        inHandObject.rigidbody.isKinematic = false; // 
        inHandObject.transform.parent = null; // on sort le fragment de sa dépendance à l'objet inHandPosition 
        DropInHandObject();
    }
}
