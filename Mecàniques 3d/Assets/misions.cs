using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.AI;

public class misions : MonoBehaviour {

    public GameObject normalLight;
    public GameObject sewerLight;
    public enum Misions { M1, M2, M3, M4, SM_1, SM_2, SM_3, SM_4, SM_5, SM_6, NONE };
    public Misions ActualMision;
    public static int respawnIndex;
    public static int misionIndex;
    private Respawns loadRespawn;
    private HUD HUD_Script;
    private GameObject Player;
    private movement playerMovement;
    private bool nextEvent;
    private GameObject mainCamera;
    private GameObject secundaryCamera;
    private GameObject secundaryCameraDestination;
    private static misions Instance;
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
    public static bool changeSceneSM1 = false;
    public int indexMision;
    public int indexRespawn;

    /*public enum Misions { M1, M2, M3, M4, SM_1, SM_2, SM_3, SM_4, SM_5, SM_6, NONE };
    public GameObject normalLight;
    public GameObject sewerLight;

    public Misions ActualMision;
    public static int respawnIndex;
    public static int misionIndex;
    public Respawns loadRespawn;
    private HUD HUD_Script;
    private GameObject Player;
    private GameObject reference;
    private movement playerMovement;
    private bool nextEvent, nextEvent2;
    private GameObject mainCamera;
    private GameObject secundaryCamera;
    private GameObject secundaryCameraDestination;
    private static misions Instance;
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
    private float startTime;
    private float finishTime;
    private bool firstTime = false, first = true;
    public int indexMission;
    public bool entra = false;
    private GameObject enemiesSM2;*/

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
        {
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
            respawnIndex = 0;
            pauseMenu = false;
            Instance = this;
        }
        else
        {
            DestroyObject(gameObject);
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    /*private void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
        {
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
        nextEvent2 = false;
            respawnIndex = 0;
            pauseMenu = false;
            Instance = this;
        }
        else
        {
            DestroyObject(gameObject);
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (GameObject.Find("Enemigos_SM2") != null) enemiesSM2 = GameObject.Find("Enemigos_SM2");
        enemiesSM2.SetActive(false);
    }*/
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
            switch (ActualMision)
            {
                case Misions.NONE:
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
                            misionIndex = 0;
                            StartCoroutine(ExecuteAfterTime(5.0f));
                            break;
                        case 1:
                            secundaryCamera.SetActive(false);
                            secundaryCameraDestination.SetActive(false);
                            misionIndex = 9;
                            break;
                        case 2:
                            secundaryCamera.SetActive(false);
                            secundaryCameraDestination.SetActive(false);
                            HUD_Script.showM1Objective(2);
                            HUD_Script.showM1Helps(10, 45);
                            misionIndex = 12;
                            break;
                    }

                    break;
                case Misions.M2:
                    break;
                case Misions.SM_1:
                    RatHood.pointObject.SetActive(true);
                    Player.transform.position = loadRespawn.SM1(respawnIndex);
                    mainCamera = GameObject.Find("Main Camera");
                    if(GameObject.Find("Secundary Camera")!=null) secundaryCamera = GameObject.Find("Secundary Camera");
                    if (GameObject.Find("Camera Destination") != null) secundaryCameraDestination = GameObject.Find("Camera Destination");
                    switch (respawnIndex)
                    {
                        case 0:
                            misionIndex = 0;
                            StartCoroutine(ExecuteAfterTime(5.0f));//14
                            break;
                        case 1:
                            secundaryCamera.SetActive(false);
                            secundaryCameraDestination.SetActive(false);
                            misionIndex = 3;
                            break;
                        case 2:
                            secundaryCamera.SetActive(false);
                            secundaryCameraDestination.SetActive(false);
                            RatHood.pointObject.GetComponent<NavMeshAgent>().Warp(new Vector3(-64.28f, -9.1f, 89.17f));
                            misionIndex = 4;
                            break;
                        case 3:
                            secundaryCamera.SetActive(false);
                            secundaryCameraDestination.SetActive(false);
                            misionIndex = 9;
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    /*void Start()
    {
        if (GameObject.Find("Jugador") != null)
        {
            Player = GameObject.Find("Jugador");
            reference = GameObject.Find("Referencia");
            playerMovement = Player.GetComponent<movement>();
            HUD_Script = Player.GetComponent<HUD>();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            hideMisionPoints();
            nextEvent = false;
            nextEvent2 = false;
            switch (ActualMision)
            {
                case Misions.NONE:
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
                            misionIndex = 0;
                            StartCoroutine(ExecuteAfterTime(5.0f));
                            break;
                        case 1:
                            secundaryCamera.SetActive(false);
                            secundaryCameraDestination.SetActive(false);
                            misionIndex = 9;
                            break;
                        case 2:
                            secundaryCamera.SetActive(false);
                            secundaryCameraDestination.SetActive(false);
                            HUD_Script.showM1Objective(2);
                            HUD_Script.showM1Helps(10, 45);
                            misionIndex = 12;
                            break;
                    }

                    break;
                case Misions.M2:
                    break;
                case Misions.SM_2:
                    Player.transform.position = loadRespawn.SM2(respawnIndex);
                    mainCamera = GameObject.Find("Main Camera");
                    secundaryCamera = GameObject.Find("Secundary Camera");
                    secundaryCameraDestination = GameObject.Find("Camera Destination");
                    if (GameObject.Find("Enemigos_SM2") != null) GameObject.Find("Enemigos_SM2").SetActive(false);
                    break;
                default:
                    break;
            }
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }*/

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.C) && Cursor.lockState == CursorLockMode.Locked) Cursor.lockState = CursorLockMode.None;
        else if (Input.GetKeyDown(KeyCode.C) && Cursor.lockState == CursorLockMode.None) Cursor.lockState = CursorLockMode.Locked;
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
                if (movement.startSM2)
                {
                    ActualMision = Misions.SM_1;
                    loadScreen.Instancia.CargarEscena("city");
                }
                break;
            case Misions.M1:
                M1();
                break;
            case Misions.SM_1:
                SM1();
                break;
        }
        indexMision = misionIndex;
        indexRespawn = respawnIndex;
    }

    /*void FixedUpdate() {
        if (Input.GetKeyDown(KeyCode.C) && Cursor.lockState == CursorLockMode.Locked) Cursor.lockState = CursorLockMode.None;
        else if (Input.GetKeyDown(KeyCode.C) && Cursor.lockState == CursorLockMode.None) Cursor.lockState = CursorLockMode.Locked;
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
                ActualMision = Misions.NONE;
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
            case Misions.SM_2:
                SM2();
                break;

        }

        indexMission = misionIndex;
    }*/

    void M1()
    {
        //MISION EVENTS
        switch (misionIndex)
        {
            case 0:
                normalLight.SetActive(true);
                sewerLight.SetActive(false);
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
                    misionIndex++;
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
                if (nextEvent)
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
                    normalLight.SetActive(false);
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

    void SM1()
    {
        switch (misionIndex)
        {
            case 0: // primera camera (mirant als gats)
                playerMovement.state = movement.playerState.HITTING;
                Player.transform.position = new Vector3(30.765f, -27.523f, -38.321f);
                Player.transform.eulerAngles = new Vector3(0.0f, 90f, 0.0f);
                /*reference.transform.position = Player.transform.position;
                reference.transform.rotation = Player.transform.rotation;*/
                
                // si han passat els gats
                if (nextEvent)
                {
                    misionIndex++;
                    nextEvent = false;
                    Player.transform.position = new Vector3(31.6725f, -27.523f, -38.321f);
                    //reference.transform.position = Player.transform.position;
                    Player.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                    //reference.transform.rotation = Player.transform.rotation;
                    /*secundaryCamera.transform.position = Vector3.Lerp(secundaryCamera.transform.position, secundaryCameraDestination.transform.position, 0.25f * Time.deltaTime);
                    secundaryCamera.transform.rotation = Quaternion.Lerp(secundaryCamera.transform.rotation, secundaryCameraDestination.transform.rotation, 0.25f * Time.deltaTime);*/
                    secundaryCamera.SetActive(false);
                    secundaryCameraDestination.SetActive(false);
                    playerMovement.state = movement.playerState.IDLE;
                }
                break;
            case 1: // anar fins al rat hood
                
                if (loadRespawn.BoxTriggers[0].activeSelf == false)
                {
                    playerMovement.state = movement.playerState.HITTING;
                    Player.transform.position = new Vector3(32.44f, -27.523f, -43f);
                    Player.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                    /*reference.transform.position = Player.transform.position;
                    reference.transform.rotation = Player.transform.rotation;*/
                    misionIndex++;
                    StartCoroutine(ExecuteAfterTime(5.0f));// 10
                }
                break;
            case 2: // anar fins a l'entrada de les clavagueres
                if(nextEvent)
                {
                    nextEvent = false;
                    playerMovement.state = movement.playerState.IDLE;
                    misionIndex++;
                    respawnIndex++;
                }
                break;
            case 3:
                if (loadRespawn.BoxTriggers[1].activeSelf == false)
                {
                    misionIndex++;
                    respawnIndex++;
                    changeSceneSM1 = true;
                    LoadScene.respawnToLoad = LoadScene.Scenes.SEWER_1;
                    /*if (GameObject.Find("RatHood") != null) GameObject.Find("RatHood")*/

                }
                break;
            case 4:
                if (loadRespawn.BoxTriggers[2].activeSelf == false)
                {
                    misionIndex++;
                    playerMovement.state = movement.playerState.HITTING;
                    StartCoroutine(ExecuteAfterTime(5.0f));// 10
                }
                break;
            case 5:
                if (nextEvent)
                {
                    misionIndex++;
                    playerMovement.state = movement.playerState.HITTING;
                    StartCoroutine(ExecuteAfterTime(5.0f));// 5 (parlar amb rat hood)
                    nextEvent = false;
                }
                break;
            case 6:
                if (nextEvent)
                {
                    misionIndex++;
                    StartCoroutine(ExecuteAfterTime(5.0f));// 5 (deixar marxar a rat hood)
                    nextEvent = false;
                }
                break;
            case 7:
                if(nextEvent)
                {
                    nextEvent = false;
                    playerMovement.state = movement.playerState.IDLE;
                    RatHood.pointObject.GetComponent<NavMeshAgent>().Warp(new Vector3(14.32f, -9.1f, 150.9f));
                    misionIndex++;
                }
                break;
            case 8:
                if(loadRespawn.BoxTriggers[3].activeSelf == false)
                {
                    misionIndex++;
                    playerMovement.state = movement.playerState.HITTING;
                    StartCoroutine(ExecuteAfterTime(5.0f));// 10
                }
                break;
            case 9:
                if (nextEvent)
                {
                    nextEvent = false;
                    playerMovement.state = movement.playerState.IDLE;
                    misionIndex++;
                    respawnIndex++;
                }
                break;
            case 10:
                if(loadRespawn.BoxTriggers[4].activeSelf == false)
                {
                    ActualMision = Misions.NONE;
                    RatHood.MisionsCompleted[(int)Misions.SM_1 - 4] = true;
                    changeSceneSM1 = true;
                    LoadScene.respawnToLoad = LoadScene.Scenes.CITY_1;
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
                RatHood.pointObject.transform.position = new Vector3(13.17202f, -31.81266f, -18.64425f) + transform.position - new Vector3 (1.6775f, 2.1f, -4.12f);
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
                RatHood.pointObject.transform.position = new Vector3(13.17202f, -31.81266f, -18.64425f) + transform.position - new Vector3 (1.6775f, 2.1f, -4.12f);
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
        if (SceneManager.GetActiveScene().name != "DEAD")
        {
            Debug.Log("LOADED");
            loadRespawn.setAllFalse();
            if (SceneManager.GetActiveScene().name == "Menu_1") respawnIndex = 0;
            Start();
        }
    }
}
