using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camaraMovement : MonoBehaviour {

    public GameObject player;
    public string a;

    // Update is called once per frame
    void FixedUpdate()
    {
            int layerMask = 0 << 8;

            layerMask = ~layerMask;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Vector3 forward = new Vector3(player.transform.position.x - transform.position.x, (player.transform.position.y + 1) - transform.position.y, player.transform.position.z - transform.position.z);

            if (Physics.Raycast(transform.position, forward, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.collider.tag != "Player" && hit.collider.tag != "limit" && hit.collider.tag != "weapon" && hit.collider.tag != "enemy" && hit.collider.tag != "enemy_weapon")
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                    Transform objectHit = hit.transform;
                    transform.position = hit.point;
                    a = hit.collider.tag;
                }
            }
        }
}