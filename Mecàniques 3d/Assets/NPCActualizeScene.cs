using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCActualizeScene : MonoBehaviour
{
    private misions misionsScript;
    public misions.Misions misionGO;
    public string sceneToLoad;

    // Use this for initialization
    void Start()
    {
        if (GameObject.Find("Misiones") != null) misionsScript = GameObject.Find("Misiones").GetComponent<misions>();
    }


    private void OnTriggerStay(Collider collision)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            misionsScript.ActualMision = misionGO;
            loadScreen.Instancia.CargarEscena(sceneToLoad);
        }
    }
}
