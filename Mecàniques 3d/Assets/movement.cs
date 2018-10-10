using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    private Rigidbody rb;
    public int speed;
    public GameObject reference;

    bool onFloor = false;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if(rb.velocity.magnitude > speed) rb.velocity = rb.velocity.normalized * speed;

        rb.AddForce(moveVertical * reference.transform.forward * speed);
        rb.AddForce(moveHorizontal * reference.transform.right * speed);

        transform.localRotation = reference.transform.localRotation;

        if (Input.GetKey(KeyCode.Space) && onFloor)
        {
            rb.AddForce(new Vector3(0, 430, 0));
            onFloor = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "floor") onFloor = true;
    }
}