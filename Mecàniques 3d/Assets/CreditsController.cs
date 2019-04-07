using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(ExecuteAfterTime(25));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        Debug.Log("Hello it's me");
    }
}
