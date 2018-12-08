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
    public GameObject[] weapons = new GameObject[4];
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
    public bool stopLiquid = false;

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
            liquidStateOn = movement.LiquidState;

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
                    movement.LiquidState = liquidStateOn;
                    cooldown = true;
                    startCooldown = Time.frameCount;
                    firstFrameNormal = true;
                    setHidratation();
                }
            }

            else
            {
                if (inFountain && Input.GetKey(KeyCode.E)) drinking = true;

                if (drinking)
                {
                    Debug.Log(liquidState.hidratation);
                    drinking = false;
                }
            }

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
                hidratation = 0;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "fountain") inFountain = true;
    }

    public void setHidratation()
    {
        hidratationPrice = hidratation / 4;
        if (hidratation < 10) hidratationPrice = 10;
        hidratation -= hidratationPrice;
    }
    public void showLiquid()
    {
        alphaJoints.SetActive(false);
        alphaSurface.SetActive(false);
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
        alphaJoints.SetActive(true);
        alphaSurface.SetActive(true);
        for (int i = 0; i < mixamorigHips.transform.childCount; i++)
        {
            var child = mixamorigHips.transform.GetChild(i).gameObject;
            if (child != null && child.GetComponent<Renderer>() != null)
                child.GetComponent<Renderer>().enabled = true;
        }
        transform.GetComponent<cucumber>().enabled = true;
        for (int i = 0; i < weapons.Length; i++) weapons[i].SetActive(true);
        if (transform.GetComponent<Animator>().GetBool("Is_Detected"))
        {
            weapons[0].SetActive(false);
        }
        else weapons[3].SetActive(false);
        liquid.SetActive(false);
    }
}