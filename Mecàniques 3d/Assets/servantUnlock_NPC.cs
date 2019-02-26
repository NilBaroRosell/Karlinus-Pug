using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class servantUnlock_NPC : MonoBehaviour {

    private bool talked;
    public static bool canTalk;

    private void Start()
    {
        talked = false;
        canTalk = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (canTalk && other.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.E) && !talked)
        {
            misions.nextEvent = true;
            talked = true;
        }
    }
}
