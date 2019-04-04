using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class addObstacleMeshToAll : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            this.gameObject.transform.GetChild(i).gameObject.AddComponent<NavMeshObstacle>();
            this.gameObject.transform.GetChild(i).gameObject.GetComponent<NavMeshObstacle>().carving = true;
        }
    }
}
