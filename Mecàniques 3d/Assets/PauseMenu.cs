using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public GameObject canvas1;
    public GameObject canvas2;
    public bool options;

    // Use this for initialization
    void Start () {
        canvas1.SetActive(false);
        canvas2.SetActive(false);
        options = false;
    }

    public void Update()
    {
        if (!options)
        { 
            if (misions.pauseMenu)
            {
                canvas1.SetActive(true);
                canvas2.SetActive(false);
                Time.timeScale = 0;
            }
            else if(MainMenu.Instance == null)
            {
                canvas1.SetActive(false);
                canvas2.SetActive(false);
                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    public void continueGame()
    {
        misions.pauseMenu = false;
        canvas1.SetActive(false);
        canvas2.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void enableOptions()
    {
        canvas1.SetActive(false);
        canvas2.SetActive(true);
        options = true;
    }

    public void enableMainMenu()
    {
        canvas1.SetActive(true);
        canvas2.SetActive(false);
        options = false;
    }
}
