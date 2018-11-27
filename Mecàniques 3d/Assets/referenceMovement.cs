using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class referenceMovement : MonoBehaviour {

    public GameObject player;
    private movement getJumping;
    public bool jumping = true;

    // Update is called once per frame
    void FixedUpdate () {
        getJumping = player.GetComponent<movement>();
        jumping = getJumping.jumping;

        if (!jumping) transform.position = player.transform.position;
        else transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
	}
}
