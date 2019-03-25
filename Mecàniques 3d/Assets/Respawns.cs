using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawns : MonoBehaviour
{

    public enum InitialRespawns { SEWER_1, SEWER_2, SEWER_3, CITY_1, CITY_2, PUB_INSIDE, PUB_OUTSIDE, NONE };
    private Vector3[] RespawnPoints;
    private GameObject All_Mision_Objects;
    public GameObject[] BoxTriggers;
    public GameObject[] Enemies;
    public GameObject[] Mision_Objects;
    public InitialRespawns initialRespawn;
    private int initialRespawnIndex;

    private void Awake()
    {
        All_Mision_Objects = transform.GetChild(1).gameObject;
        setAllFalse();
    }


    private void LoadNONE()
    {
        //NONE Mision RESPAWNS
        RespawnPoints = new Vector3[7];
        RespawnPoints[0] = new Vector3(-63.28f, -9.1f, 89.17f);
        RespawnPoints[1] = new Vector3(85.8f, -9.1f, 321.1f);
        RespawnPoints[2] = new Vector3(-73.16f, -9.1f, 400.28f);
        RespawnPoints[3] = new Vector3(-84.99f, -27.56327f, -41.88f);
        RespawnPoints[4] = new Vector3(100.59f, -27.56327f, 257.55f);
        RespawnPoints[5] = new Vector3(21.93f, 13.06f, -23.75f);
        RespawnPoints[6] = new Vector3(83.48f, -27.52f, -34.6f);

        //Enemies
        Enemies = GameObject.FindGameObjectsWithTag("enemy");

        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i].transform.parent.gameObject.name != "Enemigos Notoriedad Normal" && Enemies[i].transform.parent.gameObject.name != "ENEMIGOS_SEWER_2" && Enemies[i].transform.parent.gameObject.name != "Enemies_Zone_2") Enemies[i].GetComponent<csAreaVision>().DestroyEnemy();
        }
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

    private void LoadM2()
    {
        //MISION 2 RESPAWN POINTS
        RespawnPoints = new Vector3[6];
        RespawnPoints[0] = new Vector3(5.85f, -27.52068f, -41.88f);
        RespawnPoints[1] = new Vector3(36.38f, -27.52068f, -34.99f);
        RespawnPoints[2] = new Vector3(21.93f, 13.06f, -23.75f);
        RespawnPoints[3] = new Vector3(83.48f, -27.52f, -34.6f);
        RespawnPoints[4] = new Vector3(80.1f, -27.52f, 35.57f);
        RespawnPoints[5] = new Vector3(-7.7f, -27.52f, 151.8f);

        //MISION 2 BOX TRIGGERS
        All_Mision_Objects.transform.GetChild(1).gameObject.SetActive(true);
        BoxTriggers = new GameObject[All_Mision_Objects.transform.GetChild(1).GetChild(0).transform.childCount];
        for (int i = 0; i < BoxTriggers.Length; i++) BoxTriggers[i] = All_Mision_Objects.transform.GetChild(1).GetChild(0).GetChild(i).gameObject;

        //MISION 2 ENEMIES
        Enemies = GameObject.FindGameObjectsWithTag("enemy");

        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i].transform.parent.gameObject.name != "Enemigos M2" && Enemies[i].transform.parent.gameObject.name != "Enemigos Notoriedad Normal") Enemies[i].GetComponent<csAreaVision>().DestroyEnemy();
        }

        //MISION 2 OTHER OBJECTS
        Mision_Objects = new GameObject[4];
        if (GameObject.Find("dfk_doorframe") != null) Mision_Objects[0] = GameObject.Find("dfk_doorframe"); //PUB door trigger
        Mision_Objects[1] = All_Mision_Objects.transform.GetChild(1).GetChild(1).GetChild(0).gameObject; //Servant
        if (GameObject.Find("Enemigos M2") != null) Mision_Objects[2] = GameObject.Find("Enemigos M2"); //Enemigos
        Mision_Objects[Mision_Objects.Length - 1] =
            All_Mision_Objects.transform.GetChild(1).GetChild(1).GetChild(All_Mision_Objects.transform.GetChild(1).GetChild(1).transform.childCount - 1).gameObject;//Zone Controll

    }

    private void LoadM3(int checkPoint)
    {
        //MISION 2 RESPAWN POINTS
        RespawnPoints = new Vector3[3];
        RespawnPoints[0] = new Vector3(5.85f, -27.52068f, -41.88f);
        RespawnPoints[1] = new Vector3(8.62f, 6.558f, -13.46f);
        RespawnPoints[2] = new Vector3(168.51f, -27.52f, 158.2f);

        //MISION 2 BOX TRIGGERS
        All_Mision_Objects.transform.GetChild(2).gameObject.SetActive(true);
        BoxTriggers = new GameObject[All_Mision_Objects.transform.GetChild(2).GetChild(0).transform.childCount];
        for (int i = 0; i < BoxTriggers.Length; i++) BoxTriggers[i] = All_Mision_Objects.transform.GetChild(2).GetChild(0).GetChild(i).gameObject;

        //MISION 2 ENEMIES
        Enemies = GameObject.FindGameObjectsWithTag("enemy");

        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i].transform.parent.gameObject.name != "Enemigos M3 Scape" && Enemies[i].transform.parent.gameObject.name != "Enemigos M3 Kill" && Enemies[i].transform.parent.gameObject.name != "Enemigos Casa Capitan") Enemies[i].GetComponent<csAreaVision>().DestroyEnemy();
        }

        //MISION 3 OTHER OBJECTS
        Mision_Objects = new GameObject[7];
        if (GameObject.Find("ServantM3") != null) Mision_Objects[0] = GameObject.Find("ServantM3");
        if (GameObject.Find("Enemigos M3 Scape") != null) Mision_Objects[1] = GameObject.Find("Enemigos M3 Scape");
        if (GameObject.Find("Enemigos M3 Kill") != null) Mision_Objects[2] = GameObject.Find("Enemigos M3 Kill");
        if (GameObject.Find("DistancePoint") != null) Mision_Objects[3] = GameObject.Find("DistancePoint");
        Mision_Objects[4] = gameObject.transform.GetChild(1).transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;
        Mision_Objects[5] = gameObject.transform.GetChild(1).transform.GetChild(2).transform.GetChild(1).transform.GetChild(2).gameObject;
        if (GameObject.Find("Enemigos Casa Capitan") != null) Mision_Objects[6] = GameObject.Find("Enemigos Casa Capitan");
    }

    private void LoadM4()
    {
        //MISION 4 RESPAWN POINTS
        RespawnPoints = new Vector3[3];
        RespawnPoints[0] = new Vector3(85.8f, -9.1f, 321.1f);
        RespawnPoints[1] = new Vector3(-122.71f, -27.52f, 267.58f);
        RespawnPoints[2] = new Vector3(-69.33f, -27.52f, 295.55f);

        //MISION 4 BOX TRIGGERS
        All_Mision_Objects.transform.GetChild(3).gameObject.SetActive(true);
        BoxTriggers = new GameObject[All_Mision_Objects.transform.GetChild(3).GetChild(0).transform.childCount];
        for (int i = 0; i < BoxTriggers.Length; i++) BoxTriggers[i] = All_Mision_Objects.transform.GetChild(3).GetChild(0).GetChild(i).gameObject;

        //MISION 4 ENEMIES
        Enemies = GameObject.FindGameObjectsWithTag("enemy");

        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i].transform.parent.gameObject.name != "Enemigos M4" && Enemies[i].transform.parent.gameObject.name != "ENEMIGOS_SEWER_2") Enemies[i].GetComponent<csAreaVision>().DestroyEnemy();
        }

        //MISION 4 OTHER OBJECTS
        if (GameObject.Find("Muralla") != null) Destroy(GameObject.Find("Muralla"));
        if (GameObject.Find("Cases pobres") != null) Destroy(GameObject.Find("Cases pobres"));
        if (GameObject.Find("Casa Capita") != null) Destroy(GameObject.Find("Casa Capita"));
        if (GameObject.Find("Cases rics") != null) Destroy(GameObject.Find("Cases rics"));
        if (GameObject.Find("Taberna") != null) Destroy(GameObject.Find("Taberna"));
        if (GameObject.Find("Zone_1") != null) GameObject.Find("Zone_1").SetActive(false);
        Mision_Objects = new GameObject[1];
        Mision_Objects[Mision_Objects.Length - 1] =
            All_Mision_Objects.transform.GetChild(3).GetChild(1).GetChild(All_Mision_Objects.transform.GetChild(3).GetChild(1).transform.childCount - 1).gameObject;//Zone Controll

    }

    private void LoadSM1()
    {
        //RESPAWN POINTS
        RespawnPoints = new Vector3[3];
        RespawnPoints[0] = new Vector3(30.765f, -27.523f, -38.321f);
        RespawnPoints[1] = new Vector3(32.44f, -27.523f, -43f);
        RespawnPoints[2] = new Vector3(-63.28f, -9.1f, 89.17f);

        //BOX TRIGGERS
        All_Mision_Objects.transform.GetChild(4).gameObject.SetActive(true);
        BoxTriggers = new GameObject[All_Mision_Objects.transform.GetChild(4).GetChild(0).transform.childCount];
        for (int i = 0; i < BoxTriggers.Length; i++) BoxTriggers[i] = All_Mision_Objects.transform.GetChild(4).GetChild(0).GetChild(i).gameObject;


        //OTHER OBJECTS
        Mision_Objects = new GameObject[3];
        if (GameObject.Find("Enemigos_SM1") != null) Mision_Objects[0] = GameObject.Find("Enemigos_SM1");
        if (GameObject.Find("Secundary Camera") != null) Mision_Objects[1] = GameObject.Find("Secundary Camera");
        if (GameObject.Find("Camera Destination") != null) Mision_Objects[2] = GameObject.Find("Camera Destination");
    }

    public Vector3 NONE()
    {
        LoadNONE();
        if (GameObject.Find("Zone_1") != null) GameObject.Find("Zone_1").SetActive(false);
        if (GameObject.Find("Enemies_Zone_2") != null) GameObject.Find("Enemies_Zone_2").SetActive(true);
        if (GameObject.Find("Enemigos_SM1") != null) GameObject.Find("Enemigos_SM1").SetActive(false);
        if (initialRespawn == InitialRespawns.NONE) return RespawnPoints[(int)LoadScene.respawnToLoad];
        else
        {
            LoadScene.respawnToLoad = (InitialRespawns)initialRespawn;
            initialRespawnIndex = (int)initialRespawn;
            initialRespawn = InitialRespawns.NONE;
            return RespawnPoints[initialRespawnIndex];
        }
    }

    public Vector3 M1(int checkPoint)
    {
        //LOAD M1
        LoadM1();
        if (GameObject.Find("Enemies_SM1(1)") != null) GameObject.Find("Enemies_SM1").SetActive(false);
        //OBJECTS RESPAWN
        switch (checkPoint)
        {
            case 0:
                LoadScene.respawnToLoad = InitialRespawns.SEWER_1;
                Mision_Objects[1].SetActive(false);
                Enemies[2].SetActive(false);
                Mision_Objects[0].SetActive(false);
                Mision_Objects[2].SetActive(false);
                break;
            case 1:
                LoadScene.respawnToLoad = InitialRespawns.SEWER_1;
                if (GameObject.Find("Enemigo (3)") != null) GameObject.Find("Enemigo (3)").SetActive(false);
                for (int i = 3; i < BoxTriggers.Length; i++) BoxTriggers[i].SetActive(true);
                Mision_Objects[2].GetComponent<EnemyManager>().maxDist = 125;
                break;
            case 2:
                LoadScene.respawnToLoad = InitialRespawns.SEWER_1;
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

    public Vector3 M2(int checkPoint)
    {
        //LOAD M2
        LoadM2();
        if (GameObject.Find("Enemies_SM1") != null) GameObject.Find("Enemies_SM1").SetActive(false);
        //OBJECTS RESPAWN
        switch (checkPoint)
        {
            case 0:
            case 1:
                LoadScene.respawnToLoad = InitialRespawns.CITY_1;
                Mision_Objects[0].GetComponent<Collider>().enabled = false;
                Mision_Objects[1].SetActive(false);
                Mision_Objects[2].SetActive(false);
                for (int i = 1; i < BoxTriggers.Length; i++) BoxTriggers[i].SetActive(true);
                break;
            case 2:
                LoadScene.respawnToLoad = InitialRespawns.PUB_INSIDE;
                Mision_Objects[0].GetComponent<Collider>().enabled = false;
                Mision_Objects[1].SetActive(false);
                Mision_Objects[Mision_Objects.Length - 1].SetActive(false);
                for (int i = 2; i < BoxTriggers.Length; i++) BoxTriggers[i].SetActive(true);
                break;
            case 3:
                LoadScene.respawnToLoad = InitialRespawns.PUB_OUTSIDE;
                Mision_Objects[0].GetComponent<Collider>().enabled = false;
                Mision_Objects[1].SetActive(true);
                Mision_Objects[1].transform.position = new Vector3(74.3f, -27.52f, -34.519f);
                Mision_Objects[2].SetActive(true);
                Mision_Objects[Mision_Objects.Length - 1].SetActive(false);
                Servant.patrollingIndex = 3;
                Servant.destinationPoint = Servant.Points[Servant.patrollingIndex];
                for (int i = 3; i < BoxTriggers.Length; i++) BoxTriggers[i].SetActive(true);
                break;
            case 4:
                LoadScene.respawnToLoad = InitialRespawns.PUB_OUTSIDE;
                Mision_Objects[0].GetComponent<Collider>().enabled = false;
                Mision_Objects[1].SetActive(true);
                Mision_Objects[1].transform.position = new Vector3(75.1f, -27.52f, 35.57f);
                Mision_Objects[2].SetActive(true);
                Mision_Objects[Mision_Objects.Length - 1].SetActive(false);
                Servant.patrollingIndex = 7;
                Servant.destinationPoint = Servant.Points[Servant.patrollingIndex];
                for (int i = 4; i < BoxTriggers.Length; i++) BoxTriggers[i].SetActive(true);
                for (int i = 2; i >= 0; i--) Mision_Objects[2].transform.GetChild(i).gameObject.GetComponent<csAreaVision>().DestroyEnemy();
                break;
            case 5:
                LoadScene.respawnToLoad = InitialRespawns.PUB_OUTSIDE;
                Mision_Objects[0].GetComponent<Collider>().enabled = false;
                Mision_Objects[1].SetActive(true);
                Mision_Objects[1].transform.position = new Vector3(-13.7f, -27.52f, 151.8f);
                Mision_Objects[2].SetActive(true);
                Mision_Objects[Mision_Objects.Length - 1].SetActive(false);
                Servant.patrollingIndex = 16;
                Servant.destinationPoint = Servant.Points[Servant.patrollingIndex];
                for (int i = 5; i < BoxTriggers.Length; i++) BoxTriggers[i].SetActive(true);
                for (int i = 6; i >= 0; i--) Mision_Objects[2].transform.GetChild(i).gameObject.GetComponent<csAreaVision>().DestroyEnemy();
                break;
            default:
                break;
        }
        //PLAYER RESPAWN
        return RespawnPoints[checkPoint];
    }

    public Vector3 M3(int checkPoint)
    {
        //LOAD M4
        LoadM3(checkPoint);
        //OBJECTS RESPAWN
        switch (checkPoint)
        {
            case 0:
                LoadScene.respawnToLoad = InitialRespawns.CITY_2;
                Mision_Objects[0].SetActive(true);
                for (int i = 3; i >= 0; i--)
                {
                    Mision_Objects[1].transform.GetChild(i).gameObject.GetComponent<csAreaVision>().DestroyEnemy();
                }
                /*Mision_Objects[6].transform.GetChild(0).gameObject.GetComponent<csAreaVision>().DestroyEnemy();
                Mision_Objects[6].transform.GetChild(1).gameObject.GetComponent<csAreaVision>().DestroyEnemy();*/
                break;
            case 1:
                LoadScene.respawnToLoad = InitialRespawns.NONE;
                break;
            case 2:
                LoadScene.respawnToLoad = InitialRespawns.CITY_2;
                Mision_Objects[0].SetActive(false);
                for (int i = 5; i >= 0; i--)
                {
                    Mision_Objects[2].transform.GetChild(i).gameObject.GetComponent<csAreaVision>().DestroyEnemy();
                }
                /*Mision_Objects[6].transform.GetChild(0).gameObject.GetComponent<csAreaVision>().DestroyEnemy();
                Mision_Objects[6].transform.GetChild(1).gameObject.GetComponent<csAreaVision>().DestroyEnemy();*/
                break;
            default:
                break;
        }
        //PLAYER RESPAWN
        return RespawnPoints[checkPoint];
    }

    public Vector3 M4(int checkPoint)
    {
        //LOAD M4
        LoadM4();
        //OBJECTS RESPAWN
        switch (checkPoint)
        {
            case 0:
                LoadScene.respawnToLoad = InitialRespawns.SEWER_2;
                for (int i = 0; i < BoxTriggers.Length; i++) BoxTriggers[i].SetActive(true);
                break;
            case 1:
                Mision_Objects[Mision_Objects.Length - 1].SetActive(false);
                LoadScene.respawnToLoad = InitialRespawns.PUB_OUTSIDE;
                break;
            case 2:
                Mision_Objects[Mision_Objects.Length - 1].SetActive(false);
                if (GameObject.Find("Enemigos M4") != null)
                {
                    for (int i = 7; i >= 0; i--) GameObject.Find("Enemigos M4").transform.GetChild(i).GetComponent<csAreaVision>().DestroyEnemy();
                }
                break;
            default:
                break;
        }
        //PLAYER RESPAWN
        return RespawnPoints[checkPoint];
    }

    public Vector3 SM1(int checkPoint)
    {
        //LOAD
        LoadSM1();
        if (GameObject.Find("Zone_1") != null) GameObject.Find("Zone_1").SetActive(false);
        if (GameObject.Find("Enemies_Zone_2") != null) GameObject.Find("Enemies_Zone_2").SetActive(false);
        if (GameObject.Find("") != null) GameObject.Find("Zone_1").SetActive(false);

        //OBJECTS RESPAWN
        switch (checkPoint)
        {
            case 0:
                Mision_Objects[0].SetActive(true);
                Mision_Objects[1].SetActive(true);
                Mision_Objects[2].SetActive(true);
                break;
            case 1:
                Mision_Objects[0].SetActive(false);
                Mision_Objects[1].SetActive(true);
                Mision_Objects[2].SetActive(true);
                break;
            case 2:
                Mision_Objects[0].SetActive(true);
                Mision_Objects[2].SetActive(false);
                break;
            case 3:
                Mision_Objects[1].SetActive(false);
                Mision_Objects[2].SetActive(false);
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
