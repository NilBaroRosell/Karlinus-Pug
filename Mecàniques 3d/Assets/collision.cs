using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collision : MonoBehaviour {

    public bool onFloor = false;

    public void Update()
    {
        cucumber.onFloor = onFloor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "floor") onFloor = true;
    }
}