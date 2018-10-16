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
    public bool adPressed = false;
    public bool wsPressed = false;

    public string direction;

    public float moveHorizontal;
    public float moveVertical;

    public float startHit = 0;
    public float finishHit = 0;
    public bool hitting = false;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        direction = "F";
    }

    private void FixedUpdate()
    {
        if (hitting)
        {
            finishHit = Time.frameCount;
            if ((finishHit - startHit) > 275)
            {
                hitting = false;
            }
        }

        else
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) wsPressed = true;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) adPressed = true;
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S)) wsPressed = false;
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) adPressed = false;

            if (!wsPressed) moveVertical = 0;
            else moveVertical = Input.GetAxis("Vertical");

            if (!adPressed) moveHorizontal = 0;
            else moveHorizontal = Input.GetAxis("Horizontal");

            if (rb.velocity.magnitude > speed) rb.velocity = rb.velocity.normalized * speed;

            rb.AddForce(moveVertical * reference.transform.forward * speed);
            rb.AddForce(moveHorizontal * reference.transform.right * speed);

            transform.rotation = reference.transform.rotation;

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
                anim.SetBool("Is_Walking", false);
                anim.SetBool("Is_Idle", true);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                rb.AddForce(moveVertical * reference.transform.forward * dashSpeed);
                rb.AddForce(moveHorizontal * reference.transform.right * dashSpeed);
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                anim.SetTrigger("Is_Hitting");
                hitting = true;
                startHit = Time.frameCount;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                rb.AddForce(moveVertical * reference.transform.forward * dashSpeed);
                rb.AddForce(moveHorizontal * reference.transform.right * dashSpeed);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "floor") onFloor = true;
    }
}