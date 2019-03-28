using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;
using System;

public class ServantM3 : MonoBehaviour
{

    private GameObject point;

    private Vector3 destinationPoint;

    NavMeshAgent servantAgent;
    private Animator anim;

    private void Awake()
    {
        point = gameObject.transform.GetChild(2).gameObject;
        servantAgent = this.GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        destinationPoint = new Vector3 (point.transform.position.x, gameObject.transform.position.y, point.transform.position.z);
    }

    private void Start()
    {
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        anim.SetBool("Backward", false);
        anim.SetBool("Idle", false);
        anim.SetBool("Run", false);
        anim.SetBool("Walk", true);
        servantAgent.speed = 1;
        servantAgent.SetDestination(destinationPoint);
        gameObject.transform.LookAt(destinationPoint);
    }
}