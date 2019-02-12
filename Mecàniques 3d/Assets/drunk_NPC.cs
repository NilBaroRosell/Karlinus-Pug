using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drunk_NPC : MonoBehaviour {

    private Animator anim;
    public enum drunkState { SITTING, STANDING, HOST_ANGRY, DRUNK, FIGHTING };
    public drunkState state;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        float randResult = Random.Range(2.5f, 7.5f);
        switch (state)
        {
            case drunkState.STANDING:
                anim.SetBool("STANDING", true);
                StartCoroutine(ExecuteAfterTime(randResult));
                break;
            case drunkState.SITTING:
                anim.SetBool("SITTING", true);
                StartCoroutine(ExecuteAfterTime(randResult));
                break;
            case drunkState.HOST_ANGRY:
                anim.SetBool("ANGRY_HOST", true);
                break;
            case drunkState.DRUNK:
                anim.SetBool("DRUNK", true);
                StartCoroutine(ExecuteAfterTime(randResult));
                break;
            case drunkState.FIGHTING:
                anim.SetBool("FIGHTING", true);
                StartCoroutine(ExecuteAfterTime(randResult));
                break;
            default:
                break;
        }
}

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        float randResult;
        switch(state)
        {
            case drunkState.STANDING:
                anim.SetTrigger("Drink");
                break;
            case drunkState.SITTING:
                randResult = Random.Range(1, 7);
                switch ((int)randResult)
                {
                    case 1:
                        anim.SetTrigger("Laugh");
                        break;
                    case 2:
                        anim.SetTrigger("Fist");
                        break;
                    case 3:
                        anim.SetTrigger("Angry");
                        break;
                    case 4:
                        anim.SetTrigger("Disagree");
                        break;
                    case 5:
                        anim.SetTrigger("Clap");
                        break;
                    case 6:
                        anim.SetTrigger("Tired");
                        break;
                }
                break;
            case drunkState.DRUNK:
                randResult = Random.Range(1, 3);
                if(randResult == 1) anim.SetTrigger("Drunk_1");
                else anim.SetTrigger("Drunk_2");
                break;
            case drunkState.FIGHTING:
                anim.SetTrigger("Punch");
                break;
        }
        randResult = Random.Range(7.5f, 12.5f);
        StartCoroutine(ExecuteAfterTime(randResult));
    }

    }
