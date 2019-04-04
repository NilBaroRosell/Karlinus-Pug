using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {


    public enum states { PLAYING, SHOWING_LOGOS, MAIN_MENU, TRAVELLING_TO_PLAYER };

    public GameObject gameLogo;
    public GameObject gameLogo2;
    public GameObject teamLogo;
    public GameObject logosBackground;
    public GameObject start;
    public GameObject startButton;
    public GameObject cont;
    public GameObject back;
    public GameObject continueButton;
    public GameObject optionsButton;
    public GameObject exitButton;
    public GameObject backButton;
    public GameObject applyButton;
    public bool showMenu;
    private float logoReference;
    private float menuReference;
    public static MainMenu Instance;
    public states state;
    public GameObject secundaryCamera;
    private Vector3 camOriginalPos;
    private Quaternion camOriginalRotation;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        gameLogo.SetActive(true);
        gameLogo2.SetActive(false);
        teamLogo.SetActive(false);
        logosBackground.SetActive(true);
        start.SetActive(false);
        startButton.SetActive(false);
        cont.SetActive(false);
        back.SetActive(false);
        continueButton.SetActive(false);
        optionsButton.SetActive(false);
        exitButton.SetActive(false);
        backButton.SetActive(false);
        applyButton.SetActive(false);
        showMenu = true;
        logoReference = Time.realtimeSinceStartup;
        state = states.SHOWING_LOGOS;
        menuReference = 0.0f;
        camOriginalPos = secundaryCamera.transform.position;
        camOriginalRotation = secundaryCamera.transform.rotation;
        loadScreen.Instancia.CargarEscena("city");
    }

    public void Update()
    {
        switch(state)
        {
            case states.SHOWING_LOGOS:
                if (Time.realtimeSinceStartup - logoReference < 2.25f)
                {
                    gameLogo.SetActive(true);
                    gameLogo2.SetActive(false);
                    teamLogo.SetActive(false);
                    logosBackground.SetActive(true);
                    start.SetActive(false);
                    startButton.SetActive(false);
                }
                else if (Time.realtimeSinceStartup - logoReference < 4.5f)
                {
                    gameLogo.SetActive(false);
                    gameLogo2.SetActive(false);
                    teamLogo.SetActive(true);
                    logosBackground.SetActive(true);
                    start.SetActive(false);
                    startButton.SetActive(false);

                }
                else
                {
                    teamLogo.SetActive(false);
                    logosBackground.SetActive(false);
                    gameLogo.SetActive(false);
                }
                break;
            case states.MAIN_MENU:
                if (secundaryCamera != null)
                {
                    secundaryCamera.SetActive(true);
                    secundaryCamera.transform.position = camOriginalPos;
                    secundaryCamera.transform.rotation = camOriginalRotation;
                }
                break;
            case states.PLAYING:
                break;
            case states.TRAVELLING_TO_PLAYER:
                secundaryCamera.transform.position = Vector3.Lerp(secundaryCamera.transform.position, GameObject.Find("Jugador").transform.position + new Vector3(0, 3.5f, 0.0f),Time.deltaTime);
                secundaryCamera.transform.LookAt(GameObject.Find("Jugador").transform.GetChild(GameObject.Find("Jugador").transform.childCount - 1).transform.position);
                break;
            default:
                break;
        }
    }

    public void enableOptions()
    {
        gameLogo.SetActive(false);
        gameLogo2.SetActive(false);
        teamLogo.SetActive(false);
        logosBackground.SetActive(false);
        start.SetActive(false);
        startButton.SetActive(false);
        cont.SetActive(false);
        back.SetActive(true);
        continueButton.SetActive(false);
        optionsButton.SetActive(false);
        exitButton.SetActive(false);
        backButton.SetActive(true);
        applyButton.SetActive(true);
    }

    public void enableMainMenu()
    {
        gameLogo.SetActive(false);
        gameLogo2.SetActive(true);
        teamLogo.SetActive(false);
        logosBackground.SetActive(false);
        start.SetActive(false);
        startButton.SetActive(false);
        cont.SetActive(true);
        back.SetActive(false);
        continueButton.SetActive(true);
        optionsButton.SetActive(true);
        exitButton.SetActive(true);
        backButton.SetActive(false);
        applyButton.SetActive(false);
    }

    public void disableMainMenu()
    {
        gameLogo.SetActive(false);
        gameLogo2.SetActive(false);
        teamLogo.SetActive(false);
        logosBackground.SetActive(false);
        start.SetActive(false);
        startButton.SetActive(false);
        cont.SetActive(false);
        back.SetActive(false);
        continueButton.SetActive(false);
        optionsButton.SetActive(false);
        exitButton.SetActive(false);
        backButton.SetActive(false);
        applyButton.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 1 && showMenu)
        {
            StartCoroutine(ExecuteAfterTime(2.25f));
            state = states.MAIN_MENU;
            showMenu = false;
        }
        else
            secundaryCamera.SetActive(false);

    }
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        gameLogo2.SetActive(true);       
        start.SetActive(true);
        startButton.SetActive(true);
    }

    public IEnumerator TravellingTime(float time)
    {
        yield return new WaitForSeconds(time);

        secundaryCamera.SetActive(false);
        state = states.PLAYING;
        GameObject.Find("Jugador").GetComponent<Controller>().state = Controller.playerState.IDLE;
    }

}
