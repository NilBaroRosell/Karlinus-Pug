using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public GameObject gameLogo;
    public GameObject cont;
    public GameObject back;
    public GameObject continueButton;
    public GameObject optionsButton;
    public GameObject exitButton;
    public GameObject backButton;
    public GameObject applyButton;

    // Use this for initialization
    void Start () {
        gameLogo.SetActive(true);
        cont.SetActive(true);
        back.SetActive(false);
        continueButton.SetActive(true);
        optionsButton.SetActive(true);
        exitButton.SetActive(true);
        backButton.SetActive(false);
        applyButton.SetActive(false);
    }

    public void enableOptions()
    {
        gameLogo.SetActive(false);
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
        gameLogo.SetActive(true);
        cont.SetActive(true);
        back.SetActive(false);
        continueButton.SetActive(true);
        optionsButton.SetActive(true);
        exitButton.SetActive(true);
        backButton.SetActive(false);
        applyButton.SetActive(false);
    }
}
