using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCActualizeScene : MonoBehaviour {

    public Misions NPCMision;
    private bool startMision = false;
    public GameObject misionsGO;
    public static bool actualizeScene = false;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (startMision)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                actualizeScene = true;
            }
        }
	}

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            startMision = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        startMision = false;
    }
}
