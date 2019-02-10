using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawns : MonoBehaviour {

    public enum InitialRespawns { SEWER_1, SEWER_2, SEWER_3, CITY_1, CITY_2, PUB, HOUSE, PALACE, NONE };
    private Vector3[] RespawnPoints;
    private GameObject All_Mision_Objects;
    public GameObject[] BoxTriggers;
    public GameObject[] Enemies;
    public GameObject []Mision_Objects;
    public InitialRespawns initialRespawn;
    private int initialRespawnIndex;
    public bool entra = false;

    private void Awake()
    {
        All_Mision_Objects = transform.GetChild(1).gameObject;
        setAllFalse();
    }


    private void LoadNONE()
    {
        //NONE Mision RESPAWNS
        RespawnPoints = new Vector3[5];
        RespawnPoints[0] = new Vector3(-63.28f, -9.1f, 89.17f);
        RespawnPoints[1] = new Vector3(85.8f, -9.1f, 321.1f);
        RespawnPoints[2] = new Vector3(-73.16f, -9.1f, 400.28f);
        RespawnPoints[3] = new Vector3(-84.99f, -27.56327f, -41.88f);
        RespawnPoints[4] = new Vector3(100.59f, -27.56327f, 257.55f);
    }

    private void LoadM1()
    {
        //MISION 1 RESPAWN POINTS
        RespawnPoints = new Vector3[3];
        RespawnPoints[0] = new Vector3(-85.11f, 0.5040889f, -41.45f);
        RespawnPoints[1] = new Vector3(-0.2658552f, 0.5040889f, 5.67f);
        RespawnPoints[2] = new Vector3(50.89f, -6.778945f, 37.37239f);

        //MISION 1 BOX TRIGGERS
        All_Mision_Objects.transform.GetChild(0).gameObject.SetActive(true);
        BoxTriggers = new GameObject[All_Mision_Objects.transform.GetChild(0).GetChild(0).transform.childCount];
        for (int i = 0; i < BoxTriggers.Length; i++) BoxTriggers[i] = All_Mision_Objects.transform.GetChild(0).GetChild(0).GetChild(i).gameObject;

        //MISION 1 ENEMIES
        Enemies = GameObject.FindGameObjectsWithTag("enemy");

        //MISION 1 OTHER OBJECTS
        Mision_Objects = new GameObject[3];
        Mision_Objects[0] = GameObject.Find("Enemies_Zone_2");
        Mision_Objects[1] = GameObject.Find("Enemigo (3)");
        Mision_Objects[2] = GameObject.Find("EnemyManager");
    }

    private void LoadSM2()
    {
        //MISION 2 RESPAWN POINTS
        RespawnPoints = new Vector3[1];
        RespawnPoints[0] = new Vector3(30.765f, -27.523f, -38.321f);

        //MISION 2 BOX TRIGGERS
        All_Mision_Objects.transform.GetChild(5).gameObject.SetActive(true);
        BoxTriggers = new GameObject[All_Mision_Objects.transform.GetChild(5).GetChild(0).transform.childCount];
        for (int i = 0; i < BoxTriggers.Length; i++) BoxTriggers[i] = All_Mision_Objects.transform.GetChild(5).GetChild(0).GetChild(i).gameObject;
        entra = true;

        //MISION 2 ENEMIES
        //Enemies = GameObject.FindGameObjectsWithTag("enemy");

        //MISION 2 OTHER OBJECTS
        Mision_Objects = new GameObject[3];
        Mision_Objects[0] = GameObject.Find("Enemigos_SM2");
        Mision_Objects[1] = GameObject.Find("Secundary Camera");
        Mision_Objects[2] = GameObject.Find("Camera Destination");
    }

    public Vector3 NONE()
    {
        LoadNONE();
        if (GameObject.Find("Zone_1") != null) GameObject.Find("Zone_1").SetActive(false);
        if (GameObject.Find("Enemies_Zone_2") != null) GameObject.Find("Enemies_Zone_2").SetActive(true);
        if (GameObject.Find("Enemigos_SM2") != null) GameObject.Find("Enemigos_SM2").SetActive(false);
        if (initialRespawn == InitialRespawns.NONE) return RespawnPoints[(int)LoadScene.respawnToLoad];
        else
        {
            LoadScene.respawnToLoad = (LoadScene.Scenes)initialRespawn;
            initialRespawnIndex = (int)initialRespawn;
            initialRespawn = InitialRespawns.NONE;
            return RespawnPoints[initialRespawnIndex];
        }
    }

    public Vector3 M1(int checkPoint)
    {
        //LOAD M1
        LoadM1();
        //OBJECTS RESPAWN
        switch(checkPoint)
        {
            case 0:
                Mision_Objects[1].SetActive(false);
                Enemies[2].SetActive(false);
                Mision_Objects[0].SetActive(false);
                Mision_Objects[2].SetActive(false);
                break;
            case 1:
                if (GameObject.Find("Enemigo (3)") != null) GameObject.Find("Enemigo (3)").SetActive(false);
                for (int i = 3; i < BoxTriggers.Length; i++) BoxTriggers[i].SetActive(true);
                Mision_Objects[2].GetComponent<EnemyManager>().maxDist = 125;
                break;
            case 2:
                if (GameObject.Find("Zone_1") != null) GameObject.Find("Zone_1").SetActive(false);
                for (int i = 6; i < BoxTriggers.Length; i++) BoxTriggers[i].SetActive(true);
                Mision_Objects[0].SetActive(true);
                break;
            default:
                break;
        }
        //PLAYER RESPAWN
        return RespawnPoints[checkPoint];
    }

    public Vector3 SM2(int checkPoint)
    {
        //LOAD M2
        LoadSM2();
        //OBJECTS RESPAWN
        switch (checkPoint)
        {
            case 0:
                Mision_Objects[0].SetActive(false);
                Mision_Objects[1].SetActive(true);
                Mision_Objects[2].SetActive(true);
                break;
            case 1:
                Mision_Objects[0].SetActive(false);
                Mision_Objects[1].SetActive(true);
                Mision_Objects[2].SetActive(true);
                break;
            case 2:
                Mision_Objects[0].SetActive(false);
                Mision_Objects[1].SetActive(false);
                Mision_Objects[2].SetActive(false);
                for (int i = 0; i < BoxTriggers.Length; i++) BoxTriggers[i].SetActive(true);
                break;
            default:
                break;
        }
        //PLAYER RESPAWN
        return RespawnPoints[checkPoint];
    }

    public void setAllFalse()
    {
        for (int i = 0; i < All_Mision_Objects.transform.childCount; i++) All_Mision_Objects.transform.GetChild(i).gameObject.SetActive(false);
    }
}