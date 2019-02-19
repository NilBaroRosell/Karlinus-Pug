using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTriggerScript : MonoBehaviour {

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (gameObject.tag == "CheckPoint_Trigger") gameObject.SetActive(false);
        }
    }
}
