using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class visible : MonoBehaviour {

    public float dist;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") setAllVisibleScript.maxDist = dist;
    }
}
