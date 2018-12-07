using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fountain : MonoBehaviour{

    public bool inFountain = false;
    public bool drinking = false;
    public bool cooldownFountain = false;
    public float startCooldownFountain;
    public float finishCooldownFountain;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (drinking)
        {
            liquidState.hidratation = 100;
            drinking = false;
        }

        if (cooldownFountain)
        {
            finishCooldownFountain = Time.frameCount;
            if (finishCooldownFountain - startCooldownFountain > 980) cooldownFountain = false;
        }
        else
        {
            if (inFountain && Input.GetKey(KeyCode.E))
            {
                drinking = true;
                startCooldownFountain = Time.frameCount;
                cooldownFountain = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") inFountain = true;
    }
}