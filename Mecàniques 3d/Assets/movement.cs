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

    public bool onFloor = false;

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

        if (Input.GetKeyDown(KeyCode.Space) && onFloor)
        {
            anim.SetTrigger("Is_Jumping");
            rb.AddForce(new Vector3(0, 300, 0));
            onFloor = false;
        }

        if (moveVertical > 0 && moveHorizontal > 0)
        {
            transform.Rotate(0, 45, 0);
            anim.SetBool("Is_Walking", true);
            anim.SetBool("Is_Idle", false);
        }
        else if (moveVertical > 0 && moveHorizontal < 0)
        {
            transform.Rotate(0, -45, 0);
            anim.SetBool("Is_Walking", true);
            anim.SetBool("Is_Idle", false);
        }
        else if (moveVertical < 0 && moveHorizontal > 0)
        {
            transform.Rotate(0, 135, 0);
            anim.SetBool("Is_Walking", true);
            anim.SetBool("Is_Idle", false);
        }
        else if (moveVertical < 0 && moveHorizontal < 0)
        {
            transform.Rotate(0, -135, 0);
            anim.SetBool("Is_Walking", true);
            anim.SetBool("Is_Idle", false);
        }
        else if (moveVertical > 0)
        {
            anim.SetBool("Is_Walking", true);
            anim.SetBool("Is_Idle", false);
        }
        else if (moveVertical < 0)
        {
            transform.Rotate(0, 180, 0);
            anim.SetBool("Is_Walking", true);
            anim.SetBool("Is_Idle", false);
        }
        else if (moveHorizontal > 0)
        {
            transform.Rotate(0, 90, 0);
            anim.SetBool("Is_Walking", true);
            anim.SetBool("Is_Idle", false);
        }
        else if (moveHorizontal < 0)
        {
            transform.Rotate(0, -90, 0);
            anim.SetBool("Is_Walking", true);
            anim.SetBool("Is_Idle", false);
        }
        if (moveVertical == 0 && moveHorizontal == 0)
        {
            anim.SetBool("Is_Walking", false);
            anim.SetBool("Is_Idle", true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "floor") onFloor = true;
    }
}