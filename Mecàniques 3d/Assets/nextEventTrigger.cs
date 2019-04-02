using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nextEventTrigger : MonoBehaviour {

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        {
            misions.nextEvent = true;
        }
    }
}
