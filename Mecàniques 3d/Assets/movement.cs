using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class movement : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject camara;
    static Animator anim;
    //static Animator enemyAnim;
    GameObject weapon_show;
    GameObject weapon_hide;

    public static float speed;

    public bool onFloor = false;
    public bool jumping = false;
    public bool adPressed = false;
    public bool wsPressed = false;
    public static bool hitting;

    public string direction;
    public Vector3 vectorDirection;

    public float moveHorizontal;
    public float moveVertical;
    public float startHit = 0;
    public float finishHit = 0;
    public float startDash;
    public float finishDash;
    
    public float startDie;
    public float finishDie;
    public bool activateDash = true;
    public enum playerState { IDLE, HITTING, DYING, LIQUID };
    public playerState state;
    Collider w_collider;
    
    public static bool LiquidState = false;
    private bool cooldown = false;
    private liquidState checkCooldown;
    private bool onWater = false;
    public Vector3 rot;
    public float lastYPos;
    public Vector3 vel;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0, -250, 0);
        anim = GetComponent<Animator>();
        direction = "F";
        state = playerState.IDLE;
        anim.SetBool("Is_Draw", true);
        weapon_show = GameObject.Find("weapon_show");
        weapon_hide = GameObject.Find("weapon_hide");
        weapon_show.SetActive(false);
        weapon_hide.SetActive(true);
        Physics.IgnoreLayerCollision(9, 8);
        hitting = false;
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case playerState.IDLE:
                {
                    hitting = false;
                    vel = rb.velocity;
                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Sheathing Sword") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Withdrawing Sword"))
                    {
                        movePlayer();
                    }

                    finishDash = Time.frameCount;
                    if ((finishDash - startDash) > 80) activateDash = true;
                    if (Input.GetKeyDown(KeyCode.F) && activateDash)
                    {
                        if (direction == "L") vectorDirection = (moveHorizontal * transform.right).normalized * -10000;
                        else if (direction == "R") vectorDirection = (moveHorizontal * transform.right).normalized * 10000;
                        else if (direction == "B") vectorDirection = (moveVertical * transform.forward).normalized * -10000;
                        else if (direction == "F") vectorDirection = (moveVertical * transform.forward).normalized * 10000;
                        else if (direction == "FL" || direction == "FR") vectorDirection = ((moveVertical * transform.forward) + (moveHorizontal * transform.right)).normalized * 10000;
                        else if (direction == "BL" || direction == "BR") vectorDirection = ((moveVertical * transform.forward) + (moveHorizontal * transform.right)).normalized * -10000;
                        rb.velocity *= 0;
                        rb.AddForce(vectorDirection);
                        startDash = Time.frameCount;
                        activateDash = false;
                    }

                    checkCooldown = GetComponent<liquidState>();
                    cooldown = checkCooldown.cooldown;
                    if (Input.GetKeyDown(KeyCode.Q) && !cooldown && liquidState.hidratation >= 0)
                    {
                        rb.useGravity = false;
                        GetComponent<Collider>().enabled = false;
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
                anim.SetBool("Is_Running", false);
                anim.SetBool("Is_Walking", false);
                anim.SetBool("Is_Crouching", false);
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
                    hitting = false;
                    movePlayer();
                    if (!LiquidState)
                    {
                        rb.useGravity = true;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            jumping = false;
            onFloor = true;
        }
        if (collision.gameObject.tag == "water") onWater = true;
        else onWater = false;
    }

    public void Take_sword(int message)
    {
        if (anim.GetBool("Is_Damaging") == false && state != playerState.LIQUID)
        {
            if (anim.GetBool("Is_Detected"))
            {
                weapon_show.SetActive(true);
                weapon_hide.SetActive(false);
            }
            else
            {
                weapon_show.SetActive(false);
                weapon_hide.SetActive(true);
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

    public void stepNoise(int message)
    {
        float dist;
        Vector3 enemyDist;
        GameObject[] nearEnemies = GameObject.FindGameObjectsWithTag("enemy");
        if (message == 1) dist = 10;
        else dist = 30;
        for (int i = 0; i < nearEnemies.Length; i++)
        {
            if (nearEnemies[i].GetComponent<csAreaVision>().actualState != csAreaVision.enemyState.FIGHTING)
            {
                enemyDist = new Vector3(nearEnemies[i].transform.position.x - rb.transform.position.x, 0.0f, nearEnemies[i].transform.position.z - rb.transform.position.z);
                if (enemyDist.magnitude <= dist)
                {
                    nearEnemies[i].GetComponent<csAreaVision>().actualState = csAreaVision.enemyState.SEARCHING;
                    nearEnemies[i].GetComponent<csAreaVision>().lastState = csAreaVision.enemyState.FIGHTING;
                    nearEnemies[i].GetComponent<csAreaVision>().lastSeenPosition = GameObject.Find("Jugador").transform.position;
                    nearEnemies[i].GetComponent<csAreaVision>().speed = 50;
                }
            }
        }
    }

    public void movePlayer()
    {
        if (onFloor)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                wsPressed = true;
                anim.SetBool("Is_Walking", true);
                anim.SetBool("Is_Idle", false);
            }
            else
            {
                wsPressed = false;
                anim.SetBool("Is_Walking", false);
                anim.SetBool("Is_Idle", true);
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                adPressed = true;
                anim.SetBool("Is_Walking", true);
                anim.SetBool("Is_Idle", false);
            }
            else
            {
                adPressed = false;
                anim.SetBool("Is_Walking", true);
                anim.SetBool("Is_Idle", true);
            }

            if (adPressed || wsPressed)
            {
                transform.rotation = camara.transform.rotation;
                rot = camara.transform.rotation.eulerAngles;
                transform.Rotate(-rot.x, 0, -rot.z);
                speed = 9;
            }

            else speed = 0;

            if (LiquidState)
            {
                if (onWater) speed = 11;
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (adPressed || wsPressed)
                    {
                        anim.SetBool("Is_Running", true);
                        speed = 13;
                    }
                    else anim.SetBool("Is_Running", false);
                }
                else anim.SetBool("Is_Running", false);

                if (Input.GetKey(KeyCode.LeftControl))
                {
                     anim.SetBool("Is_Crouching", true);
                     speed = 6;
                }
                else anim.SetBool("Is_Crouching", false);

                if (Input.GetKeyUp(KeyCode.LeftShift)) anim.SetBool("Is_Running", false);
                if (Input.GetKeyUp(KeyCode.LeftControl)) anim.SetBool("Is_Crouching", false);
            }

            if (!wsPressed) moveVertical = 0;
            else moveVertical = Input.GetAxis("Vertical");

            if (!adPressed) moveHorizontal = 0;
            else moveHorizontal = Input.GetAxis("Horizontal");

            if (rb.velocity.magnitude > speed) rb.velocity = rb.velocity.normalized * speed;
            vectorDirection = ((moveVertical * transform.forward) + (moveHorizontal * transform.right));
            vectorDirection.Normalize();
            rb.velocity = vectorDirection * speed;

            if (moveVertical > 0 && moveHorizontal > 0)
            {
                transform.Rotate(0, 45, 0);
                direction = "FR";
            }
            else if (moveVertical > 0 && moveHorizontal < 0)
            {
                transform.Rotate(0, -45, 0);
                direction = "FL";
            }
            else if (moveVertical < 0 && moveHorizontal > 0)
            {
                transform.Rotate(0, 135, 0);
                direction = "BR";
            }
            else if (moveVertical < 0 && moveHorizontal < 0)
            {
                transform.Rotate(0, -135, 0);
                direction = "BL";
            }
            else if (moveVertical > 0)
            {
                direction = "F";
            }
            else if (moveVertical < 0)
            {
                transform.Rotate(0, 180, 0);
                direction = "B";
            }
            else if (moveHorizontal > 0)
            {
                transform.Rotate(0, 90, 0);
                direction = "R";
            }
            else if (moveHorizontal < 0)
            {
                transform.Rotate(0, -90, 0);
                direction = "L";
            }
            else
            {
                transform.Rotate(0, rot.y - lastYPos, 0);
                direction = "I";
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                transform.position = new Vector3(50.89f, -6.778945f, 37.37239f);
                camara.transform.position = new Vector3(50.811f, -3.91f, 31.41f);
            }

            if (moveVertical == 0 && moveHorizontal == 0)
            {
                switch (direction)
                {
                    case "F":
                        break;
                    case "B":
                        transform.Rotate(0, 180, 0);
                        break;
                    case "L":
                        transform.Rotate(0, -90, 0);
                        break;
                    case "R":
                        transform.Rotate(0, 90, 0);
                        break;
                    case "FL":
                        transform.Rotate(0, -45, 0);
                        break;
                    case "FR":
                        transform.Rotate(0, 45, 0);
                        break;
                    case "BL":
                        transform.Rotate(0, -135, 0);
                        break;
                    case "BR":
                        transform.Rotate(0, 135, 0);
                        break;
                    default:
                        break;
                }

                anim.SetBool("Is_Walking", false);
                anim.SetBool("Is_Idle", true);
            }

            rot = transform.rotation.eulerAngles;
            lastYPos = rot.y;
        }
    }
}