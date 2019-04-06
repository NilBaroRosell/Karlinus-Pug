using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCActualizeScene : MonoBehaviour {

    private misions misionsScript;
    public misions.Misions misionGO;
    public string sceneToLoad;
    Sprite interact;

    // Use this for initialization
    void Start () {
        if (GameObject.Find("Misiones") != null) misionsScript = GameObject.Find("Misiones").GetComponent<misions>();
        interact = Resources.Load<Sprite>("Sprites/StartMission");
    }


    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        {
            misions.Instance.ActualMision = misionGO;
            loadScreen.Instancia.CargarEscena(sceneToLoad);
        }
        else if (collision.gameObject.tag == "Player") collision.gameObject.GetComponentInParent<HUD>().showInteractSprite(this.transform.position, interact);
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player") collision.gameObject.GetComponentInParent<HUD>().hideInteractSprite();
    }
}
