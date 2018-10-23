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

    public int speed;
    public int dashSpeed;

    public bool onFloor = false;
    public bool touchingEnemy = false;
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
    

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        direction = "F";
        state = playerState.IDLE;
        transform.position = new Vector3(-84.99f, 1.27f, -41.88f);
    }

    private void FixedUpdate()
    {
        switch(state)
        {
            case playerState.IDLE:
                {
                    if (onFloor)
                    {
                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            anim.SetBool("Is_Running", true);
                            speed = 30;
                        }
                        else if (Input.GetKeyUp(KeyCode.LeftShift)) anim.SetBool("Is_Running", false);
                        else if (Input.GetKey(KeyCode.LeftControl))
                        {
                            anim.SetBool("Is_Crouching", true);
                            speed = 10;
                        }
                        else if (Input.GetKeyUp(KeyCode.LeftControl)) anim.SetBool("Is_Crouching", false);
                        else
                        {
                            speed = 20;
                        }

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

                        if (!wsPressed) moveVertical = 0;
                        else moveVertical = Input.GetAxis("Vertical");

                        if (!adPressed) moveHorizontal = 0;
                        else moveHorizontal = Input.GetAxis("Horizontal");

                        if (rb.velocity.magnitude > speed) rb.velocity = rb.velocity.normalized * speed;

                        rb.AddForce(moveVertical * reference.transform.forward * speed);
                        rb.AddForce(moveHorizontal * reference.transform.right * speed);

                        transform.rotation = reference.transform.rotation;

                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            anim.SetTrigger("Is_Jumping");
                            rb.AddForce(new Vector3(0, 275, 0));
                            onFloor = false;
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
                    if ((finishDash - startDash) > 1000) activateDash = true;
                    if (Input.GetKeyDown(KeyCode.F) && activateDash)
                    {
                        rb.AddForce(moveVertical * reference.transform.forward * dashSpeed);
                        rb.AddForce(moveHorizontal * reference.transform.right * dashSpeed);
                        startDash = Time.frameCount;
                        activateDash = false;
                    }

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        anim.SetTrigger("Is_Hitting");
                        state = playerState.HITTING;
                        startHit = Time.frameCount;
                    }

                    if (touchingEnemy)
                    {
                        anim.SetTrigger("Is_Dying");
                        state = playerState.DYING;
                        startDie = Time.frameCount;
                    }
                    break;
                }
            
            case playerState.HITTING:
                {
                    finishHit = Time.frameCount;
                    if ((finishHit - startHit) > 140) state = playerState.IDLE;
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
        if (collision.gameObject.tag == "enemy") touchingEnemy = true;
    }
}