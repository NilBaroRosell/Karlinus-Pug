using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(ExecuteAfterTime(25));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        MainMenu.Instance.state = MainMenu.states.PLAYING;
            misions.pauseMenu = false;
            MainMenu.Instance.showMenu = true;
            misions.Instance.ActualMision = misions.Misions.NONE;
        loadScreen.Instancia.CargarEscena("city");
    }
}
