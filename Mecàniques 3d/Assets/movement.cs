using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject reference;
    public GameObject camara;
    static Animator anim;

    public int speed;
    public int dashSpeed;

    public bool onFloor = false;

    public string direction;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if(rb.velocity.magnitude > speed) rb.velocity = rb.velocity.normalized * speed;

        rb.AddForce(moveVertical * reference.transform.forward * speed);
        rb.AddForce(moveHorizontal * reference.transform.right * speed);

        transform.rotation = reference.transform.rotation;

        if (Input.GetKeyDown(KeyCode.F))
        {
            switch (direction)
            {
                case "F":
                {
                    transform.Rotate(0, 0, 0);
                    break;
                }
                case "B":
                {
                    transform.Rotate(0, 180, 0);
                    break;
                }
                case"L":
                {
                    transform.Rotate(0, -90, 0);
                    break;
                }
                case "R":
                {
                    transform.Rotate(0, 90, 0);
                    break;
                }
                case "FL":
                {
                    transform.Rotate(0, -45, 0);
                    break;
                }
                case "FR":
                {
                    transform.Rotate(0, 45, 0);
                    break;
                }
                case "BL":
                {
                    transform.Rotate(0, -135, 0);
                    break;
                }
                case "BR":
                {
                    transform.Rotate(0, 135, 0);
                    break;
                }
                default: break;
            }
            rb.AddForce(moveVertical * reference.transform.forward * dashSpeed);
            rb.AddForce(moveHorizontal * reference.transform.right * dashSpeed);
        }

        if (Input.GetKeyDown(KeyCode.Space) && onFloor)
        {
            anim.SetTrigger("Is_Jumping");
            rb.AddForce(new Vector3(0, 275, 0));
            onFloor = false;
        }

        if (moveVertical > 0 && moveHorizontal > 0)
        {
            transform.Rotate(0, 45, 0);
            anim.SetBool("Is_Walking", true);
            anim.SetBool("Is_Idle", false);
            direction = "FR";
        }
        else if (moveVertical > 0 && moveHorizontal < 0)
        {
            transform.Rotate(0, -45, 0);
            anim.SetBool("Is_Walking", true);
            anim.SetBool("Is_Idle", false);
            direction = "FL";
        }
        else if (moveVertical < 0 && moveHorizontal > 0)
        {
            transform.Rotate(0, 135, 0);
            anim.SetBool("Is_Walking", true);
            anim.SetBool("Is_Idle", false);
            direction = "BR";
        }
        else if (moveVertical < 0 && moveHorizontal < 0)
        {
            transform.Rotate(0, -135, 0);
            anim.SetBool("Is_Walking", true);
            anim.SetBool("Is_Idle", false);
            direction = "BL";
        }
        else if (moveVertical > 0)
        {
            anim.SetBool("Is_Walking", true);
            anim.SetBool("Is_Idle", false);
            direction = "F";
        }
        else if (moveVertical < 0)
        {
            transform.Rotate(0, 180, 0);
            anim.SetBool("Is_Walking", true);
            anim.SetBool("Is_Idle", false);
            direction = "B";
        }
        else if (moveHorizontal > 0)
        {
            transform.Rotate(0, 90, 0);
            anim.SetBool("Is_Walking", true);
            anim.SetBool("Is_Idle", false);
            direction = "R";
        }
        else if (moveHorizontal < 0)
        {
            transform.Rotate(0, -90, 0);
            anim.SetBool("Is_Walking", true);
            anim.SetBool("Is_Idle", false);
            direction = "L";
        }
        if (moveVertical == 0 && moveHorizontal == 0)
        {
            switch (direction)
            {
                case "F":
                    {
                        transform.Rotate(0, 0, 0);
                        break;
                    }
                case "B":
                    {
                        transform.Rotate(0, 180, 0);
                        break;
                    }
                case "L":
                    {
                        transform.Rotate(0, -90, 0);
                        break;
                    }
                case "R":
                    {
                        transform.Rotate(0, 90, 0);
                        break;
                    }
                case "FL":
                    {
                        transform.Rotate(0, -45, 0);
                        break;
                    }
                case "FR":
                    {
                        transform.Rotate(0, 45, 0);
                        break;
                    }
                case "BL":
                    {
                        transform.Rotate(0, -135, 0);
                        break;
                    }
                case "BR":
                    {
                        transform.Rotate(0, 135, 0);
                        break;
                    }
                default:
                    break;
            }
            anim.SetBool("Is_Walking", false);
            anim.SetBool("Is_Idle", true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "floor") onFloor = true;
    }
}