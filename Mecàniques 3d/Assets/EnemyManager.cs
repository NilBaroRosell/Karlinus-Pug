using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public static GameObject[] Enemies;
    public static Vector3[] EnemiesPos;
    public GameObject Player;
    public float maxDist;
	// Use this for initialization
	void Awake () {
        Enemies = GameObject.FindGameObjectsWithTag("enemy");
        EnemiesPos = new Vector3[Enemies.Length];
        Debug.Log(Enemies.Length);
        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i] != null)
                EnemiesPos[i] = Enemies[i].transform.position;
        }
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < Enemies.Length; i++)
        {
            if(!Enemies[i].activeSelf)
            {
                Vector3 playerDist = new Vector3(Player.transform.position.x - EnemiesPos[i].x, 0.0f, Player.transform.position.z - EnemiesPos[i].z);
                if (playerDist.magnitude <= maxDist)
                {
                    Enemies[i].SetActive(true);
                }
            }
        }

    }
}
