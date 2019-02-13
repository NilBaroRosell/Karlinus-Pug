using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;
using System;
public class FollowKarlinus : MonoBehaviour {

    private GameObject karlinus;
    private misions misionsScript;
    private Rigidbody rb;

    private Vector3 destinationPoint;
    private Vector3 vecToDestination;

    NavMeshAgent ratHoodAgent;
    private Animator anim;

    // Use this for initialization
    void Start () {
        if (GameObject.Find("Jugador") != null) karlinus = GameObject.Find("Jugador");
        if (GameObject.Find("Misiones") != null) misionsScript = GameObject.Find("Misiones").GetComponent<misions>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        anim.SetBool("Is_Walking", true);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(misionsScript.ActualMision == misions.Misions.SM_1)
        {
            switch(misionsScript.indexMision)
            {
                case 3:
                    destinationPoint = karlinus.transform.position - (gameObject.transform.forward * 3)
                    if ((karlinus.transform.position - gameObject.transform.position).magnitude < 4)
                    {
                        if (!anim.GetBool("Is_Running")) anim.SetBool("Is_Running", true);
                        if (GetComponent<NavMeshObstacle>().enabled == false)
                        {
                            ratHoodAgent.SetDestination(destinationPoint);
                            ratHoodAgent.speed = 5;
                        }
                        else
                        {
                            anim.SetBool("Is_Running", false);
                        }
                    }
                    break;
                case 4:
                    destinationPoint = karlinus.transform.position - (gameObject.transform.forward * 3);
                    if (karlinus != null && (karlinus.transform.position - gameObject.transform.position).magnitude < 4)
                    {
                        if (!anim.GetBool("Is_Running")) anim.SetBool("Is_Running", true);
                        if (GetComponent<NavMeshObstacle>().enabled == false)
                        {
                            ratHoodAgent.SetDestination(destinationPoint);
                            ratHoodAgent.speed = 5;
                        }
                        else
                        {
                            anim.SetBool("Is_Running", false);
                        }
                    }
                    break;
                case 5:
                    destinationPoint = new Vector3(21.55f, 8.019f, 62.6f);
                    vecToDestination = new Vector3(destinationPoint.x - gameObject.transform.position.x, 0.0f, destinationPoint.z - gameObject.transform.position.z);
                    if ((gameObject.transform.position - new Vector3(44.4f, 8.019f, 89.496f)).magnitude > 1) // posicio font
                    {
                        if (vecToDestination.magnitude > 1) // posicio en la que el jugador no vegi a Rat hood
                        {
                            if (!anim.GetBool("Is_Running")) anim.SetBool("Is_Running", true);
                            if (GetComponent<NavMeshObstacle>().enabled == false)
                            {
                                ratHoodAgent.SetDestination(destinationPoint);
                                ratHoodAgent.speed = 5;
                            }
                            else
                            {
                                anim.SetBool("Is_Running", false);
                            }
                            //correr cap a (21.55f, 8.019f, 62.6f)
                        }
                        else
                        {
                            gameObject.transform.position = new Vector3(44.4f, 8.019f, 89.496f);// posicio font
                            gameObject.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);  //rotacio font
                        }
                    }
                    
                    break;
                default:
                    break;
            }
            
        }
        
	}
}
