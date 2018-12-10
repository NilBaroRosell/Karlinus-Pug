using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    public float startTime;
    public float finalTime;
    public bool started;
    public GameObject Objective;
    private GameObject Objective_text;
    public GameObject M1_Texts;
    public static GameObject canvasHUD;

	// Use this for initialization
	void Awake () {
        Objective_text = Objective.transform.GetChild(0).gameObject;
        M1_Texts.SetActive(false);
        Objective.SetActive(false);
        startTime = Time.frameCount;
        started = false;
        canvasHUD = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
            finalTime = Time.frameCount;
        if (finalTime - startTime > 300)
        {
            Objective.SetActive(false);
        }
    }

    public void showM1Objective(int text_to_show)
    {
        M1_Texts.SetActive(true);
        Objective.SetActive(true);       
        Objective_text.GetComponent<Text>().text = M1_Texts.transform.GetChild(text_to_show).gameObject.GetComponent<Text>().text;
        M1_Texts.SetActive(false);
        startTime = Time.frameCount;
    }
}
