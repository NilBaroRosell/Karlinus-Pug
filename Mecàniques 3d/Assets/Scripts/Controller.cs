using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    CharacterController characterController;
    //public float jumpSpeed = 8.0f;

    //Changeable variables
    public float speed = 6.0f;
    public float gravity = 20.0f;
    public float runMultiplier = 2f;
    public float croachMultiplier = 0.5f;

    //Object references
    public Camera mainCamera;
    public Animator anim;
    public GameObject hidratationStates;

    public GameObject[] weaponState; //0 Hid 1 Shown


    //Variables
    private Vector3 camForward;
    private Vector3 camRight;
    private Vector3 moveDirection = Vector3.zero;
    public bool usingGravity = true;
    private bool dashing = false;
    public float dashMultiplier = 3f;
    public float dashTime = 3.0f;

    //Nil Variables
    public bool hitting;
    public float startDash;
    public float finishDash;
    private Vector3 dashDistance;
    public float distanceDash;
    public bool activateDash = true;
    public string direction;
    public Vector3 vectorDirection;
    public float moveHorizontal;
    public float moveVertical;
    private bool cooldown = false;
    private liquidState checkCooldown;
    public bool LiquidState = false;
    public float startDie;
    public float finishDie;

    //States
    public enum playerState { IDLE, HITTING, DYING, LIQUID };
    public playerState state;

    // Use this for initialization
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        state = playerState.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case playerState.IDLE:
                {
                    Idle();
                    hidratationStates.SetActive(true);
                    hitting = false;
                    Move();

                    checkCooldown = GetComponent<liquidState>();
                    cooldown = checkCooldown.cooldown;
                    if (Input.GetKeyDown(KeyCode.Q) && !cooldown && GameObject.Find("Jugador").GetComponent<liquidState>().hidratation >= 0)
                    {
                        //GetComponent<Collider>().enabled = false;
                        LiquidState = true;
                        state = playerState.LIQUID;
                    }



                    if (anim.GetBool("Is_Dying") == true)
                    {
                        state = playerState.DYING;
                        startDie = Time.frameCount;
                    }
                    break;
                }
            case playerState.HITTING:
                hitting = true;
                hidratationStates.SetActive(false);
                anim.SetBool("Is_Running", false);
                anim.SetBool("Is_Walking", false);
                anim.SetBool("Is_Crouching", false);
                anim.SetBool("Is_Crouched_Idle", false);
                anim.SetBool("Is_Idle", true);
                break;
            case playerState.DYING:
                {
                    finishDie = Time.frameCount;
                    if ((finishDie - startDie) > 250)
                    {
                        //loadScreen.Instancia.CargarEscena("DEAD");
                    }
                    break;
                }
            case playerState.LIQUID:
                {
                    hidratationStates.SetActive(true);
                    hitting = false;
                    Move();
                    if (!LiquidState)
                    {
                        GetComponent<Collider>().enabled = true;
                        state = playerState.IDLE;
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
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

    void Move()
    {
        if (dashing)
        {
            speed = Mathf.Lerp(speed, 6.0f, dashTime * Time.deltaTime);
            if (speed <= 6.1f) dashing = false;
        }
        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

            //Change relation to camera
            moveDirection = moveDirection.x * camRight + moveDirection.z * camForward;

            if(state == playerState.LIQUID) moveDirection *= speed * runMultiplier;
            else if (Input.GetButton("Crouch")) Crouch();
            else if (Input.GetButton("Run")) Run();
            else if (!(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)) Walk();
            if (Input.GetButton("Dash") && !dashing) Dash();
        }

       if(usingGravity) moveDirection.y -= gravity * Time.deltaTime;

        //Move relative to camera
        CamDirection();

        transform.LookAt(transform.position + new Vector3(moveDirection.x, 0, moveDirection.z));
        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

    }

    void Crouch()
    {
        moveDirection *= speed * croachMultiplier;
        anim.SetBool("Is_Idle", false);
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) anim.SetBool("Is_Crouched_Idle", true);
        else anim.SetBool("Is_Crouching", true);
    }

    void Run()
    {
        moveDirection *= speed * runMultiplier;
        if (moveDirection != Vector3.zero) anim.SetBool("Is_Running", true);
        anim.SetBool("Is_Idle", false);
    }

    void Dash()
    {
        dashing = true;
        speed *= dashMultiplier;
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
        if (state == playerState.LIQUID) return;
        float dist;
        Vector3 enemyDist;
        if (message == 1) dist = 10;
        else dist = 30;
        if (GameObject.Find("EnemyManager") != null)
        {
            for (int i = 0; i < EnemyManager.Enemies.Length; i++)
            {
                if (EnemyManager.Enemies[i].activeSelf && EnemyManager.Enemies[i].GetComponent<csAreaVision>().actualState != csAreaVision.enemyState.FIGHTING)
                {
                    enemyDist = new Vector3(EnemyManager.Enemies[i].transform.position.x - transform.position.x, 0.0f, EnemyManager.Enemies[i].transform.position.z - transform.position.z);
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
        else if(GameObject.Find("batalla final") != null)
        {
            GameObject[] Bosses = GameObject.Find("batalla final").GetComponent<finalBattleManager>().Bosses;
            for (int i = 0; i < Bosses.Length; i++)
            {
                if (Bosses[i].activeSelf && Bosses[i].GetComponent<bossIA>().actualState != bossIA.enemyState.FIGHTING)
                {
                    enemyDist = new Vector3(Bosses[i].transform.position.x - transform.position.x, 0.0f, Bosses[i].transform.position.z - transform.position.z);
                    if (enemyDist.magnitude <= dist) Bosses[i].GetComponent<bossIA>().noiseDetected();
                }
            }
        }
    }

    public void finishAnim(int message)
    {
        if (message == 1)
        {
            kill_cono_vision.returnPlayer = true;
        }

    }


    public void liquidTransformation()
    {
        LiquidState = true;
        state = playerState.LIQUID;
    }
}