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
        Debug.Log(gameObject.transform.parent);
        visible = true;
        //GameObject.Find("Jugador").GetComponent<HUD>().IntroduceEnemy(gameObject);
    }

    private void OnBecameInvisible()
    {
        visible = false;
    }
}
