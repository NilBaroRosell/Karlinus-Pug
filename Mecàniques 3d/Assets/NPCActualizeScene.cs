using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCActualizeScene : MonoBehaviour {

    private bool startMision = false;
    public misions misionsGO;
    public static bool actualizeScene = false;

    // Use this for initialization
    void Start () {
        if (GameObject.Find("Misiones") != null) misionsGO = GameObject.Find("Misiones").GetComponent<misions>();

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (startMision)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                misionsGO.ActualMision = misions.Misions.SM_2;
                loadScreen.Instancia.CargarEscena("city");
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
