using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class visibleEnemy : MonoBehaviour {

    public bool visible;

    private void Start()
    {
        visible = false;
    }

    void OnBecameVisible()
    {
        visible = true;
        GameObject.Find("Jugador").GetComponent<HUD>().IntroduceEnemy(gameObject.transform.parent.gameObject);
    }

    private void OnBecameInvisible()
    {
        visible = false;
    }
}
