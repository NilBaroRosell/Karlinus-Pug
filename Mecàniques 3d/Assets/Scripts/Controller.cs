using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    CharacterController characterController;
    //public float jumpSpeed = 8.0f;

    public float speed = 6.0f;
    public float gravity = 20.0f;
    public Camera mainCamera;
    public float runMultiplier = 2f;
    public float croachMultiplier = 0.5f;
    public Animator anim;
    GameObject[] weaponState; //0 Hid 1 Shown

    private Vector3 camForward;
    private Vector3 camRight;
    private Vector3 moveDirection = Vector3.zero;

    public enum playerState { IDLE, HITTING, DYING, LIQUID };
    public playerState state;

    // Use this for initialization
    void Start () {
        characterController = GetComponent<CharacterController>();
        WeaponInitialize();
        state = playerState.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        Idle();
        Move();
    }

    void CamDirection()
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        //Don't need Y
        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }

    void WeaponInitialize()
    {
        anim.SetBool("Is_Draw", true);
        weaponState[1] = GameObject.Find("weapon_show");
        weaponState[0] = GameObject.Find("weapon_hide");
        weaponState[1].SetActive(false);
        weaponState[0].SetActive(true);
    }

    void Move()
    {
        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

            //Change relation to camera
            moveDirection = moveDirection.x * camRight + moveDirection.z * camForward;

            if (Input.GetButton("Crouch"))
            {
                Crouch();
            }
            else if (Input.GetButton("Run"))
            {
                Run();
            }
            else if(!(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)) Walk();
        }

        moveDirection.y -= gravity * Time.deltaTime;

        //Move relative to camera
        CamDirection();

        transform.LookAt(transform.position + new Vector3(moveDirection.x, 0, moveDirection.z));
        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
     
    }

    void Crouch()
    {
        moveDirection *= speed * croachMultiplier;
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) anim.SetBool("Is_Crouched_Idle", true);
        else anim.SetBool("Is_Crouching", true);
    }

    void Run()
    {
        moveDirection *= speed * runMultiplier;
        anim.SetBool("Is_Running", true);
    }

    void Walk()
    {
        moveDirection *= speed;
        anim.SetBool("Is_Walking", true);
    }

    void Idle()
    {
        anim.SetBool("Is_Walking", false);
        anim.SetBool("Is_Running", false);
        anim.SetBool("Is_Crouching", false);
        anim.SetBool("Is_Crouched_Idle", false);
        anim.SetBool("Is_Idle", true);
    }

    public void stepNoise(int message)
    {
        float dist;
        Vector3 enemyDist;
        if (message == 1) dist = 10;
        else dist = 30;
        for (int i = 0; i < EnemyManager.Enemies.Length; i++)
        {
            if (EnemyManager.Enemies[i].activeSelf && EnemyManager.Enemies[i].GetComponent<csAreaVision>().actualState != csAreaVision.enemyState.FIGHTING)
            {
                enemyDist = new Vector3(EnemyManager.Enemies[i].transform.position.x - rb.transform.position.x, 0.0f, EnemyManager.Enemies[i].transform.position.z - rb.transform.position.z);
                if (enemyDist.magnitude <= dist)
                {
                    EnemyManager.Enemies[i].GetComponent<csAreaVision>().actualState = csAreaVision.enemyState.SEARCHING;
                    EnemyManager.Enemies[i].GetComponent<csAreaVision>().lastState = csAreaVision.enemyState.FIGHTING;
                    EnemyManager.Enemies[i].GetComponent<csAreaVision>().lastSeenPosition = GameObject.Find("Jugador").transform.position;
                    EnemyManager.Enemies[i].GetComponent<csAreaVision>().speed = 50;
                }
            }
        }
    }

    public void Take_sword(int message)
    {
        if (anim.GetBool("Is_Damaging") == false && state != playerState.LIQUID)
        {
            if (anim.GetBool("Is_Detected"))
            {
                weaponState[1].SetActive(true);
                weaponState[0].SetActive(false);
            }
            else
            {
                weaponState[1].SetActive(false);
                weaponState[0].SetActive(true);
            }
        }
    }
}
