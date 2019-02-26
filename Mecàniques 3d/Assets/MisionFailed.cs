using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisionFailed : MonoBehaviour {

    public GameObject advice;
    public GameObject misionFailed;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(advice.activeSelf == false && misionFailed.activeSelf == true)
        {
            // s'avisa al jugador que està sortint del radi
        }
        else if (misionFailed.activeSelf == false)
        {
            // misió fallida
        }
    }
}
