using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torchManager : MonoBehaviour {

    public GameObject Player;
    public float maxDist;
    // Use this for initialization
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Vector3 playerDist = new Vector3(Player.transform.position.x -gameObject.transform.GetChild(i).transform.position.x, 0.0f, Player.transform.position.z - gameObject.transform.GetChild(i).transform.position.z);
            if (playerDist.magnitude <= maxDist && !gameObject.transform.GetChild(i).gameObject.activeSelf)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(true);
            }
            else if(playerDist.magnitude > maxDist) gameObject.transform.GetChild(i).gameObject.SetActive(false);

        }

    }
}
