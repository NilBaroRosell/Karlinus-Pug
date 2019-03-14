using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirameWapo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = GameObject.Find("Jugador").transform.position;
      
	}
}
