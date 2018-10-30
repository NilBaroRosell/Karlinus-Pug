using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class movement : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject reference;
    public GameObject camara;
    static Animator anim;
    //static Animator enemyAnim;
    GameObject weapon_show;
    GameObject weapon_hide;

    public int speed;
    public int dashSpeed;

    public bool onFloor = false;
    public bool adPressed = false;
    public bool wsPressed = false;

    public string direction;

    public float moveHorizontal;
    public float moveVertical;

    public float startHit = 0;
    public float finishHit = 0;
    public float startDash;
    public float finishDash;
    public float startDie;
    public float finishDie;
    public bool activateDash = true;
    public enum playerState { IDLE, HITTING, DYING };
    public playerState state;
    Collider w_collider;
    private bool hitting = false;
    

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0, -50, 0);
        anim = GetComponent<Animator>();
        direction = "F";
        state = playerState.IDLE;
        transform.position = new Vector3(-84.99f, 1.27f, -41.88f);
        weapon_show = GameObject.Find("weapon_show");
        weapon_hide = GameObject.Find("weapon_hide");
        weapon_show.SetActive(false);
        weapon_hide.SetActive(true);
    }

    private void FixedUpdate()
    {
        switch(state)
        {
            case playerState.IDLE:
                {
                    if (onFloor)
                    {
                        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                        {
                            wsPressed = true;
                            anim.SetBool("Is_Walking", true);
                            anim.SetBool("Is_Idle", false);
                        }
                        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                        {
                            anim.SetBool("Is_Walking", true);
                            anim.SetBool("Is_Idle", false);
                            adPressed = true;
                        }
                        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
                        {
                            anim.SetBool("Is_Walking", false);
                            anim.SetBool("Is_Idle", true);
                            wsPressed = false;
                        }
                        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
                        {
                            anim.SetBool("Is_Walking", false);
                            anim.SetBool("Is_Idle", true);
                            adPressed = false;
                        }

                        speed = 40;

                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            if (adPressed || wsPressed)
                            {
                                anim.SetBool("Is_Running", true);
                                speed = 55;
                            }
                            else anim.SetBool("Is_Running", false);
                        }
                        else if (Input.GetKeyUp(KeyCode.LeftShift)) anim.SetBool("Is_Running", false);

                        if (Input.GetKey(KeyCode.LeftControl))
                        {
                            if (adPressed || wsPressed)
                            {
                                anim.SetBool("Is_Crouching", true);
                                speed = 35;
                            }
                            else anim.SetBool("Is_Crouching", false);
                        }
                        else if (Input.GetKeyUp(KeyCode.LeftControl)) anim.SetBool("Is_Crouching", false);

                        if (!wsPressed) moveVertical = 0;
                        else moveVertical = Input.GetAxis("Vertical");

                        if (!adPressed) moveHorizontal = 0;
                        else moveHorizontal = Input.GetAxis("Horizontal");

                        if (rb.velocity.magnitude > speed) rb.velocity = rb.velocity.normalized * speed;

                        if (hitting)
                        {
                            finishHit = Time.frameCount;
                            if ((finishHit - startHit) > 30)
                            {
                               hitting = false;
                                anim.SetBool("Is_Damaging", false);
                            }
                        }

                        rb.AddForce(moveVertical * reference.transform.forward * speed);
                        rb.AddForce(moveHorizontal * reference.transform.right * speed);
  

                        transform.rotation = reference.transform.rotation;

                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            anim.SetTrigger("Is_Jumping");
                            rb.AddForce(new Vector3(0, 1000, 0));
                            rb.AddForce(reference.transform.forward * 4 * speed);
                            onFloor = false;
                        }

                        if (Input.GetKeyDown(KeyCode.Z))
                        {
                            transform.position = new Vector3(-58.511f, -9.288946f, 92.9324f);
                            camara.transform.position = new Vector3(-58.59f, -6.42f, 86.97f);
                        }

                        if (Input.GetKeyDown(KeyCode.X))
                        {
                            transform.position = new Vector3(55.89f, -6.778945f, 37.37239f);
                            camara.transform.position = new Vector3(55.811f, -3.91f, 31.41f);
                        }

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
                    }


                    if (moveVertical == 0 && moveHorizontal == 0)
                    {
                        switch (direction)
                        {
                            case "F":
                                transform.Rotate(0, 0, 0);
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

                    finishDash = Time.frameCount;
                    if ((finishDash - startDash) > 80) activateDash = true;
                    if (Input.GetKeyDown(KeyCode.F) && activateDash)
                    {
                        rb.AddForce(moveVertical * reference.transform.forward * dashSpeed);
                        rb.AddForce(moveHorizontal * reference.transform.right * dashSpeed);
                        startDash = Time.frameCount;
                        activateDash = false;
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
            default:
                {
                    break;
                }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "floor") onFloor = true;

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
}