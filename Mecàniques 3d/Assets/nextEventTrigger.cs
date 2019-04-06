using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nextEventTrigger : MonoBehaviour {

    Sprite interact;

    private void Start()
    {
        interact = Resources.Load<Sprite>("Sprites/Default");
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        {
            collision.gameObject.GetComponentInParent<HUD>().hideInteractSprite();
            this.transform.GetComponent<Collider>().enabled = false;
            misions.nextEvent = true;
        }
        else if (collision.gameObject.tag == "Player") collision.gameObject.GetComponentInParent<HUD>().showInteractSprite(this.transform.position, interact);

    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player") collision.gameObject.GetComponentInParent<HUD>().hideInteractSprite();
    }

}
