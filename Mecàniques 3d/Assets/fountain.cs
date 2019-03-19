using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fountain : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown("e")) other.GetComponent<liquidState>().DrinkWater();
    }
}