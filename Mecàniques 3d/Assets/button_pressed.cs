using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class button_pressed : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public void sceneChange() {
        switch (LoadScene.respawnToLoad)
        {
            case Respawns.InitialRespawns.SEWER_1:
            case Respawns.InitialRespawns.SEWER_2:
            case Respawns.InitialRespawns.SEWER_3:
                loadScreen.Instancia.CargarEscena("sewer");
                break;
            case Respawns.InitialRespawns.CITY_1:
            case Respawns.InitialRespawns.CITY_2:
            case Respawns.InitialRespawns.PUB_OUTSIDE:
            case Respawns.InitialRespawns.CAPTAIN_OUTSIDE:
                loadScreen.Instancia.CargarEscena("city");
                break;
            case Respawns.InitialRespawns.CAPTAIN_INSIDE:
                loadScreen.Instancia.CargarEscena("CaptainHouse");
                break;
            case Respawns.InitialRespawns.CHAMBER:
                loadScreen.Instancia.CargarEscena("StoneChamber");
                break;
            default:
                break;
        }
    }
}
