using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camara_movement : MonoBehaviour {

    public GameObject player;
    public GameObject reference;
    public Vector3 distance;

    // Use this for initialization
    void Start () {
        distance = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate () {

        distance = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * 2, Vector3.up) * distance;
        transform.position = player.transform.position + distance;
        transform.LookAt(player.transform.position);

        /*distance = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * 2, -Vector3.right) * distance;
        transform.position = player.transform.position + distance;
        transform.LookAt(player.transform.position);*/

        int layerMask = 0 << 8;

        layerMask = ~layerMask;

        RaycastHit hit;

        Vector3 forward = new Vector3(player.transform.position.x - transform.position.x, (player.transform.position.y + 1) - transform.position.y, player.transform.position.z - transform.position.z);

        if (Physics.Raycast(transform.position, forward, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.tag != "Player" && hit.collider.tag != "limit" && hit.collider.tag != "weapon" && hit.collider.tag != "enemy" && hit.collider.tag != "enemy_weapon")
            {
                Transform objectHit = hit.transform;
                transform.position = hit.point;
            }
        }
        
        transform.Rotate(-15, 0, 0);

        Vector3 copyRotation = new Vector3(0, transform.eulerAngles.y, 0);
        reference.transform.eulerAngles = copyRotation;
    }
}
