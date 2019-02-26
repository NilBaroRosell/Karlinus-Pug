using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class NPC : MonoBehaviour
{
    private Transform npcTransform;
    public Vector3 destinationPoint;
    private Vector3 vecToDestination;
    public Rigidbody rb;
    NavMeshAgent npcAgent;
    private Animator anim;
    public Vector3 stuckPos;

    // Use this for initialization
    void Awake()
    {
        npcAgent = this.GetComponent<NavMeshAgent>();
        npcTransform = this.GetComponent<Transform>();
        rb = this.GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        anim.SetBool("Is_Walking", true);
        destinationPoint = new Vector3(gameObject.transform.position.x + 3, gameObject.transform.position.y, gameObject.transform.position.z + 3);
    }

    void Start()
    {
        StartCoroutine(CheckStuck(1));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        vecToDestination = new Vector3(destinationPoint.x - npcTransform.position.x, 0.0f, destinationPoint.z - npcTransform.transform.position.z);
        if (vecToDestination.magnitude < 1)
        {
            anim.SetBool("Is_Walking", false);
            anim.SetBool("Is_Idle", true);
            getDestination();
        }
        else
        {
            npcAgent.SetDestination(destinationPoint);
            gameObject.transform.LookAt(gameObject.transform.forward);
            npcAgent.speed = 1;
            anim.SetBool("Is_Walking", true);
            anim.SetBool("Is_Idle", false);
        }
    }

    private void getDestination()
    {
        destinationPoint = this.gameObject.GetComponent<RandomDestination>().RandomNavmeshLocation(this.gameObject, 20);
    }

    IEnumerator CheckStuck(float time)
    {
        yield return new WaitForSeconds(time);

        stuckPos = transform.position;
        stuckPos = new Vector3(stuckPos.x - rb.transform.position.x, 0.0f, stuckPos.z - rb.transform.position.z);
        if (stuckPos.magnitude < 2) getDestination();
        StartCoroutine(CheckStuck(1));
    }
}