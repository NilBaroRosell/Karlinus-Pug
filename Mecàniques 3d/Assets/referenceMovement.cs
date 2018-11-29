﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class referenceMovement : MonoBehaviour {

    public GameObject player;
    private movement getInfo;
    public GameObject zona1;
    public bool jumping = true;
    public Vector3 lastPos;
    public bool wsPressed = false;
    public bool adPressed = false;
    public bool following = false;
    public Rigidbody camara;

    private void Start()
    {
        transform.position = player.transform.position;
        lastPos = transform.position;
    }
    // Update is called once per frame
    void FixedUpdate() {
        getInfo = player.GetComponent<movement>();
        jumping = getInfo.jumping;
        wsPressed = getInfo.wsPressed;
        adPressed = getInfo.adPressed;

        if (!wsPressed && !adPressed)
        {
            following = false;
        }
        else
        {
            if (!jumping)
            {
                if (transform.position.y != player.transform.position.y && (wsPressed || adPressed) && (player.transform.position.x > lastPos.x + 1 || player.transform.position.x < lastPos.x - 1 || player.transform.position.z > lastPos.z + 1 || player.transform.position.z < lastPos.z - 1))
                {
                    following = true;
                }
            }
        }

        if (((player.transform.position.x > 47.7 && player.transform.position.x < 52) && (player.transform.position.y > -8 && player.transform.position.y < 0.75) && (player.transform.position.z > 32 && player.transform.position.z < 36)) || following)
        {
            transform.position = player.transform.position;
            lastPos = transform.position;
        }
	}
}
