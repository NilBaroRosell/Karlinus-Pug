using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour {

    public AudioClip drink;
    public AudioClip stateChange;
    public AudioClip liqState;
    public AudioClip run;
    public AudioClip walk;
    public AudioClip withdraw;    
    public GameObject head, leftEnd, leftBase, rightEnd, rightBase, rightFoot, leftFoot;

    //Animation Sounds

    public void SoftStep(string foot)
    {
        if (foot == "left") leftFoot.GetComponent<AudioSource>().PlayOneShot(walk);
        else if (foot == "right") rightFoot.GetComponent<AudioSource>().PlayOneShot(walk);
    }

    public void HardStep(string foot)
    {
        if (foot == "left") leftFoot.GetComponent<AudioSource>().PlayOneShot(run);
        else if (foot == "right") rightFoot.GetComponent<AudioSource>().PlayOneShot(run);
    }

    public void WithdrawSword()
    {
        GetComponent<AudioSource>().PlayOneShot(withdraw);
    }
}
