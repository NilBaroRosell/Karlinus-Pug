using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCucumber : MonoBehaviour {

    public AudioClip cucumberSound;
    AudioSource source;
    private bool lastOnfloor;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        lastOnfloor = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (cucumber.onFloor && cucumber.onFloor != lastOnfloor)
        {
            source.clip = cucumberSound;
            source.Play();
        }
	}

    private void LateUpdate()
    {
        lastOnfloor = cucumber.onFloor;
    }
}
