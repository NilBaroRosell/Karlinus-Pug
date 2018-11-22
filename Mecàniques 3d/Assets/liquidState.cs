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
    public bool liquidStateOn = false;
    public bool cooldown = false;
    public bool firstFrameLiquid = true;
    public bool firstFrameNormal = false;
    public float startLiquid;
    public float finishLiquid;
    public float startCooldown;
    public float finishCooldown;

    // Use this for initialization
    void Start()
    {
        alphaJoints.SetActive(true);
        alphaSurface.SetActive(true);
        mixamorigHips.SetActive(true);
        liquid.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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

                //moure el jugador

                if (finishLiquid - startLiquid > 300)
                {
                    liquidStateOn = false;
                    movement.liquidState = liquidStateOn;
                    cooldown = true;
                    startCooldown = Time.frameCount;
                    firstFrameNormal = true;
                }
            }
        }
    }
}