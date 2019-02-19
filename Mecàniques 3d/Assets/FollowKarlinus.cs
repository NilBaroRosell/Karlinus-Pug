using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;
using System;
public class FollowKarlinus : MonoBehaviour {

    private GameObject karlinus;
    private Transform ratHoodTransform;
    private misions misionsScript;
    private Rigidbody rb;

    private Vector3 destinationPoint;
    private Vector3 vecToDestination;

    NavMeshAgent ratHoodAgent;
    private Animator anim;
    public bool entra = false;

    private void Awake()
    {
        ratHoodAgent = this.GetComponent<NavMeshAgent>();
        ratHoodTransform = this.GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
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
        
        if(misionsScript.ActualMision == misions.Misions.SM_1)
        {
            switch(misionsScript.indexMision)
            {
                case 6:
                    anim.SetBool("Is_Idle", false);
                    destinationPoint = karlinus.transform.position;
                    if ((destinationPoint - gameObject.transform.position).magnitude > 2)
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
                    else entra = true;
                    break;
                case 9:
                    anim.SetBool("Is_Running", false);
                    anim.SetBool("Is_Idle", true);
                    gameObject.transform.LookAt(karlinus.transform.position);
                    break;
                case 11:
                    anim.SetBool("Is_Idle", false);
                    entra = false;
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
                case 12:
                    anim.SetBool("Is_Running", false);
                    anim.SetBool("Is_Idle", true);
                    gameObject.transform.LookAt(karlinus.transform.position);
                    break;
                case 13:
                    anim.SetBool("Is_Running", false);
                    anim.SetBool("Is_Idle", true);
                    gameObject.transform.LookAt(karlinus.transform.position);
                    break;
                case 14:
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
