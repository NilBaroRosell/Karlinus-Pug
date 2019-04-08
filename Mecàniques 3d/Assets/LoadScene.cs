using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

    public Respawns.InitialRespawns SceneToLoad;
    public static Respawns.InitialRespawns respawnToLoad;
    Sprite interact;

    private void Start()
    {
        interact = Resources.Load<Sprite>("Sprites/Default");
    }


    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && Input.GetKey(KeyCode.E) && !misions.fight)
        {
            collision.gameObject.GetComponentInParent<HUD>().hideInteractSprite();
            if (misions.Instance.ActualMision == misions.Misions.M3)
            {
                misions.nextEvent = true;
                misions.Instance.PrincipalMision.MisionsCompleted[2] = true;
                misions.Instance.doorsUnlucked = true;
                misions.Instance.gameObject.GetComponent<Respawns>().initialRespawn = SceneToLoad;
            }
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
        else if (collision.gameObject.tag == "Player") collision.gameObject.GetComponentInParent<HUD>().showInteractSprite(this.transform.position, interact);
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player") collision.gameObject.GetComponentInParent<HUD>().hideInteractSprite();
    }
}
