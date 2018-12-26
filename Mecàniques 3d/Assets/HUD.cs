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
    public GameObject Helps;
    public GameObject M1;
    private GameObject Helps_text;
    public static GameObject canvasHUD;

	// Use this for initialization
	void Awake () {
        Objective_text = Objective.transform.GetChild(0).gameObject;
        Helps_text = Helps.transform.GetChild(0).gameObject;
        M1.SetActive(false);
        Objective.SetActive(false);
        Helps.SetActive(false);
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
            Helps.SetActive(false);
        }
    }

    public void showM1Objective(int text_to_show)
    {
        M1.SetActive(true);
        Objective.SetActive(true);       
        Objective_text.GetComponent<Text>().text = M1.transform.GetChild(0).GetChild(text_to_show).gameObject.GetComponent<Text>().text;
        M1.SetActive(false);
        startTime = Time.frameCount;
    }
    public void showM1Helps(int text_to_show, int font_to_set)
    {
        M1.SetActive(true);
        Helps.SetActive(true);
        Helps_text.GetComponent<Text>().text = M1.transform.GetChild(1).GetChild(text_to_show).gameObject.GetComponent<Text>().text;
        Helps_text.GetComponent<Text>().fontSize = font_to_set;
        M1.SetActive(false);
        startTime = Time.frameCount;
    }
}
