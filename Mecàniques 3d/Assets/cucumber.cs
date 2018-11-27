using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class cucumber : MonoBehaviour {

    public GameObject cucumber_hide;
    public GameObject cucumber_show;
    public GameObject cucumber_thrown;
    public Rigidbody cucumberRig;
    public enum cucumberState { HIDE, SHOW, FLYING, FLOOR, COOLDOWN };
    public cucumberState state;
    public static bool onFloor = false;
    public bool touchingCucumber = false;
    public float startCooldown;
    public float finishCooldown;
    private bool firstTime = false;
    public Vector3 force;
    private bool flying = false;

    // Use this for initialization
    void Start () {
        state = cucumberState.HIDE;
        cucumber_hide.SetActive(true);
        cucumber_show.SetActive(false);
        cucumber_thrown.SetActive(false);
    }

    private void Update()
    {
        if (flying) Debug.Log(cucumber.onFloor);
    }

    // Update is called once per frame
    void FixedUpdate () {
        switch (state)
        {
            case cucumberState.HIDE:
            {
                cucumber_hide.SetActive(true);
                cucumber_show.SetActive(false);
                cucumber_thrown.SetActive(false);
                if (Input.GetKeyDown(KeyCode.Mouse2)) state = cucumberState.SHOW;
                break;
            }
            case cucumberState.SHOW:
            {
                cucumber_hide.SetActive(false);
                cucumber_show.SetActive(true);
                cucumber_thrown.SetActive(false);
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    state = cucumberState.FLYING;
                    cucumber_thrown.transform.position = cucumber_show.transform.position;
                    firstTime = true;
                }
                break;
            }
            case cucumberState.FLYING:
            {
                cucumber_hide.SetActive(false);
                cucumber_show.SetActive(false);
                cucumber_thrown.SetActive(true);
                flying = false;
                if (firstTime)
                {
                    cucumberRig.AddForce(transform.forward.x * force.x, force.y, transform.forward.z * force.z);
                    firstTime = false;
                }
                if (onFloor)
                {
                    state = cucumberState.FLOOR;
                }
                break;
            }
            case cucumberState.FLOOR:
            {
                cucumber_hide.SetActive(false);
                cucumber_show.SetActive(false);
                cucumber_thrown.SetActive(true);
                flying = false;
                if (touchingCucumber)
                {
                    onFloor = false;
                    touchingCucumber = false;
                    state = cucumberState.COOLDOWN;
                    startCooldown = Time.frameCount;
                }
                break;
            }
            case cucumberState.COOLDOWN:
            {
                cucumber_hide.SetActive(true);
                cucumber_show.SetActive(false);
                cucumber_thrown.SetActive(false);
                finishCooldown = Time.frameCount;
                if(finishCooldown - startCooldown > 300) state = cucumberState.HIDE;
                break;
            }
            default:
            {
                break;
            }
        }
	}

    private void CheckPlayer()
    {
        if () touchingCucumber = true;
    }
}
