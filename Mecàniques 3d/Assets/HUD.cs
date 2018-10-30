using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour {

    public float startTime;
    public float finalTime;
    public float startTime2;
    public float finalTime2;
    public bool started;
    public GameObject firstdMision;
    public GameObject secondMision;

	// Use this for initialization
	void Start () {
        firstdMision.SetActive(true);
        secondMision.SetActive(false);
        startTime = Time.frameCount;
        startTime2 = 0;
        started = false;
    }

    // Update is called once per frame
    void Update()
    {
        finalTime = Time.frameCount;
        if (finalTime - startTime > 300) firstdMision.SetActive(false);

        if (transform.localPosition.z > 39 && !started)
        {
            startTime2 = Time.frameCount;
            secondMision.SetActive(true);
            started = true;
        }

        if (startTime2 > 0)
        {
            finalTime2 = Time.frameCount;
            if (finalTime2 - startTime2 > 300) secondMision.SetActive(false);
        }
    }
}
