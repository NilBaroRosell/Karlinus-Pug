using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public GameObject gameLogo;
    public GameObject gameLogo2;
    public GameObject teamLogo;
    public GameObject logosBackground;
    public GameObject start;
    public GameObject startButton;
    public GameObject startBackground;
    public GameObject cont;
    public GameObject back;
    public GameObject continueButton;
    public GameObject optionsButton;
    public GameObject exitButton;
    public GameObject backButton;
    public GameObject applyButton;
    private bool finished;

    // Use this for initialization
    void Start () {
        gameLogo.SetActive(true);
        gameLogo2.SetActive(false);
        teamLogo.SetActive(false);
        logosBackground.SetActive(true);
        start.SetActive(false);
        startButton.SetActive(false);
        startBackground.SetActive(false);
        cont.SetActive(false);
        back.SetActive(false);
        continueButton.SetActive(false);
        optionsButton.SetActive(false);
        exitButton.SetActive(false);
        backButton.SetActive(false);
        applyButton.SetActive(false);
        finished = false;
    }

    public void Update()
    {
        if (!finished)
        {
            if (Time.frameCount < 150)
            {
                gameLogo.SetActive(true);
                gameLogo2.SetActive(false);
                teamLogo.SetActive(false);
                logosBackground.SetActive(true);
                start.SetActive(false);
                startButton.SetActive(false);
                startBackground.SetActive(false);
            }
            else if (Time.frameCount < 300)
            {
                gameLogo.SetActive(false);
                gameLogo2.SetActive(false);
                teamLogo.SetActive(true);
                logosBackground.SetActive(true);
                start.SetActive(false);
                startButton.SetActive(false);
                startBackground.SetActive(false);
            }
            else
            {
                gameLogo.SetActive(false);
                gameLogo2.SetActive(true);
                teamLogo.SetActive(false);
                logosBackground.SetActive(false);
                start.SetActive(true);
                startButton.SetActive(true);
                startBackground.SetActive(true);
                finished = true;
            }
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
        startBackground.SetActive(true);
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
        startBackground.SetActive(true);
        cont.SetActive(true);
        back.SetActive(false);
        continueButton.SetActive(true);
        optionsButton.SetActive(true);
        exitButton.SetActive(true);
        backButton.SetActive(false);
        applyButton.SetActive(false);
    }
}
