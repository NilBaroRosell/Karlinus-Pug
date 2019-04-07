using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    private AudioClip intro;
    private AudioClip ambiental;
    private AudioClip sneak;
    private AudioClip fight;

    private AudioSource mainAudioSource;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        intro = Resources.Load<AudioClip>("music/intro");
        ambiental = Resources.Load<AudioClip>("music/ambiental");
        sneak = Resources.Load<AudioClip>("music/sneak");
        fight = Resources.Load<AudioClip>("music/fight");

        mainAudioSource = this.GetComponent<AudioSource>();

        Invoke("AudioFinished", mainAudioSource.clip.length);

    }

    // Use this for initialization
    void Start () { 
    }

    private void FixedUpdate()
    {
        if (!mainAudioSource.isPlaying) OnLevelWasLoaded(SceneManager.GetActiveScene().buildIndex);
    }

    void AudioFinished()
        {
        mainAudioSource.clip = ambiental;
    }

    private void OnLevelWasLoaded(int level)
    {       
        switch(level)
        {
            case 1:
            case 3:
            case 7:
                setAmbiental();
                break;
            case 6:
                break;
            default:
                setSneak();
                break;

        }
    }

    private void setAmbiental()
    {
        if(mainAudioSource.clip.name != "intro" && mainAudioSource.clip.name != "ambiental")
        {
            mainAudioSource.clip = ambiental;
            mainAudioSource.Play();
        }
    }
    private void setSneak()
    {
        Debug.Log("hola");
        if (mainAudioSource.clip.name != "sneak")
        {
            mainAudioSource.clip = sneak;
            mainAudioSource.Play();
        }
    }

    public void checkFight()
    {
        if (misions.fight && mainAudioSource.clip.name != "fight")
        {
            mainAudioSource.clip = fight;
            mainAudioSource.Play();
        }
        else if (mainAudioSource.clip.name == "fight" && !misions.fight) OnLevelWasLoaded(SceneManager.GetActiveScene().buildIndex);
    }

}
