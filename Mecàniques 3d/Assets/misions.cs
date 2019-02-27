using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class misions : MonoBehaviour {

    private GameObject normalLight;
    private GameObject sewerLight;
    public enum Misions { M1, M2, M3, M4, SM_1, SM_2, SM_3, SM_4, SM_5, SM_6, NONE };
    public  Misions ActualMision;
    public bool editorRespawn;
    public int editorRespawnNum;
    public static int respawnIndex;
    public static int misionIndex;
    private Respawns loadRespawn;
    private HUD HUD_Script;
    private GameObject Player;
    private movement playerMovement;
    public static bool nextEvent;
    private GameObject mainCamera;
    private GameObject secundaryCamera;
    private GameObject secundaryCameraDestination;
    public static misions Instance;
    [System.Serializable]
    public struct MisionPoint
    {
        public GameObject pointObject;
        public Vector3 playerDistance;
        public bool[] MisionsCompleted;
    }

    public MisionPoint PrincipalMision;
    public MisionPoint RatHood;
    public MisionPoint RebelCat;
    public MisionPoint ScaryDog;
    public static bool pauseMenu;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null || Instance.ActualMision == Misions.NONE)
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
                ActualMision = Misions.NONE;
                editorRespawn = false;
            }
            Instance = this;
            MisionPoint PrincipalMision = new MisionPoint();
        PrincipalMision.MisionsCompleted = new bool[4];
        MisionPoint RatHood = new MisionPoint();
        RatHood.MisionsCompleted = new bool[2];
        MisionPoint RebelCat = new MisionPoint();
        RebelCat.MisionsCompleted = new bool[2];
        MisionPoint ScaryDog = new MisionPoint();
        ScaryDog.MisionsCompleted = new bool[2];
        loadRespawn = GetComponent<Respawns>();
        misionIndex = 0;
        nextEvent = false;
            if (editorRespawn)
                respawnIndex = editorRespawnNum;
            else respawnIndex = 0;
            pauseMenu = false;
        }
        else
        {
            Destroy(gameObject);
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    // Use this for initialization
    void Start()
    {
        if (GameObject.Find("Jugador") != null)
        {
            Player = GameObject.Find("Jugador");
            playerMovement = Player.GetComponent<movement>();
            HUD_Script = Player.GetComponent<HUD>();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            hideMisionPoints();
            nextEvent = false;
            if (SceneManager.GetActiveScene().name == "sewer")
            {
                sewerLight = GameObject.Find("Torchs Sewer");
                normalLight = GameObject.Find("Directional Light");
            }
            switch (ActualMision)
            {
                case Misions.NONE:
                    if (GameObject.Find("RatHood") != null) GameObject.Find("RatHood").SetActive(false);
                    if (GameObject.Find("Secundary Camera") != null)
                    {
                        secundaryCamera = GameObject.Find("Secundary Camera");
                        secundaryCamera.SetActive(false);
                    }
                    if (GameObject.Find("Camera Destination") != null)
                    {
                        secundaryCameraDestination = GameObject.Find("Camera Destination");
                        secundaryCameraDestination.SetActive(false);
                    }
                    if (GameObject.Find("Torchs Sewer") != null)
                    {
                        sewerLight.SetActive(true);
                        normalLight.SetActive(false);
                    }
                    if (GameObject.Find("Enemigos M2") != null)
                    {
                        GameObject.Find("Enemigos M2").SetActive(false);
                    }
                        Player.transform.position = loadRespawn.NONE();
                    showMisionPoints();
                    break;
                case Misions.M1:
                    Player.transform.position = loadRespawn.M1(respawnIndex);
                    mainCamera = GameObject.Find("Main Camera");
                    secundaryCamera = GameObject.Find("Secundary Camera");
                    secundaryCameraDestination = GameObject.Find("Camera Destination");
                    switch (respawnIndex)
                    {
                        case 0:
                            normalLight.SetActive(true);
                            sewerLight.SetActive(false);
                            misionIndex = 0;
                            StartCoroutine(ExecuteAfterTime(5.0f));
                            break;
                        case 1:
                            secundaryCamera.SetActive(false);
                            secundaryCameraDestination.SetActive(false);
                            sewerLight.SetActive(false);
                            normalLight.SetActive(true);
                            HUD_Script.showM1Objective(1);
                            HUD_Script.showM1Helps(7, 38);
                            misionIndex = 9;
                            break;
                        case 2:
                            sewerLight.SetActive(true);
                            normalLight.SetActive(false);
                            secundaryCamera.SetActive(false);
                            secundaryCameraDestination.SetActive(false);
                            HUD_Script.showM1Objective(2);
                            HUD_Script.showM1Helps(10, 45);
                            misionIndex = 12;
                            break;
                    }

                    break;
                case Misions.M2:
                    Player.transform.position = loadRespawn.M2(respawnIndex);
                    mainCamera = GameObject.Find("Main Camera");
                    secundaryCamera = GameObject.Find("Secundary Camera");
                    secundaryCameraDestination = GameObject.Find("Camera Destination");
                    if (GameObject.Find("Enemigos_SM1") != null) GameObject.Find("Enemigos_SM1").transform.GetChild(0).gameObject.GetComponent<csAreaVision>().DestroyEnemy();
                    if (GameObject.Find("Enemigos_SM1") != null) GameObject.Find("Enemigos_SM1").transform.GetChild(1).gameObject.GetComponent<csAreaVision>().DestroyEnemy();
                    switch (respawnIndex)
                    {
                        case 0:
                            secundaryCamera.SetActive(false);
                            HUD_Script.showM2Objective(0, 40);
                            HUD_Script.showM2Helps(0, 70);
                            misionIndex = 0;
                            break;
                        case 1:
                            secundaryCamera.SetActive(false);
                            HUD_Script.showM2Objective(1);
                            HUD_Script.showM2Helps(0, 70);
                            misionIndex = 4;
                            break;
                        case 2:
                            secundaryCamera.SetActive(false);
                            HUD_Script.showM2Objective(2);
                            HUD_Script.showM2Helps(1, 70);
                            misionIndex = 5;
                            break;
                        default:
                            break;
                        case 3:
                            secundaryCamera.SetActive(false);
                            HUD_Script.showM2Objective(4);
                            HUD_Script.showM2Helps(2, 45);
                            misionIndex = 9;
                            break;
                        case 4:
                            secundaryCamera.SetActive(false);
                            HUD_Script.showM2Objective(4);
                            HUD_Script.showM2Helps(2, 45);
                            misionIndex = 10;
                            break;
                        case 5:
                            secundaryCamera.SetActive(false);
                            HUD_Script.showM2Objective(4);
                            HUD_Script.showM2Helps(2, 45);
                            misionIndex = 11;
                            break;
                    }
                    break;
                case Misions.SM_1:
                    RatHood.pointObject.SetActive(true);
                    Player.transform.position = loadRespawn.SM1(respawnIndex);
                    mainCamera = GameObject.Find("Main Camera");
                    if (GameObject.Find("Secundary Camera") != null) secundaryCamera = GameObject.Find("Secundary Camera");
                    if (GameObject.Find("Camera Destination") != null) secundaryCameraDestination = GameObject.Find("Camera Destination");
                    if(GameObject.Find("Enemigos M2") != null)  GameObject.Find("Enemigos M2").transform.GetChild(0).gameObject.GetComponent<csAreaVision>().DestroyEnemy();
                    switch (respawnIndex)
                    {
                        case 0:
                            misionIndex = 0;
                            StartCoroutine(ExecuteAfterTime(14.0f));
                            secundaryCamera.transform.position = new Vector3(31.6f, -22.98f, -42.68f);
                            secundaryCamera.transform.eulerAngles = new Vector3(30, 10, 0);
                            break;
                        case 1:
                            secundaryCamera.SetActive(false);
                            secundaryCameraDestination.SetActive(false);
                            misionIndex = 6;
                            break;
                        case 2:
                            Player.transform.position = new Vector3(-63.28f, -9.1f, 89.17f);
                            secundaryCamera.SetActive(false);
                            if (GameObject.Find("Directional Light") != null) GameObject.Find("Directional Light").SetActive(false);
                            RatHood.pointObject.GetComponentInChildren<UnityEngine.AI.NavMeshAgent>().Warp(new Vector3(-64.28f, -9.1f, 89.17f));
                            misionIndex = 8;
                            break;
                        case 3:
                            secundaryCamera.SetActive(false);
                            secundaryCameraDestination.SetActive(false);
                            misionIndex = 14;
                            break;
                    }
                    break;
            }
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ActualMision = Misions.M2;
            respawnIndex = 5;
            loadScreen.Instancia.CargarEscena("city");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu)
            {
                pauseMenu = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                pauseMenu = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        switch (ActualMision)
        {
            case Misions.NONE:
                break;
            case Misions.M1:
                M1();
                break;
            case Misions.M2:
                M2();
                break;
            case Misions.SM_1:
                SM1();
                break;
        }
    }

    void M1()
    {
        //MISION EVENTS
        switch (misionIndex)
        {
            case 0:
                Color c = GameObject.Find("Logo_M1").transform.GetChild(0).GetComponent<Image>().color;
                if (c.a < 1)
                {
                    c.a += 0.04f;
                    GameObject.Find("Logo_M1").transform.GetChild(0).GetComponent<Image>().color = c;
                }
                playerMovement.state = movement.playerState.HITTING;
                Player.GetComponent<Animator>().SetBool("Is_Draw", false);
                liquidState.hidratation = 100;
                if (nextEvent)
                {
                    misionIndex++;
                    nextEvent = false;
                    StartCoroutine(ExecuteAfterTime(7.0f));
                }
                break;
            case 1:
                Color c2 = GameObject.Find("Logo_M1").transform.GetChild(0).GetComponent<Image>().color;
                if (c2.a > 0)
                {
                    c2.a -= 0.04f;
                    GameObject.Find("Logo_M1").transform.GetChild(0).GetComponent<Image>().color = c2;
                }
                liquidState.hidratation = 100;
                secundaryCamera.transform.position = Vector3.Lerp(secundaryCamera.transform.position, secundaryCameraDestination.transform.position, 0.75f * Time.deltaTime);
                secundaryCamera.transform.rotation = Quaternion.Lerp(secundaryCamera.transform.rotation, secundaryCameraDestination.transform.rotation, 0.75f * Time.deltaTime);
                if (nextEvent)
                {
                    secundaryCameraDestination.transform.position = mainCamera.transform.position;
                    secundaryCameraDestination.transform.rotation = mainCamera.transform.rotation;
                    GameObject.Find("Logo_M1").SetActive(false);
                    StartCoroutine(ExecuteAfterTime(5.0f));
                    misionIndex++;
                    nextEvent = false;
                }
                break;
            case 2:
                liquidState.hidratation = 100;
                secundaryCamera.transform.position = Vector3.Lerp(secundaryCamera.transform.position, secundaryCameraDestination.transform.position, 1.25f * Time.deltaTime);
                secundaryCamera.transform.rotation = Quaternion.Lerp(secundaryCamera.transform.rotation, secundaryCameraDestination.transform.rotation, 1.25f * Time.deltaTime);
                if (nextEvent)
                {
                    secundaryCamera.transform.position = new Vector3(-40.49f, 2.57f, -18.57f);
                    secundaryCamera.transform.eulerAngles = new Vector3(0.0f, 18.425f, 0.0f);
                    secundaryCamera.SetActive(false);
                    secundaryCameraDestination.SetActive(false);
                    misionIndex++;
                    nextEvent = false;
                    HUD_Script.showM1Objective(0);
                    HUD_Script.showM1Helps(0, 60);
                    playerMovement.state = movement.playerState.IDLE;
                }
                break;
            case 3:
                liquidState.hidratation = 100;
                if (loadRespawn.BoxTriggers[0].activeSelf == false)
                {
                    Player.transform.position = new Vector3(-48.61f, 0.5040904f, -16.74f);
                    Player.transform.eulerAngles = new Vector3(0.0f, 147.341f, 0.0f);
                    playerMovement.state = movement.playerState.HITTING;
                    secundaryCamera.SetActive(true);
                    loadRespawn.Mision_Objects[2].SetActive(true);
                    loadRespawn.Mision_Objects[2].GetComponent<EnemyManager>().maxDist = 125;
                    loadRespawn.Mision_Objects[1].SetActive(true);
                    loadRespawn.Mision_Objects[1].GetComponent<csAreaVision>().speed = 10;
                    loadRespawn.Mision_Objects[1].GetComponent<csAreaVision>().actualState = csAreaVision.enemyState.PATROLLING;
                    misionIndex ++;
                    HUD_Script.showM1Helps(1, 45);
                    Player.GetComponent<Animator>().SetTrigger("Is_Withdrawing");
                    Player.GetComponent<Animator>().SetBool("Is_Detected", true);
                    StartCoroutine(ExecuteAfterTime(9.0f));
                }
                break;
            case 4:
                liquidState.hidratation = 100;
                loadRespawn.Mision_Objects[1].GetComponent<csAreaVision>().speed = 10;
                loadRespawn.Mision_Objects[1].GetComponent<csAreaVision>().actualState = csAreaVision.enemyState.PATROLLING;
                if (nextEvent)
                {
                    nextEvent = false;
                    misionIndex++;
                    secundaryCameraDestination.transform.position = mainCamera.transform.position;
                    secundaryCameraDestination.transform.rotation = mainCamera.transform.rotation;
                    HUD_Script.showM1Helps(2, 45);
                    StartCoroutine(ExecuteAfterTime(5.0f));
                }
                break;
            case 5:
                loadRespawn.Mision_Objects[1].GetComponent<csAreaVision>().speed = 10;
                loadRespawn.Mision_Objects[1].GetComponent<csAreaVision>().actualState = csAreaVision.enemyState.PATROLLING;
                liquidState.hidratation = 100;
                secundaryCamera.transform.position = Vector3.Lerp(secundaryCamera.transform.position, secundaryCameraDestination.transform.position, 1.25f * Time.deltaTime);
                secundaryCamera.transform.rotation = Quaternion.Lerp(secundaryCamera.transform.rotation, secundaryCameraDestination.transform.rotation, 1.25f * Time.deltaTime);
                if(nextEvent)
                {
                    secundaryCamera.SetActive(false);
                    nextEvent = false;
                    misionIndex++;
                    HUD_Script.showM1Helps(3, 40);
                }
                break;
            case 6:
                liquidState.hidratation = 100;
                if (GameObject.Find("Enemigo (3)") != null && GameObject.Find("Enemigo (3)").transform.GetChild(4).gameObject.activeSelf)
                {
                    HUD_Script.showM1Helps(4, 32);
                    GameObject.Find("Enemigo (3)").GetComponent<csAreaVision>().speed = 0;
                }
                if (GameObject.Find("Enemigo (3)") == null)
                {
                    misionIndex++;
                    HUD_Script.showM1Helps(5, 45);
                    Player.GetComponent<Animator>().SetBool("Is_Detected", true);
                    Player.GetComponent<Animator>().SetBool("Is_Draw", true);
                    playerMovement.state = movement.playerState.IDLE;
                }
                break;
            case 7:
                liquidState.hidratation = 100;
                if (loadRespawn.BoxTriggers[1].activeSelf == false)
                {
                    misionIndex++;
                    HUD_Script.showM1Helps(6, 35);
                }
                break;
            case 8:
                liquidState.hidratation = 100;
                if (loadRespawn.BoxTriggers[2].activeSelf == false)
                {
                    misionIndex++;
                    respawnIndex++;
                    HUD_Script.showM1Objective(1);
                    HUD_Script.showM1Helps(7, 38);
                }
                break;
            case 9:
                liquidState.hidratation = 100;
                if (loadRespawn.BoxTriggers[3].activeSelf == false)
                {
                    misionIndex++;
                    HUD_Script.showM1Helps(8, 38);
                }
                break;
            case 10:
                liquidState.hidratation = 100;
                if (loadRespawn.BoxTriggers[4].activeSelf == false)
                {
                    misionIndex++;
                    HUD_Script.showM1Helps(9, 50);
                }
                break;
            case 11:
                if (loadRespawn.BoxTriggers[5].activeSelf == false)
                {                   
                    misionIndex++;
                    respawnIndex++;
                    liquidState.hidratation = 10;
                    loadRespawn.Mision_Objects[0].SetActive(true);
                    for (int i = 0; i < loadRespawn.Mision_Objects[0].transform.childCount; i++) loadRespawn.Mision_Objects[0].transform.GetChild(i).gameObject.SetActive(true);
                    GameObject.Find("Zone_1").SetActive(false);
                    sewerLight.SetActive(true);
                    loadScreen.Instancia.CargarEscena("sewer");
                }
                break;
            case 12:
                if (loadRespawn.BoxTriggers[6].activeSelf == false)
                {
                    misionIndex++;
                    HUD_Script.showM1Helps(11, 38);
                }
                break;
            case 13:
                if (loadRespawn.BoxTriggers[7].activeSelf == false)
                {
                    ActualMision = Misions.NONE;
                    PrincipalMision.MisionsCompleted[0] = true;
                    loadRespawn.initialRespawn = Respawns.InitialRespawns.CITY_1;
                    loadScreen.Instancia.CargarEscena("city");
                }
                break;
            default:
                break;
        }
    }

    void M2()
    {
        //MISION EVENTS
        switch (misionIndex)
        {
            case 0:
                if (loadRespawn.BoxTriggers[0].activeSelf == false)
                {
                    misionIndex++;
                    playerMovement.state = movement.playerState.HITTING;
                    secundaryCamera.SetActive(true);
                    secundaryCamera.transform.position = new Vector3(66.5f, -13.7f, -29.8f);
                    secundaryCamera.transform.eulerAngles = new Vector3(14.901f, 448.579f, -0.9480001f);
                    Player.transform.position = new Vector3(36.38f, -27.52068f, -34.99f);
                    StartCoroutine(ExecuteAfterTime(2.5f));
                }
                break;
            case 1:
                if(nextEvent)
                {
                    misionIndex++;
                    secundaryCameraDestination.transform.position = mainCamera.transform.position + new Vector3( 0.0f, 7.0f, 0.0f);
                    secundaryCameraDestination.transform.rotation = mainCamera.transform.rotation;
                    StartCoroutine(ExecuteAfterTime(3.25f));
                    nextEvent = false;
                }
                break;
            case 2:
                secundaryCamera.transform.position = Vector3.Lerp(secundaryCamera.transform.position, secundaryCameraDestination.transform.position, 0.75f * Time.deltaTime);
                secundaryCamera.transform.rotation = Quaternion.Lerp(secundaryCamera.transform.rotation, secundaryCameraDestination.transform.rotation, 0.75f * Time.deltaTime);
                if (nextEvent)
                {
                    misionIndex++;
                    secundaryCameraDestination.transform.position = mainCamera.transform.position;
                    secundaryCameraDestination.transform.rotation = mainCamera.transform.rotation;
                    StartCoroutine(ExecuteAfterTime(3.0f));
                    nextEvent = false;
                }
                break;
            case 3:
                secundaryCamera.transform.position = Vector3.Lerp(secundaryCamera.transform.position, secundaryCameraDestination.transform.position, 1.25f * Time.deltaTime);
                secundaryCamera.transform.rotation = Quaternion.Lerp(secundaryCamera.transform.rotation, secundaryCameraDestination.transform.rotation, 1.25f * Time.deltaTime);
                if (nextEvent)
                {
                    playerMovement.state = movement.playerState.IDLE;
                    HUD_Script.showM2Objective(1);
                    HUD_Script.showM2Helps(0, 70);
                    secundaryCamera.SetActive(false);
                    nextEvent = false;
                    misionIndex++;
                    respawnIndex++;
                }
                break;
            case 4:
                if (loadRespawn.BoxTriggers[1].activeSelf == false)
                {
                    respawnIndex++;
                    misionIndex++;
                    loadRespawn.initialRespawn = Respawns.InitialRespawns.PUB_INSIDE;
                    loadScreen.Instancia.CargarEscena("PUB");
                }
                break;
            case 5:
                servantUnlock_NPC.canTalk = true;
                if(nextEvent)
                {
                    loadRespawn.Mision_Objects[1].SetActive(true);
                    nextEvent = false;
                    misionIndex++;
                    playerMovement.state = movement.playerState.HITTING;
                    secundaryCamera.SetActive(true);
                    secundaryCamera.transform.position = mainCamera.transform.position;
                    secundaryCamera.transform.rotation = mainCamera.transform.rotation;
                    secundaryCameraDestination.transform.position = new Vector3(9.91f, 14.42f, -18.64f);
                    secundaryCameraDestination.transform.eulerAngles = new Vector3(14.901f, 665.441f, -0.9480001f);
                }
                break;
            case 6:
                secundaryCamera.transform.position = Vector3.Lerp(secundaryCamera.transform.position, secundaryCameraDestination.transform.position, 0.75f * Time.deltaTime);
                secundaryCamera.transform.rotation = Quaternion.Lerp(secundaryCamera.transform.rotation, secundaryCameraDestination.transform.rotation, 0.75f * Time.deltaTime);
                if (nextEvent)
                {
                    misionIndex++;
                    nextEvent = false;
                }
                break;
            case 7:
                secundaryCamera.transform.position = loadRespawn.Mision_Objects[1].transform.position + new Vector3(1.0f, 2.0f, -1.0f);
                if (nextEvent)
                {
                    nextEvent = false;
                    playerMovement.state = movement.playerState.IDLE;
                    HUD_Script.showM2Objective(3);
                    secundaryCamera.SetActive(false);
                    misionIndex++;
                    loadRespawn.Mision_Objects[1].SetActive(false);
                    loadRespawn.BoxTriggers[2].SetActive(true);
                }
                break;
            case 8:
                if(loadRespawn.BoxTriggers[2].activeSelf == false)
                {
                    misionIndex++;
                    respawnIndex++;
                    loadRespawn.initialRespawn = Respawns.InitialRespawns.PUB_OUTSIDE;
                    loadScreen.Instancia.CargarEscena("city");
                }
                break;
            case 9:
                if (loadRespawn.BoxTriggers[3].activeSelf == false)
                {
                    misionIndex++;
                    respawnIndex++;
                }
                break;
             case 10:
                if (loadRespawn.BoxTriggers[4].activeSelf == false)
                {
                    misionIndex++;
                    respawnIndex++;
                }
                break;
            case 11:
                if(nextEvent)
                {
                    misionIndex++;
                    HUD_Script.showM2Objective(5);
                    HUD_Script.showM2Helps(0, 70);
                    nextEvent = false;
                }
                break;
            case 12:
                if (loadRespawn.BoxTriggers[5].activeSelf == false)
                {
                    misionIndex++;
                    respawnIndex++;
                    nextEvent = false;
                    ActualMision = Misions.NONE;
                    PrincipalMision.MisionsCompleted[1] = true;
                    loadRespawn.initialRespawn = Respawns.InitialRespawns.PUB_OUTSIDE;
                    loadScreen.Instancia.CargarEscena("city");
                }
                break;
        }
    }

    void SM1()
    {
        switch (misionIndex)
        {
            case 0:
                playerMovement.state = movement.playerState.HITTING;
                Player.transform.position = new Vector3(30.765f, -27.523f, -38.321f);
                Player.transform.eulerAngles = new Vector3(0.0f, 90f, 0.0f);
                if (nextEvent)
                {
                    misionIndex++;
                    nextEvent = false;
                    Player.transform.position = new Vector3(31.6725f, -27.523f, -38.321f);
                    Player.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                    secundaryCamera.SetActive(false);
                    playerMovement.state = movement.playerState.IDLE;
                    HUD_Script.showSM_1Dialog(0, 60, (Screen.width * 5) / 9, (Screen.height) / 9, 300);
                    if (GameObject.Find("Enemigos_SM1") != null) GameObject.Find("Enemigos_SM1").transform.GetChild(0).gameObject.GetComponent<csAreaVision>().DestroyEnemy();
                    if (GameObject.Find("Enemigos_SM1") != null) GameObject.Find("Enemigos_SM1").transform.GetChild(1).gameObject.GetComponent<csAreaVision>().DestroyEnemy();
                }
                break;
            case 1:
                if (loadRespawn.BoxTriggers[0].activeSelf == false)
                {
                    playerMovement.state = movement.playerState.HITTING;
                    Player.transform.position = new Vector3(32.44f, -27.523f, -43f);
                    Player.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                    misionIndex++;
                    HUD_Script.showSM_1Dialog(1, 40, (Screen.width * 8) / 13, (Screen.height * 10) / 12, 300);
                    secundaryCamera.SetActive(true);
                    secundaryCamera.transform.position = new Vector3(33.09f, -23.33f, -38.72f);
                    secundaryCamera.transform.eulerAngles = new Vector3(30.0f, 180.0f, 0.0f);
                }
                break;
            case 2:
                if (HUD.finalTime - HUD.startTime > HUD.timeUntilDisapear)
                {
                    HUD_Script.showSM_1Dialog(2, 50, (Screen.width * 8) / 13, (Screen.height * 10) / 12, 300);
                    misionIndex++;
                }
                break;
            case 3:
                if (HUD.finalTime - HUD.startTime > HUD.timeUntilDisapear)
                {
                    HUD_Script.showSM_1Dialog(3, 50, (Screen.width * 7) / 11, (Screen.height * 9) / 12, 300);
                    misionIndex++;
                }
                break;
            case 4:
                if (HUD.finalTime - HUD.startTime > HUD.timeUntilDisapear)
                {
                    HUD_Script.showSM_1Dialog(4, 40, (Screen.width * 8) / 13, (Screen.height * 10) / 12, 150);
                    misionIndex++;
                }
                break;
            case 5:
                if (HUD.finalTime - HUD.startTime > HUD.timeUntilDisapear + 10)
                {
                    playerMovement.state = movement.playerState.IDLE;
                    HUD_Script.showSM_1Objective(0);
                    HUD_Script.showSM_1Helps(0, 38);
                    secundaryCamera.SetActive(false);
                    misionIndex++;
                    respawnIndex++;
                }
                break;
            case 6:
                if (loadRespawn.BoxTriggers[1].activeSelf == false)
                {
                    misionIndex++;
                    respawnIndex++;
                    loadRespawn.initialRespawn = Respawns.InitialRespawns.SEWER_1;
                    loadScreen.Instancia.CargarEscena("sewer");
                }
                break;
            case 7:
                if (SceneManager.GetActiveScene().name == "sewer")
                {
                    HUD_Script.showSM_1Dialog(5, 50, (Screen.width * 8) / 13, (Screen.height * 10) / 12, 300);
                    misionIndex++;
                }
                break;
            case 8:
                if (loadRespawn.BoxTriggers[2].activeSelf == false)
                {
                    misionIndex++;
                    secundaryCamera.SetActive(true);
                    Player.transform.position = new Vector3(-59.11f, -9.3985f, 108.4447f);
                    Player.transform.eulerAngles = new Vector3(0f, 10f, 0f);
                    secundaryCamera.transform.position = new Vector3(-63.08f, -3.9f, 105.13f);
                    secundaryCamera.transform.eulerAngles = new Vector3(30.0f, 40.0f, 0.0f);
                    playerMovement.state = movement.playerState.HITTING;
                    HUD_Script.showSM_1Dialog(6, 35, (Screen.width * 7) / 13, (Screen.height * 10) / 13, 300);
                }
                break;
            case 9:
                if (HUD.finalTime - HUD.startTime > HUD.timeUntilDisapear)
                {
                    HUD_Script.showSM_1Dialog(7, 35, (Screen.width * 7) / 13, (Screen.height * 10) / 13, 300);
                    misionIndex++;
                }
                break;
            case 10:
                if (HUD.finalTime - HUD.startTime > HUD.timeUntilDisapear)
                {
                    HUD_Script.showSM_1Dialog(8, 50, (Screen.width * 7) / 13, (Screen.height * 10) / 13, 150);
                    misionIndex++;
                }
                break;
            case 11:
                if (HUD.finalTime - HUD.startTime > HUD.timeUntilDisapear + 10)
                {
                    misionIndex++;
                    StartCoroutine(ExecuteAfterTime(5.0f));
                }
                break;
            case 12:
                if (nextEvent)
                {
                    secundaryCamera.SetActive(false);
                    HUD_Script.showSM_1Objective(1);
                    HUD_Script.showSM_1Helps(1, 38);
                    nextEvent = false;
                    playerMovement.state = movement.playerState.IDLE;
                    RatHood.pointObject.GetComponentInChildren<UnityEngine.AI.NavMeshAgent>().Warp(new Vector3(14.32f, -9.1f, 150.9f));
                    misionIndex++;
                }
                break;
            case 13:
                if (loadRespawn.BoxTriggers[3].activeSelf == false)
                {
                    misionIndex++;
                    respawnIndex++;
                    Player.transform.position = new Vector3(14.25f, -9.3985f, 143.88f);
                    Player.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                    secundaryCamera.SetActive(true);
                    secundaryCamera.transform.position = new Vector3(14.27f, -4.5f, 140.0f);
                    secundaryCamera.transform.eulerAngles = new Vector3(20.0f, 0.0f, 0.0f);
                    playerMovement.state = movement.playerState.HITTING;
                    HUD_Script.showSM_1Dialog(9, 50, (Screen.width * 8) / 13, (Screen.height * 10) / 12, 300);
                }
                break;
            case 14:
                if (HUD.finalTime - HUD.startTime > HUD.timeUntilDisapear)
                {
                    HUD_Script.showSM_1Dialog(10, 50, (Screen.width * 8) / 13, (Screen.height * 10) / 12, 400);
                    misionIndex++;
                }
                break;
            case 15:
                if (HUD.finalTime - HUD.startTime > HUD.timeUntilDisapear)
                {
                    HUD_Script.showSM_1Objective(2);
                    secundaryCamera.SetActive(false);
                    playerMovement.state = movement.playerState.IDLE;
                    misionIndex++;
                }
                break;
            case 16:
                if (loadRespawn.BoxTriggers[4].activeSelf == false)
                {
                    ActualMision = Misions.NONE;
                    RatHood.MisionsCompleted[(int)Misions.SM_1 - 4] = true;
                    loadRespawn.initialRespawn = Respawns.InitialRespawns.CITY_1;
                    loadScreen.Instancia.CargarEscena("city");
                }
                break;
        }
    }

    void showMisionPoints()
    {
        if (PrincipalMision.MisionsCompleted[(int)Misions.M3] == false)
        {
            if (PrincipalMision.MisionsCompleted[(int)Misions.M2])
            {
                PrincipalMision.pointObject.SetActive(true);
                PrincipalMision.pointObject.transform.position = new Vector3(80.73f, -31.81266f, 189.97f) + transform.position;
            }
            else
            {
                {
                    PrincipalMision.pointObject.SetActive(true);
                    PrincipalMision.pointObject.transform.position = new Vector3(-14.1f, -31.81266f, -12.6f) + transform.position;
                }
            }
            if (RatHood.MisionsCompleted[(int)Misions.SM_1 - 4] == false)
            {
                RatHood.pointObject.SetActive(true);
                //RatHood.pointObject.transform.position = new Vector3(13.17202f, -31.81266f, -18.64425f) + transform.position;
            }
            if (RebelCat.MisionsCompleted[(int)Misions.SM_2 - 5] == false)
            {
                RebelCat.pointObject.SetActive(true);
                RebelCat.pointObject.transform.position = new Vector3(-49.41798f, -31.81266f, 48.72575f) + transform.position;
            }
            if (ScaryDog.MisionsCompleted[(int)Misions.SM_3 - 6] == false)
            {
                ScaryDog.pointObject.SetActive(true);
                ScaryDog.pointObject.transform.position = new Vector3(-95.95798f, -31.81266f, 5.595755f) + transform.position;
            }
        }
        else
        {
            PrincipalMision.pointObject.SetActive(true);
            PrincipalMision.pointObject.transform.position = new Vector3(78.94f, -31.81266f, 275.4f) + transform.position;
            if (RatHood.MisionsCompleted[(int)Misions.SM_1 - 4] == false)
            {
                RatHood.pointObject.SetActive(true);
                //RatHood.pointObject.transform.position = new Vector3(13.17202f, -31.81266f, -18.64425f) + transform.position;
            }
            else if (RatHood.MisionsCompleted[(int)Misions.SM_4 - 6] == false)
            {
                RatHood.pointObject.SetActive(true);
                RatHood.pointObject.transform.position = new Vector3(-14.7f, -31.81266f, 178.6f) + transform.position;
            }
            if (RebelCat.MisionsCompleted[(int)Misions.SM_2 - 5] == false)
            {
                RebelCat.pointObject.SetActive(true);
                RebelCat.pointObject.transform.position = new Vector3(-49.41798f, -31.81266f, 48.72575f) + transform.position;
            }
            else if (RebelCat.MisionsCompleted[(int)Misions.SM_5 - 7] == false)
            {
                RebelCat.pointObject.SetActive(true);
                RebelCat.pointObject.transform.position = new Vector3(-96.1f, -31.81266f, 133.5f) + transform.position;
            }
            if (ScaryDog.MisionsCompleted[(int)Misions.SM_3 - 6] == false)
            {
                ScaryDog.pointObject.SetActive(true);
                ScaryDog.pointObject.transform.position = new Vector3(-95.95798f, -31.81266f, 5.595755f) + transform.position;
            }
            else if (ScaryDog.MisionsCompleted[(int)Misions.SM_6 - 8] == false)
            {
                ScaryDog.pointObject.SetActive(true);
                ScaryDog.pointObject.transform.position = new Vector3(-18.41f, -31.81266f, 293.47f) + transform.position;
            }
        }
    }

    void hideMisionPoints()
    {
        RatHood.pointObject.SetActive(false);
        ScaryDog.pointObject.SetActive(false);
        RebelCat.pointObject.SetActive(false);
        PrincipalMision.pointObject.SetActive(false);
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        nextEvent = true;
    }

    void OnLevelWasLoaded()
    {
        Debug.Log("LOADED");
        loadRespawn.setAllFalse();
        if (SceneManager.GetActiveScene().name != "DEAD")
        {
            if (SceneManager.GetActiveScene().name == "Menu_1")
            {
                respawnIndex = 0;
                ActualMision = Misions.NONE;
            }
            else Start();
        }
    }
}
