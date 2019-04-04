using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class NPC : MonoBehaviour
{
    public Vector3 destinationPoint;
    private Vector3 vecToDestination;
    NavMeshAgent npcAgent;
    private Animator anim;
    public Vector3 stuckPos;

    // Use this for initialization
    void Awake()
    {
        npcAgent = this.GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        destinationPoint = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y, gameObject.transform.position.z + 1);
    }

    void Start()
    {
        StartCoroutine(CheckStuck(1));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        vecToDestination = new Vector3(destinationPoint.x - transform.position.x, 0.0f, destinationPoint.z - transform.transform.position.z);
        if (vecToDestination.magnitude < 1 || destinationPoint.x > 100 || destinationPoint.x < -90 || destinationPoint.z > 250 || destinationPoint.z < -50)
        {
            getDestination();
        }
        else
        {
            npcAgent.SetDestination(destinationPoint);
            //gameObject.transform.LookAt(gameObject.transform.forward);
            npcAgent.speed = 1;
        }
    }

    private void getDestination()
    {
        destinationPoint = this.gameObject.transform.GetComponent<RandomDestination>().RandomNavmeshLocationNPC(this.gameObject, 50);
    }

    IEnumerator CheckStuck(float time)
    {
        yield return new WaitForSeconds(time);

        stuckPos = new Vector3(stuckPos.x - transform.position.x, 0.0f, stuckPos.z - transform.position.z);
        if (stuckPos.magnitude < 0.5f) getDestination();
        stuckPos = transform.position;
        StartCoroutine(CheckStuck(1));
    }
}