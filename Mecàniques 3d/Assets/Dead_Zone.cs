using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead_Zone : MonoBehaviour {

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            loadScreen.Instancia.CargarEscena("DEAD");
        }
    }
}
