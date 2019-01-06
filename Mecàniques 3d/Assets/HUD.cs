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
    //private const float objectiveY = 120.0f;
    //private const float HelpsY = 200.0f;
    private float objectiveY = Screen.height / 5;
    private float HelpsY = Screen.height/4;

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
            if (Objective.activeSelf && Objective.GetComponent<RectTransform>().position.y > -381.4f / 10) Objective.GetComponent<RectTransform>().position =
                    new Vector3(Objective.GetComponent<RectTransform>().position.x, Objective.GetComponent<RectTransform>().position.y - 40, Objective.GetComponent<RectTransform>().position.z);
            else Objective.SetActive(false);
            if (Helps.activeSelf && Helps.GetComponent<RectTransform>().position.y > -426.0f / 10) Helps.GetComponent<RectTransform>().position =
                    new Vector3(Helps.GetComponent<RectTransform>().position.x, Helps.GetComponent<RectTransform>().position.y - 40, Helps.GetComponent<RectTransform>().position.z);
            else Helps.SetActive(false);
        }
        else
        {
            objectiveY = Screen.height / 4;
            HelpsY = Screen.height / 2.25f;
            if (Objective.activeSelf && Objective.GetComponent<RectTransform>().position.y < objectiveY) Objective.GetComponent<RectTransform>().position = 
                    new Vector3(Objective.GetComponent<RectTransform>().position.x, Objective.GetComponent<RectTransform>().position.y + 40, Objective.GetComponent<RectTransform>().position.z);
            if (Helps.activeSelf && Helps.GetComponent<RectTransform>().position.y < HelpsY) Helps.GetComponent<RectTransform>().position =
                    new Vector3(Helps.GetComponent<RectTransform>().position.x, Helps.GetComponent<RectTransform>().position.y + 40, Helps.GetComponent<RectTransform>().position.z);
        }
    }

    public void showM1Objective(int text_to_show)
    {
        M1.SetActive(true);
        Objective.SetActive(true);       
        Objective_text.GetComponent<Text>().text = M1.transform.GetChild(0).GetChild(text_to_show).gameObject.GetComponent<Text>().text;
        M1.SetActive(false);
        Objective.GetComponent<RectTransform>().position = 
            new Vector3(Objective.GetComponent<RectTransform>().position.x, -381.4f/10, Objective.GetComponent<RectTransform>().position.z);
        startTime = Time.frameCount;
    }
    public void showM1Helps(int text_to_show, int font_to_set)
    {
        M1.SetActive(true);
        Helps.SetActive(true);
        Helps_text.GetComponent<Text>().text = M1.transform.GetChild(1).GetChild(text_to_show).gameObject.GetComponent<Text>().text;
        Helps_text.GetComponent<Text>().fontSize = font_to_set;
        M1.SetActive(false);
        Helps.GetComponent<RectTransform>().position =
                    new Vector3(Helps.GetComponent<RectTransform>().position.x, -426.0f/10, Helps.GetComponent<RectTransform>().position.z);
        startTime = Time.frameCount;
    }
}
