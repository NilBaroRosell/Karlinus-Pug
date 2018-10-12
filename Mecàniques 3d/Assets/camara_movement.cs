using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camara_movement : MonoBehaviour {

    public GameObject player;
    public GameObject reference;
    private Vector3 distance;

	// Use this for initialization
	void Start () {
        distance = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        distance = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * 2, Vector3.up) * distance;
        transform.position = player.transform.position + distance;
        transform.LookAt(player.transform.position);

        Vector3 copyRotation = new Vector3(0, transform.eulerAngles.y, 0);
        reference.transform.eulerAngles = copyRotation;
    }
}
