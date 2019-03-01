using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class liquidState : MonoBehaviour
{
    public GameObject _highpug;
    public GameObject mixamorigHips;
    public GameObject liquid;
    public GameObject perfectlyHidratated;
    public GameObject wellHidratated;
    public GameObject badlyHidratated;
    public GameObject dead;
    public GameObject[] weapons = new GameObject[3];
    private Controller controller;
    public bool liquidStateOn = false;
    public bool cooldown = false;
    public bool firstFrameLiquid = true;
    public bool firstFrameNormal = false;
    public float startLiquid;
    public float finishLiquid;
    public float startCooldown;
    public float finishCooldown;
    public int hidratation;
    public int showHidratation;
    public int hidratationPrice;
    public bool inFountain;
    public static bool drinking = false;
    public bool stopLiquid = false;
    public static string fountain;
    public string aux;

    // Use this for initialization
    void Start()
    {
        hidratation = 100;
        _highpug.SetActive(true);
        mixamorigHips.SetActive(true);
        liquid.SetActive(false);
        perfectlyHidratated.SetActive(false);
        wellHidratated.SetActive(false);
        badlyHidratated.SetActive(false);
        dead.SetActive(false);
        controller = GetComponent<Controller>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!controller.hitting)
        {
            showHidratation = hidratation;
            if (cooldown)
            {
                if (firstFrameNormal)
                {
                    hideLiquid();
                    firstFrameNormal = false;
                }

                finishCooldown = Time.frameCount;

                if (finishCooldown - startCooldown > 300)
                {
                    cooldown = false;
                    firstFrameLiquid = true;
                }
            }
            else
            {
                liquidStateOn = controller.LiquidState;

                if (liquidStateOn)
                {
                    if (firstFrameLiquid)
                    {
                        showLiquid();
                        startLiquid = Time.frameCount;
                        firstFrameLiquid = false;
                    }

                    finishLiquid = Time.frameCount;

                    if ((finishLiquid - startLiquid > hidratation * 3) || (Input.GetKey(KeyCode.Q) && startLiquid + 30 < Time.frameCount))
                    {
                        liquidStateOn = false;
                        controller.LiquidState = liquidStateOn;
                        cooldown = true;
                        startCooldown = Time.frameCount;
                        firstFrameNormal = true;
                        setHidratation();
                    }
                }

                else
                {
                    if (inFountain && Input.GetKey(KeyCode.E)) drinking = true;

                    else if (drinking)
                    {
                        drinking = false;
                    }
                }

                if (hidratation > 66)
                {
                    hidratation = 100;
                    perfectlyHidratated.SetActive(true);
                    wellHidratated.SetActive(false);
                    badlyHidratated.SetActive(false);
                    dead.SetActive(false);
                }
                else if (hidratation > 33)
                {
                    hidratation = 50;
                    perfectlyHidratated.SetActive(false);
                    wellHidratated.SetActive(true);
                    badlyHidratated.SetActive(false);
                    dead.SetActive(false);
                }
                else if (hidratation > 0)
                {
                    hidratation = 10;
                    perfectlyHidratated.SetActive(false);
                    wellHidratated.SetActive(false);
                    badlyHidratated.SetActive(true);
                    dead.SetActive(false);
                }
                else
                {
                    hidratation = 0;
                    perfectlyHidratated.SetActive(false);
                    wellHidratated.SetActive(false);
                    badlyHidratated.SetActive(false);
                    dead.SetActive(true);
                    hidratation = 0;
                }
            }
        }

        aux = fountain;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (fountain != "NONE")
        {
            if (collision.gameObject.name == fountain)
            {
                inFountain = true;
            }
        } 
    }

    private void OnTriggerExit(Collider collision)
    {
        if (fountain != "NONE")
        {
            if (collision.gameObject.name == fountain)
            {
                inFountain = false;
            }
        }
    }

    public void setHidratation()
    {
        hidratationPrice = 40;
        if (hidratation < 10) hidratationPrice = 10;
        hidratation -= hidratationPrice;
    }
    public void showLiquid()
    {
        _highpug.SetActive(false);
        transform.GetComponent<cucumber>().enabled = false;
        for (int i = 0; i < mixamorigHips.transform.childCount; i++)
        {
            var child = mixamorigHips.transform.GetChild(i).gameObject;
            if (child != null && child.GetComponent<Renderer>() != null)
                child.GetComponent<Renderer>().enabled = false;
        }
        for (int i = 0; i < weapons.Length; i++) weapons[i].SetActive(false);
        liquid.SetActive(true);
    }

    public void hideLiquid()
    {
        _highpug.SetActive(true);
        for (int i = 0; i < mixamorigHips.transform.childCount; i++)
        {
            var child = mixamorigHips.transform.GetChild(i).gameObject;
            if (child != null && child.GetComponent<Renderer>() != null)
                child.GetComponent<Renderer>().enabled = true;
        }
        transform.GetComponent<cucumber>().enabled = true;
        for (int i = 0; i < weapons.Length; i++) weapons[i].SetActive(true);
        liquid.SetActive(false);
    }

    public void DrinkWater()
    {
        hidratation = 100;
    }
}