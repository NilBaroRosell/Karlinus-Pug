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

    public float speed;
    //public float normalDash = 10000;
    //public float superDash = 15000;

    public bool onFloor = false;
    public bool jumping = false;
    public bool adPressed = false;
    public bool wsPressed = false;

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
    private bool hitting = false;

    public static bool liquidState = false;
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
        transform.position = new Vector3(PlayerPrefs.GetFloat("KarlinusPosX"), PlayerPrefs.GetFloat("KarlinusPosY"),
        PlayerPrefs.GetFloat("KarlinusPosZ"));//(-84.99f, 1.27f, -41.88f);
        weapon_show = GameObject.Find("weapon_show");
        weapon_hide = GameObject.Find("weapon_hide");
        weapon_show.SetActive(false);
        weapon_hide.SetActive(true);
        Physics.IgnoreLayerCollision(9, 8);
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case playerState.IDLE:
                {
                    vel = rb.velocity;

                    movePlayer();

                    finishDash = Time.frameCount;
                    if ((finishDash - startDash) > 80) activateDash = true;
                    if (Input.GetKeyDown(KeyCode.F) && activateDash)
                    {
                        vectorDirection = ((moveVertical * transform.forward) + (moveHorizontal * transform.right));
                        vectorDirection.Normalize();
                        rb.velocity *= 0;
                        rb.AddForce(vectorDirection * 10000);
                        startDash = Time.frameCount;
                        activateDash = false;
                    }

                    checkCooldown = GetComponent<liquidState>();
                    cooldown = checkCooldown.cooldown;
                    if (Input.GetKeyDown(KeyCode.Q) && !cooldown)
                    {
                        liquidState = true;
                        state = playerState.LIQUID;
                    }

                    //Weapon
                    if (anim.GetBool("Is_Detected") == true && Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        anim.SetBool("Is_Running", false);
                        anim.SetBool("Is_Crouching", false);
                        anim.SetBool("Is_Walking", false);
                        anim.SetBool("Is_Idle", false);
                        anim.SetTrigger("Is_Hitting");
                        hitting = true;
                        anim.SetBool("Is_Damaging", true);
                        startHit = Time.frameCount;
                    }

                    if (anim.GetBool("Is_Dying") == true)
                    {
                        state = playerState.DYING;
                        startDie = Time.frameCount;
                    }
                    break;
                }
            case playerState.DYING:
                {
                    finishDie = Time.frameCount;
                    if ((finishDie - startDie) > 250)
                    {
                        SceneManager.LoadScene("DEAD");
                    }
                    break;
                }
            case playerState.LIQUID:
                {
                    Debug.Log(movement.liquidState);
                    movePlayer();
                    if (!liquidState) state = playerState.IDLE;
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
        if (message == 1)
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
            }

            speed = 9;

            if (liquidState)
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

            if (hitting)
            {
                finishHit = Time.frameCount;
                if ((finishHit - startHit) > 30)
                {
                    hitting = false;
                    anim.SetBool("Is_Damaging", false);
                }
            }

            if (rb.velocity.magnitude > speed) rb.velocity = rb.velocity.normalized * speed;
            vectorDirection = ((moveVertical * transform.forward) + (moveHorizontal * transform.right));
            vectorDirection.Normalize();
            rb.velocity = vectorDirection * speed;

            if (!liquidState && Input.GetKeyDown(KeyCode.Space) && !anim.GetBool("Is_Withdrawing") && !anim.GetBool("Is_Hitting") && !anim.GetBool("Is_Sheathing") && onFloor)
            {
                Physics.gravity = new Vector3(0, -100, 0);
                anim.SetTrigger("Is_Jumping");
                speed = 4;
                vectorDirection *= 2;
                vectorDirection += new Vector3(0, 6, 0);
                rb.velocity = vectorDirection * speed;
                onFloor = false;
                jumping = true;
            }
            else Physics.gravity = new Vector3(0, -250, 0);

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

            if (Input.GetKeyDown(KeyCode.Z))
            {
                transform.position = new Vector3(-64.511f, -9.288946f, 92.9324f);
                camara.transform.position = new Vector3(-64.59f, -6.42f, 86.97f);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                transform.position = new Vector3(50.89f, -6.778945f, 37.37239f);
                camara.transform.position = new Vector3(50.811f, -3.91f, 31.41f);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Menu_1");
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