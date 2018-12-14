using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawns : MonoBehaviour {

    private Vector3[] RespawnPoints;
    private GameObject All_Mision_Objects;
    public GameObject[] BoxTriggers;
    public GameObject[] Enemies;
    public GameObject []Mision_Objects;

    private void Awake()
    {
        All_Mision_Objects = transform.GetChild(1).gameObject;
        setAllFalse();
    }


    private void LoadNONE()
    {
        //NONE Mision RESPAWNS
        RespawnPoints = new Vector3[1];
        RespawnPoints[0] = new Vector3(-84.99f, -27.56327f, -41.88f);
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
        Mision_Objects = new GameObject[1];
        Mision_Objects[0] = GameObject.Find("Enemies_Zone_2");
    }

    public Vector3 NONE()
    {
        return RespawnPoints[(int)LoadScene.respawnToLoad];
    }

    public Vector3 M1(int checkPoint)
    {
        //LOAD M1
        LoadM1();
        //OBJECTS RESPAWN
        switch(checkPoint)
        {
            case 0:
                Enemies[2].SetActive(false);
                Mision_Objects[0].SetActive(false);
                HUD.canvasHUD.SetActive(false);
                break;
            case 1:
                if (GameObject.Find("Enemigo (3)") != null) GameObject.Find("Enemigo (3)").SetActive(false);
                break;
            case 2:
                if (GameObject.Find("Zone_1") != null) GameObject.Find("Zone_1").SetActive(false);
                Mision_Objects[0].SetActive(true);
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
