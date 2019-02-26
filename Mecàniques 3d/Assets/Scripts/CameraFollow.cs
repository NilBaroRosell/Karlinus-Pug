using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public float CameraMoveSpeed = 120.0f;

    public GameObject followObject;

    private Vector3 FollowPos;

    public float clampAngle = 80.0f;
    public float inputSensitivity = 150.0f;
    public GameObject CameraObj;
    public GameObject PlayerObj;
    public Vector3 camDistanceToPlayer = new Vector3(3.0f, 3.0f, 3.0f);
    public Vector2 mouse;
    public Vector2 smooth;
    private Vector2 rotation = Vector2.zero;


    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotation.x = rot.x;
        rotation.y = rot.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        mouse.x = Input.GetAxis("Mouse X");
        mouse.y = Input.GetAxis("Mouse Y");

        rotation.y += mouse.y * inputSensitivity * Time.deltaTime;
        rotation.x += mouse.x * inputSensitivity * Time.deltaTime;

        rotation.x = Mathf.Clamp(rotation.x, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotation.x, rotation.y, 0.0f);
        transform.rotation = localRotation;
    }

    private void LateUpdate()
    {
        CameraUpdater();
    }

    private void CameraUpdater()
    {
        Transform target = followObject.transform;

        float step = CameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    } 
}
