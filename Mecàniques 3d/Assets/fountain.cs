using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fountain : MonoBehaviour
{
    Sprite interact;

    private void Start()
    {
        interact = Resources.Load<Sprite>("Sprites/Drink");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown("e"))
        {
            other.GetComponent<liquidState>().DrinkWater();
        }
        else if (other.gameObject.tag == "Player") other.gameObject.GetComponentInParent<HUD>().showInteractSprite(this.transform.position, interact);
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player") collision.gameObject.GetComponentInParent<HUD>().hideInteractSprite();
    }
}