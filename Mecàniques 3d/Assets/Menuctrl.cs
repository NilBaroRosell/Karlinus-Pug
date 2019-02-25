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
        loadScreen.Instancia.CargarEscena(sceneName);
    }

    public void ExitGameBtn()
    {
        Application.Quit();
    }
}
