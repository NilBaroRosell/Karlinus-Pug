using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.GetChild(1).gameObject.SetActive(false);
        if (misions.Instance.ActualMision == misions.Misions.M1) transform.GetChild(0).gameObject.SetActive(false);
        else
        {
            MainMenu.Instance.intro.SetActive(false);
            MainMenu.Instance.intro.GetComponent<Animator>().speed = 0;
            StartCoroutine(ExecuteAfterTime(25));
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        if (!transform.GetChild(1).gameObject.activeSelf)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            StartCoroutine(ExecuteAfterTime(25));
        }
        else
        {
            MainMenu.Instance.state = MainMenu.states.PLAYING;
            misions.pauseMenu = false;
            MainMenu.Instance.showMenu = true;
            misions.Instance.ActualMision = misions.Misions.NONE;
            loadScreen.Instancia.CargarEscena("city");
        }
    }

    IEnumerator introAnim(float time)
    {
        yield return new WaitForSeconds(time);
        MainMenu.Instance.state = MainMenu.states.PLAYING;
        MainMenu.Instance.intro.SetActive(false);
        MainMenu.Instance.intro.GetComponent<Animator>().speed = 0;
        loadScreen.Instancia.CargarEscena("sewer");
    }

    IEnumerator waitAnim(float time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(introAnim(25.0f));
        MainMenu.Instance.intro.SetActive(true);
        MainMenu.Instance.intro.GetComponent<Animator>().speed = 1;
    }

    private void OnLevelWasLoaded(int level)
    {
        if (misions.Instance.ActualMision == misions.Misions.M1) StartCoroutine(waitAnim(1.5f));
    }
}
