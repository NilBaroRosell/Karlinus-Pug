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
        //GameObject.Find("Misiones").GetComponent<misions>().ActualMision = misions.Misions.NONE;
        if(!misions.Instance.resetGameFile)loadScreen.Instancia.CargarEscena(sceneName);
        else loadScreen.Instancia.CargarEscena("sewer");
    }

    public void ExitGameBtn()
    {
        Application.Quit();
    }
}
