using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    private Rigidbody rb;
    static Animator anim;
    public GameObject reference;

    public int speed;
    private Vector3 example = new Vector3(1.0f, 0.0f, 0.0f);

    //Patrol points and variables
    public Vector3 pointA;
    public Vector3 pointB;
    private Vector3 destinationPoint;
    private bool GoToA;
    private Vector3 vecEnemy1;
    private Vector3 rbDirection;
    private double timeTurnRef;
    private Vector3 playerDist;
    private bool patrolMode;
    

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        GoToA = true;
        anim.SetBool("Is_Walking", true);
        timeTurnRef = 0;
        destinationPoint = pointA;
        playerDist = new Vector3(GameObject.Find("Jugador").transform.position.x - rb.transform.position.x, 0.0f, GameObject.Find("Jugador").transform.position.z - rb.transform.position.z);
        patrolMode = true;

    }

    private void FixedUpdate()
    {
        
        playerDist = new Vector3(GameObject.Find("Jugador").transform.position.x - rb.transform.position.x, 0.0f, GameObject.Find("Jugador").transform.position.z - rb.transform.position.z);
        if (playerDist.magnitude < 40 && playerDist.magnitude > 20)
        {
            patrolMode = false;
            speed = 10;
            destinationPoint = GameObject.Find("Jugador").transform.position;
        }
        else if (playerDist.magnitude <= 20)
        {
            patrolMode = false;
            speed = 20;
            destinationPoint = GameObject.Find("Jugador").transform.position;
        }
        else if (patrolMode == false)
        {
            speed = 10;
            destinationPoint = pointA;
            patrolMode = true;
        }
        if (rb.velocity.magnitude > speed) rb.velocity = rb.velocity.normalized * speed;
            rb.transform.LookAt(destinationPoint);
            vecEnemy1 = new Vector3(destinationPoint.x - rb.transform.position.x, 0.0f, destinationPoint.z - rb.transform.position.z);
            if (vecEnemy1.magnitude < 1 && GoToA)
            {
                //anim.SetBool("Is_Walking", false);                                                 
                //GoToA = false;                  
                //anim.SetBool("Is_Walking", true);
                destinationPoint = pointB;
                    GoToA = false;
                Debug.Log("a por bee");
            }
            else if(vecEnemy1.magnitude < 1 && GoToA == false)
            {
                destinationPoint = pointA;
                GoToA = true;
            }
                vecEnemy1.Normalize();
                rb.AddForce(vecEnemy1 * speed);        
    }
}
