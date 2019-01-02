using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEnemy : MonoBehaviour {

    public AudioClip catSound;
    AudioSource source;
    private string lastActualState;

    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (csAreaVision.actualString == "F" && csAreaVision.actualString != lastActualState)
        {
            source.clip = catSound;
            source.Play();
        }
    }

    private void LateUpdate()
    {
        lastActualState = csAreaVision.actualString;
    }
}
