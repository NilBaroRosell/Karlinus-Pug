using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class button_pressed : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    public void sceneChange()
    {
        switch (LoadScene.respawnToLoad)
        {
            case LoadScene.Scenes.SEWER_1:
            case LoadScene.Scenes.SEWER_2:
            case LoadScene.Scenes.SEWER_3:
                loadScreen.Instancia.CargarEscena("sewer");
                break;
            case LoadScene.Scenes.CITY_1:
            case LoadScene.Scenes.CITY_2:
                loadScreen.Instancia.CargarEscena("city");
                break;
            default:
                break;
        }
    }
}