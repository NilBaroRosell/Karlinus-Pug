using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour {

    public AudioClip drink;
    public AudioClip stateChange;
    public AudioClip liqState;
    public AudioClip run;
    public AudioClip walk;
    public AudioClip draw;
    AudioSource source1;
    AudioSource source2;
    AudioSource source3;
    AudioSource source4;
    AudioSource source5;
    AudioSource source6;
    private bool lastDrink;
    public bool lastState;
    public float lastSpeed;
    public bool playingDrink, playingStateChange, playingLiquidState, playingRun, playingWalk, playingDraw;
    private float startDrink, finishDrink, startChange, finishChange, startDraw, finishDraw;
    public GameObject head, leftEnd, leftBase, rightEnd, rightBase;
    private Animator anim;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        source1 = head.GetComponent<AudioSource>();
        source2 = leftEnd.GetComponent<AudioSource>();
        source3 = leftBase.GetComponent<AudioSource>();
        source4 = rightEnd.GetComponent<AudioSource>();
        source5 = rightBase.GetComponent<AudioSource>();
        source6 = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (liquidState.drinking && liquidState.drinking != lastDrink && !playingDrink)
        {
            source1.clip = drink;
            source1.Play();
            playingDrink = true;
            startDrink = Time.frameCount;
        }
        if (movement.LiquidState && movement.LiquidState != lastState)
        {
            source2.clip = stateChange;
            source2.Play();
            playingStateChange = true;
            startChange = Time.frameCount;
        }
        if (movement.LiquidState && movement.LiquidState == lastState && movement.speed == 9 &&!playingLiquidState && !playingStateChange)
        {
            source3.clip = liqState;
            source3.Play();
            playingLiquidState = true;
        }
        if (movement.speed == 13 && !playingRun)
        {
            source4.clip = run;
            source4.Play();
            playingRun = true;
        }
        if (movement.speed > 0 && movement.speed < 10 && !playingWalk)
        {
            source5.clip = walk;
            source5.Play();
            playingWalk = true;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && !playingDraw && anim.GetBool("Is_Draw"))
        {
            source6.clip = draw;
            source6.Play();
            playingDraw = true;
            startDraw = Time.frameCount;
        }
        
        if (playingDrink)
        {
            finishDrink = Time.frameCount;
            if (finishDrink - startDrink > 300)
            {
                playingDrink = false;
            }
        }

        if (playingStateChange)
        {
            finishChange = Time.frameCount;
            if (finishChange - startChange > 50)
            {
                playingStateChange = false;
            }
        }

        if (playingLiquidState && !movement.LiquidState)
        {
            source3.Stop();
            playingLiquidState = false;
        }

        if (playingRun && movement.speed != 13)
        {
            source4.Stop();
            playingRun = false;
        }

        if (playingWalk && (movement.speed == 0 || movement.speed > 10 || movement.LiquidState))
        {
            source5.Stop();
            playingWalk = false;
        }

        if (playingDraw)
        {
            finishDraw = Time.frameCount;
            if (finishDraw -startDraw > 50) playingDraw = false;
        }
    }

    private void LateUpdate()
    {
        lastDrink = liquidState.drinking;
        lastState = movement.LiquidState;
        lastSpeed = movement.speed;
    }
}
