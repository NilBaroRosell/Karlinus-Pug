using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class misions : MonoBehaviour {


    public enum Misions { M1, M2, M3, M4, SM_1 = 0, SM_2 = 0, SM_3 = 0, SM_4 = 1, SM_5 = 1, SM_6 = 1, NONE = 10 };
    public Misions ActualMision;
    private int respawnIndex;
    private int misionIndex;
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
            Instance = this;
        }
        else
        {
            DestroyObject(gameObject);
        }
    }
    // Use this for initialization
    void Start() {
        if (GameObject.Find("Jugador") != null)
        {
            Player = GameObject.Find("Jugador");
            playerMovement = Player.GetComponent<movement>();
            HUD_Script = Player.GetComponent<HUD>();
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
                            misionIndex = 4;
                            break;
                            case 2:
                            secundaryCamera.SetActive(false);
                            secundaryCameraDestination.SetActive(false);
                            misionIndex = 5;
                            break;
                    }
                    
                    break;
                case Misions.M2:
                    break;
                default:
                    break;
            }
        }
        else Cursor.lockState = CursorLockMode.None;
        Debug.Log(misionIndex);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (Input.GetKeyDown(KeyCode.C) && Cursor.lockState == CursorLockMode.Locked) Cursor.lockState = CursorLockMode.None;
        else if (Input.GetKeyDown(KeyCode.C) && Cursor.lockState == CursorLockMode.None) Cursor.lockState = CursorLockMode.Locked;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(gameObject);
            SceneManager.LoadScene("Menu_1");
        }
        switch (ActualMision)
        {
            case Misions.NONE:
                break;
            case Misions.M1:
                M1();
                break;

        }
    }

    void M1()
    {
        //MISION EVENTS
        switch (misionIndex)
        {
            case 0:
                playerMovement.state = movement.playerState.HITTING;
                if (nextEvent)
                {
                    misionIndex++;
                    nextEvent = false;
                    StartCoroutine(ExecuteAfterTime(7.0f));
                }
                break;
            case 1:
                secundaryCamera.transform.position = Vector3.Lerp(secundaryCamera.transform.position, secundaryCameraDestination.transform.position, 0.75f * Time.deltaTime);
                secundaryCamera.transform.rotation = Quaternion.Lerp(secundaryCamera.transform.rotation, secundaryCameraDestination.transform.rotation, 0.75f * Time.deltaTime);
                if (nextEvent)
                {
                    secundaryCameraDestination.transform.position = mainCamera.transform.position;
                    secundaryCameraDestination.transform.rotation = mainCamera.transform.rotation;
                    StartCoroutine(ExecuteAfterTime(5.0f));
                    misionIndex++;
                    nextEvent = false;
                }
                break;
            case 2:
                secundaryCamera.transform.position = Vector3.Lerp(secundaryCamera.transform.position, secundaryCameraDestination.transform.position, 1.25f * Time.deltaTime);
                secundaryCamera.transform.rotation = Quaternion.Lerp(secundaryCamera.transform.rotation, secundaryCameraDestination.transform.rotation, 1.25f * Time.deltaTime);
                if (nextEvent)
                {
                    secundaryCamera.SetActive(false);
                    secundaryCameraDestination.SetActive(false);
                    misionIndex++;
                    nextEvent = false;
                    HUD.canvasHUD.SetActive(true);
                    HUD_Script.showM1Objective(0);
                    playerMovement.state = movement.playerState.IDLE;
                }
                break;
            case 3:
                if (loadRespawn.BoxTriggers[0].activeSelf == false)
                {
                    misionIndex++;
                    respawnIndex++;
                    HUD_Script.showM1Objective(1);
                }
                break;
            case 4:
                if (loadRespawn.BoxTriggers[1].activeSelf == false)
                {
                    misionIndex++;
                    respawnIndex++;
                    loadRespawn.Mision_Objects[0].SetActive(true);
                    GameObject.Find("Zone_1").SetActive(false);
                    HUD_Script.showM1Objective(2);
                }
                break;
            case 5:
                if (loadRespawn.BoxTriggers[2].activeSelf == false)
                {
                    ActualMision = Misions.NONE;
                    PrincipalMision.MisionsCompleted[0] = true;
                    loadRespawn.initialRespawn = Respawns.InitialRespawns.CITY_1;
                    SceneManager.LoadScene("city");
                }
                break;
            default:
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
            if (RatHood.MisionsCompleted[(int)Misions.SM_1] == false)
            {
                RatHood.pointObject.SetActive(true);
                RatHood.pointObject.transform.position = new Vector3(13.17202f, -31.81266f, -18.64425f) + transform.position;
            }
            if (RebelCat.MisionsCompleted[(int)Misions.SM_2] == false)
            {
                RebelCat.pointObject.SetActive(true);
                RebelCat.pointObject.transform.position = new Vector3(-49.41798f, -31.81266f, 48.72575f) + transform.position;
            }
            if (ScaryDog.MisionsCompleted[(int)Misions.SM_3] == false)
            {
                ScaryDog.pointObject.SetActive(true);
                ScaryDog.pointObject.transform.position = new Vector3(-95.95798f, -31.81266f, 5.595755f) + transform.position;
            }
        }
        else
        {
            PrincipalMision.pointObject.SetActive(true);
            PrincipalMision.pointObject.transform.position = new Vector3(78.94f, -31.81266f, 275.4f) + transform.position;
            if (RatHood.MisionsCompleted[(int)Misions.SM_1] == false)
            {
                RatHood.pointObject.SetActive(true);
                RatHood.pointObject.transform.position = new Vector3(13.17202f, -31.81266f, -18.64425f) + transform.position;
            }
            else if (RatHood.MisionsCompleted[(int)Misions.SM_4] == false)
            {
                RatHood.pointObject.SetActive(true);
                RatHood.pointObject.transform.position = new Vector3(-14.7f, -31.81266f, 178.6f) + transform.position;
            }
            if (RebelCat.MisionsCompleted[(int)Misions.SM_2] == false)
            {
                RebelCat.pointObject.SetActive(true);
                RebelCat.pointObject.transform.position = new Vector3(-49.41798f, -31.81266f, 48.72575f) + transform.position;
            }
            else if (RebelCat.MisionsCompleted[(int)Misions.SM_5] == false)
            {
                RebelCat.pointObject.SetActive(true);
                RebelCat.pointObject.transform.position = new Vector3(-96.1f, -31.81266f, 133.5f) + transform.position;
            }
            if (ScaryDog.MisionsCompleted[(int)Misions.SM_3] == false)
            {
                ScaryDog.pointObject.SetActive(true);
                ScaryDog.pointObject.transform.position = new Vector3(-95.95798f, -31.81266f, 5.595755f) + transform.position;
            }
            else if (ScaryDog.MisionsCompleted[(int)Misions.SM_6] == false)
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
            Start();
        }
    }
}
