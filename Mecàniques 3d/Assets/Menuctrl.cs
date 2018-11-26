using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menuctrl : MonoBehaviour {

    void Start()
    {

    }

    public void LoadScene(string sceneName)
    {
        PlayerPrefs.SetFloat("KarlinusPosX", -84.99f);
        PlayerPrefs.SetFloat("KarlinusPosY", 1.27f);
        PlayerPrefs.SetFloat("KarlinusPosZ", -41.88f);
        SceneManager.LoadScene(sceneName);
        
    }

    public void ExitGameBtn()
    {
        Application.Quit();
    }
}
