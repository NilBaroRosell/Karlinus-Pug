using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

    public Respawns.InitialRespawns SceneToLoad;
    public static Respawns.InitialRespawns respawnToLoad;


    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && Input.GetKey(KeyCode.E))
        {
           switch (SceneToLoad)
            {
                case Respawns.InitialRespawns.SEWER_1:
                case Respawns.InitialRespawns.SEWER_2:
                case Respawns.InitialRespawns.SEWER_3:
                    respawnToLoad = SceneToLoad;
                    loadScreen.Instancia.CargarEscena("sewer");
                    break;
                case Respawns.InitialRespawns.CITY_1:
                case Respawns.InitialRespawns.CITY_2:
                case Respawns.InitialRespawns.PUB_OUTSIDE:
                    respawnToLoad = SceneToLoad;
                    loadScreen.Instancia.CargarEscena("city");
                    break;
                case Respawns.InitialRespawns.PUB_INSIDE:
                    respawnToLoad = SceneToLoad;
                    loadScreen.Instancia.CargarEscena("PUB");
                    break;
                default:
                    break;
            }
        }
    }
}
