using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warning_Zone : MonoBehaviour {

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject.Find("Jugador").GetComponent<HUD>().showZoneWarning();
        }
    }
}
