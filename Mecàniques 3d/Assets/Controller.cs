using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    CharacterController characterController;
    //public float jumpSpeed = 8.0f;

    public float speed = 6.0f;
    public float gravity = 20.0f;
    public Camera mainCamera;

    private Vector3 camForward;
    private Vector3 camRight;
    private Vector3 moveDirection = Vector3.zero;

    // Use this for initialization
    void Start () {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

            //Change relation to camera
            moveDirection = moveDirection.x * camRight + moveDirection.z * camForward;
            moveDirection *= speed;

           //Por si queremos que el colega salte
           // if (Input.GetButton("Jump"))
           //{
           //    moveDirection.y = jumpSpeed;
           //}
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        //Move relative to camera
        camDirection();

        transform.LookAt(transform.position + new Vector3(moveDirection.x, 0, moveDirection.z));
        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }

    void camDirection()
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        //Don't need Y
        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }
}
