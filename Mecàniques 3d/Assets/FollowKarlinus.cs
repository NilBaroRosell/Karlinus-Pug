using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;
using System;
public class FollowKarlinus : MonoBehaviour {

    private GameObject karlinus;
    private Transform ratHoodTransform;
    private misions misionsScript;

    private Vector3 destinationPoint;
    private Vector3 vecToDestination;

    NavMeshAgent ratHoodAgent;
    private Animator anim;

    public bool firstPoint = false;

    private void Awake()
    {
        ratHoodAgent = this.GetComponent<NavMeshAgent>();
        ratHoodTransform = this.GetComponent<Transform>();
        anim = GetComponent<Animator>();
        anim.SetBool("Is_Walking", true);
    }

    // Use this for initialization
    void Start () {
        if (GameObject.Find("Jugador") != null) karlinus = GameObject.Find("Jugador");
        if (GameObject.Find("Misiones") != null) misionsScript = GameObject.Find("Misiones").GetComponent<misions>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (misionsScript.ActualMision == misions.Misions.SM_1)
        {
            switch(misions.misionIndex)
            {
                case 6:
                    if (GameObject.Find("Zone Controll SM1") != null) GameObject.Find("Zone Controll SM1").transform.position = gameObject.transform.position;
                    anim.SetBool("Is_Idle", false);
                    if(!firstPoint) destinationPoint = new Vector3(32.56f, -27.33f, -29.2f);
                    else destinationPoint = new Vector3(-83.95f, -27.33f, -46.68f);
                    vecToDestination = new Vector3(destinationPoint.x - ratHoodTransform.position.x, 0.0f, destinationPoint.z - ratHoodTransform.transform.position.z);
                    if (vecToDestination.magnitude > 2 && !firstPoint)
                    {
                        if (!anim.GetBool("Is_Running")) anim.SetBool("Is_Running", true);
                        if (GetComponent<NavMeshObstacle>().enabled == false)
                        {
                            ratHoodAgent.SetDestination(destinationPoint);
                            gameObject.transform.LookAt(destinationPoint);
                            ratHoodAgent.speed = 7;
                        }
                        else
                        {
                            anim.SetBool("Is_Running", false);
                        }
                    }
                    else firstPoint = true;

                    if (vecToDestination.magnitude > 2 && firstPoint)
                    {
                        if (!anim.GetBool("Is_Running")) anim.SetBool("Is_Running", true);
                        if (GetComponent<NavMeshObstacle>().enabled == false)
                        {
                            ratHoodAgent.SetDestination(destinationPoint);
                            gameObject.transform.LookAt(destinationPoint);
                            ratHoodAgent.speed = 7;
                        }
                        else
                        {
                            anim.SetBool("Is_Running", false);
                        }
                    }
                    break;
                case 8:
                    if (GameObject.Find("Zone Controll SM1") != null) GameObject.Find("Zone Controll SM1").transform.position = gameObject.transform.position;
                    anim.SetBool("Is_Idle", false);
                    destinationPoint = new Vector3 (-58.87f,-9.1f,112.47f);
                    vecToDestination = new Vector3(destinationPoint.x - ratHoodTransform.position.x, 0.0f, destinationPoint.z - ratHoodTransform.transform.position.z);
                    if (vecToDestination.magnitude > 2)
                    {
                        if (!anim.GetBool("Is_Running")) anim.SetBool("Is_Running", true);
                        if (GetComponent<NavMeshObstacle>().enabled == false)
                        {
                            ratHoodAgent.SetDestination(destinationPoint);
                            gameObject.transform.LookAt(destinationPoint);
                            ratHoodAgent.speed = 5;
                        }
                        else
                        {
                            anim.SetBool("Is_Running", false);
                        }
                    }
                    else
                    {
                        anim.SetBool("Is_Running", false);
                        anim.SetBool("Is_Idle", true);
                        gameObject.transform.LookAt(destinationPoint);
                    }
                    break;
                case 9:
                    if (GameObject.Find("Zone Controll SM1") != null) GameObject.Find("Zone Controll SM1").transform.position = karlinus.transform.position;
                    anim.SetBool("Is_Running", false);
                    anim.SetBool("Is_Idle", true);
                    gameObject.transform.LookAt(karlinus.transform.position);
                    break;
                case 12:
                    if (GameObject.Find("Zone Controll SM1") != null) GameObject.Find("Zone Controll SM1").transform.position = karlinus.transform.position;
                    anim.SetBool("Is_Idle", false);
                    destinationPoint = new Vector3(-37f, -9.1f, 95);
                    vecToDestination = new Vector3(destinationPoint.x - gameObject.transform.position.x, 0.0f, destinationPoint.z - gameObject.transform.position.z);
                    if ((gameObject.transform.position - new Vector3(14.32f, -9.1f, 150.9f)).magnitude > 1) // posicio font
                    {
                        if (vecToDestination.magnitude > 1) // posicio en la que el jugador no vegi a Rat hood
                        {
                            if (!anim.GetBool("Is_Running")) anim.SetBool("Is_Running", true);
                            if (GetComponent<NavMeshObstacle>().enabled == false)
                            {
                                ratHoodAgent.SetDestination(destinationPoint);
                                gameObject.transform.LookAt(destinationPoint);
                                ratHoodAgent.speed = 5;
                            }
                            else
                            {
                                anim.SetBool("Is_Running", false);
                            }
                        }
                    }
                    break;
                case 13:
                    if (GameObject.Find("Zone Controll SM1") != null) GameObject.Find("Zone Controll SM1").transform.position = karlinus.transform.position;
                    anim.SetBool("Is_Running", false);
                    anim.SetBool("Is_Idle", true);
                    gameObject.transform.LookAt(karlinus.transform.position);
                    break;
                case 14:
                    if (GameObject.Find("Zone Controll SM1") != null) GameObject.Find("Zone Controll SM1").transform.position = karlinus.transform.position;
                    anim.SetBool("Is_Running", false);
                    anim.SetBool("Is_Idle", true);
                    gameObject.transform.LookAt(karlinus.transform.position);
                    break;
                case 15:
                    if (GameObject.Find("Zone Controll SM1") != null) GameObject.Find("Zone Controll SM1").transform.position = karlinus.transform.position;
                    anim.SetBool("Is_Running", false);
                    anim.SetBool("Is_Idle", true);
                    gameObject.transform.LookAt(karlinus.transform.position);
                    break;
                default:
                    break;
            }
            
        }
        
	}

    void OnLevelWasLoaded()
    {
            Start();
    }
}
