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
    AudioSource source7;
    AudioSource source8;
    private bool lastDrink;
    private bool lastState;
    private float lastSpeed;
    private string lastActualState;
    private int lastMisionIndex;
    public bool playingDrink, playingStateChange, playingLiquidState, playingRun, playingWalk, playingCrouch, playingDraw, playingAttack;
    private float startDrink, finishDrink, startChange, finishChange, startDraw, finishDraw;
    public GameObject head, leftEnd, leftBase, rightEnd, rightBase, rightFoot, leftFoot;
    private Animator anim;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        source1 = head.GetComponent<AudioSource>();
        source2 = leftEnd.GetComponent<AudioSource>();
        source3 = leftBase.GetComponent<AudioSource>();
        source4 = rightEnd.GetComponent<AudioSource>();
        source5 = rightBase.GetComponent<AudioSource>();
        source6 = rightFoot.GetComponent<AudioSource>();
        source7 = GetComponent<AudioSource>();
        source8 = leftFoot.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {

        if (misions.misionIndex == 4 || misions.misionIndex == 5)
        {
            if (source1.isPlaying)
            {
                source1.Stop();
                playingDrink = false;
            }
            if (source2.isPlaying)
            {
                source2.Stop();
                playingStateChange = false;
            }
            if (source3.isPlaying)
            {
                source3.Stop();
                playingLiquidState = false;
            }
            if (source4.isPlaying)
            {
                source4.Stop();
                playingRun = false;
            }
            if (source5.isPlaying)
            {
                source5.Stop();
                playingWalk = false;
            }
            if (source6.isPlaying)
            {
                source6.Stop();
                playingCrouch = false;
            }
            if (source7.isPlaying)
            {
                source7.Stop();
                playingDraw = false;
            }
            if (source8.isPlaying)
            {
                source8.Stop();
                playingAttack = false;
            }
        }
        else
        {
            if (kill_cono_vision.actualString == "W")
            {
                if (liquidState.drinking && !playingDrink)
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
                if (movement.LiquidState && movement.LiquidState == lastState && movement.speed == 9 && !playingLiquidState && !playingStateChange)
                {
                    source3.clip = liqState;
                    source3.Play();
                    playingLiquidState = true;
                }
                if (movement.speed == 13 && !playingRun && !playingDraw && misions.misionIndex != 6)
                {
                    source4.clip = run;
                    source4.Play();
                    playingRun = true;
                }
                if (movement.speed == 9 && !playingWalk && !playingDraw && misions.misionIndex != 6)
                {
                    source5.clip = walk;
                    source5.Play();
                    playingWalk = true;
                }
                if (movement.speed == 6 && !playingCrouch && !playingDraw && misions.misionIndex != 6)
                {
                    source6.clip = walk;
                    source6.Play();
                    playingCrouch = true;
                }
                if (Input.GetKeyDown(KeyCode.Mouse1) && !playingDraw && anim.GetBool("Is_Draw"))
                {
                    source7.clip = draw;
                    source7.Play();
                    playingDraw = true;
                    startDraw = Time.frameCount;
                }
            }

            else if (kill_cono_vision.actualString == "A" || kill_cono_vision.actualString == "R")
            {
                if (source1.isPlaying)
                {
                    source1.Stop();
                    playingDrink = false;
                }
                if (source2.isPlaying)
                {
                    source2.Stop();
                    playingStateChange = false;
                }
                if (source3.isPlaying)
                {
                    source3.Stop();
                    playingLiquidState = false;
                }
                if (source4.isPlaying)
                {
                    source4.Stop();
                    playingRun = false;
                }
                if (source5.isPlaying)
                {
                    source5.Stop();
                    playingWalk = false;
                }
                if (source6.isPlaying)
                {
                    source6.Stop();
                    playingCrouch = false;
                }
                if (source7.isPlaying)
                {
                    source7.Stop();
                    playingDraw = false;
                }
                if (!playingAttack)
                {
                    source8.clip = stateChange;
                    source8.Play();
                    playingAttack = true;
                }

            }

            /*else if (kill_cono_vision.actualString == "K")
            {

            }*/
        }

        if (playingDrink)
        {
            if (source2.isPlaying)
            {
                source2.Stop();
                playingStateChange = false;
            }
            if (source3.isPlaying)
            {
                source3.Stop();
                playingLiquidState = false;
            }
            if (source4.isPlaying)
            {
                source4.Stop();
                playingRun = false;
            }
            if (source5.isPlaying)
            {
                source5.Stop();
                playingWalk = false;
            }
            if (source6.isPlaying)
            {
                source6.Stop();
                playingCrouch = false;
            }
            finishDrink = Time.frameCount;
            if (finishDrink - startDrink > 150)
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

        if (playingRun && (movement.speed != 13))
        {
            source4.Stop();
            playingRun = false;
        }

        if (playingWalk && (movement.speed != 9 || movement.LiquidState))
        {
            source5.Stop();
            playingWalk = false;
        }

        if (playingCrouch && (movement.speed != 6 || movement.LiquidState))
        {
            source6.Stop();
            playingCrouch = false;
        }

        if (playingDraw)
        {
            if (source1.isPlaying)
            {
                source1.Stop();
                playingDrink = false;
            }
            if (source2.isPlaying)
            {
                source2.Stop();
                playingStateChange = false;
            }
            if (source3.isPlaying)
            {
                source3.Stop();
                playingLiquidState = false;
            }
            if (source4.isPlaying)
            {
                source4.Stop();
                playingRun = false;
            }
            if (source5.isPlaying)
            {
                source5.Stop();
                playingWalk = false;
            }
            if (source6.isPlaying)
            {
                source6.Stop();
                playingCrouch = false;
            }
            finishDraw = Time.frameCount;
            if (finishDraw - startDraw > 80) playingDraw = false;
        }

        if (playingAttack && !source8.isPlaying) playingAttack = false;
    }

    private void LateUpdate()
    {
        lastDrink = liquidState.drinking;
        lastState = movement.LiquidState;
        lastSpeed = movement.speed;
        lastActualState = kill_cono_vision.actualString;
    }
}
