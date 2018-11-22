using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class liquidState : MonoBehaviour
{

    public GameObject alphaJoints;
    public GameObject alphaSurface;
    public GameObject mixamorigHips;
    public GameObject liquid;
    public GameObject perfectlyHidratated;
    public GameObject wellHidratated;
    public GameObject badlyHidratated;
    public GameObject dead;
    public bool liquidStateOn = false;
    public bool cooldown = false;
    public bool firstFrameLiquid = true;
    public bool firstFrameNormal = false;
    public float startLiquid;
    public float finishLiquid;
    public float startCooldown;
    public float finishCooldown;
    public static int hidratation;
    public int showHidratation;
    public int hidratationPrice;
    public bool inFountain = false;
    public bool drinking = false;

    // Use this for initialization
    void Start()
    {
        hidratation = 100;
        alphaJoints.SetActive(true);
        alphaSurface.SetActive(true);
        mixamorigHips.SetActive(true);
        liquid.SetActive(false);
        perfectlyHidratated.SetActive(false);
        wellHidratated.SetActive(false);
        badlyHidratated.SetActive(false);
        dead.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        showHidratation = hidratation;

        if (cooldown)
        {
            if (firstFrameNormal)
            {
                alphaJoints.SetActive(true);
                alphaSurface.SetActive(true);
                mixamorigHips.SetActive(true);
                liquid.SetActive(false);
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
            liquidStateOn = movement.liquidState;

            if (liquidStateOn)
            {
                if (firstFrameLiquid)
                {
                    alphaJoints.SetActive(false);
                    alphaSurface.SetActive(false);
                    mixamorigHips.SetActive(false);
                    liquid.SetActive(true);
                    startLiquid = Time.frameCount;
                    firstFrameLiquid = false;
                }

                finishLiquid = Time.frameCount;

                if (finishLiquid - startLiquid > hidratation * 3)
                {
                    liquidStateOn = false;
                    movement.liquidState = liquidStateOn;
                    cooldown = true;
                    startCooldown = Time.frameCount;
                    firstFrameNormal = true;
                    hidratationPrice = hidratation / 4;
                    if (hidratation < 10) hidratationPrice = 10;
                    hidratation -= hidratationPrice;
                }
            }

            else
            {
                if (hidratation > 100) hidratation = 100;
                else if (hidratation > 66)
                {
                    perfectlyHidratated.SetActive(true);
                    wellHidratated.SetActive(false);
                    badlyHidratated.SetActive(false);
                    dead.SetActive(false);
                }
                else if (hidratation > 33)
                {
                    perfectlyHidratated.SetActive(false);
                    wellHidratated.SetActive(true);
                    badlyHidratated.SetActive(false);
                    dead.SetActive(false);
                }
                else if (hidratation > 0)
                {
                    perfectlyHidratated.SetActive(false);
                    wellHidratated.SetActive(false);
                    badlyHidratated.SetActive(true);
                    dead.SetActive(false);
                }
                else
                {
                    perfectlyHidratated.SetActive(false);
                    wellHidratated.SetActive(false);
                    badlyHidratated.SetActive(false);
                    dead.SetActive(true);
                    //mostrar que ha mort
                    SceneManager.LoadScene("DEAD");
                }

                if (inFountain && Input.GetKey(KeyCode.E)) drinking = true;
                if (drinking)
                {
                    Debug.Log(liquidState.hidratation);
                    drinking = false;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "fountain") inFountain = true;
    }
}