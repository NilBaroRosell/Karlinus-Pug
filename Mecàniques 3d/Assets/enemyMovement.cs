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
    private bool GoToA;
    private Vector3 vecEnemy1;
    private float vecAngle;
    private Vector3 rbDirection;
    private double timeTurnRef;
    

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        GoToA = true;
        vecAngle = 0.0f;
        anim.SetBool("Is_Walking", true);
        timeTurnRef = 0;
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude > speed) rb.velocity = rb.velocity.normalized * speed;
        if (GoToA)
        {
            rb.transform.LookAt(pointA);
            vecEnemy1 = new Vector3(pointA.x - rb.transform.position.x, 0.0f, pointA.z - rb.transform.position.z);
            if (vecEnemy1.magnitude < 1)
            {
                    //anim.SetBool("Is_Walking", false);                                                 
                    GoToA = false;                  
                    //anim.SetBool("Is_Walking", true);
                
                   
            }
            
            
                vecEnemy1.Normalize();
                rb.AddForce(vecEnemy1 * speed);
            
            
        }
        else
        {
            rb.transform.LookAt(pointB);
            vecEnemy1 = new Vector3(pointB.x - rb.transform.position.x, 0.0f, pointB.z - rb.transform.position.z);
            if (vecEnemy1.magnitude < 1)
                GoToA = true;
            vecEnemy1.Normalize();
            rb.AddForce(vecEnemy1 * speed);
        }
        Debug.Log(vecAngle);
    }
}
