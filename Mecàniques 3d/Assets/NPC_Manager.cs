using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Manager : MonoBehaviour
{
    public GameObject[] Npcs;
    public Vector3[] NpcsPos;
    public GameObject Player;
    public float maxDist;
    // Use this for initialization
    void Start()
    {
        Npcs = GameObject.FindGameObjectsWithTag("NPC");
        NpcsPos = new Vector3[Npcs.Length];
        for (int i = 0; i < Npcs.Length; i++)
        {
            if (Npcs[i] != null)
                NpcsPos[i] = Npcs[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Npcs.Length; i++)
        {
            if (!Npcs[i].activeSelf)
            {
                Vector3 playerDist = new Vector3(Player.transform.position.x - NpcsPos[i].x, 0.0f, Player.transform.position.z - NpcsPos[i].z);
                if (playerDist.magnitude <= maxDist) Npcs[i].SetActive(true);
                else Npcs[i].SetActive(false);
            }
        }

    }
}