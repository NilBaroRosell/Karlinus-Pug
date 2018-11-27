using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class referenceMovement : MonoBehaviour {

    public GameObject player;
    private movement getJumping;
    public bool onFloor = true;

    // Update is called once per frame
    void FixedUpdate () {
        getJumping = player.GetComponent<movement>();
        onFloor = getJumping.onFloor;

        if (onFloor) transform.position = player.transform.position;
        else transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
	}
}
