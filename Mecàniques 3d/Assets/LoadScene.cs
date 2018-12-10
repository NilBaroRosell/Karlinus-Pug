﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

    public enum Scenes {SEWER_1, SEWER_2, CITY_1, CITY_2, PUB, HOUSE, PALACE};
    public Scenes SceneToLoad;
    private bool changeScene;

    // Use this for initialization
    void Start () {
        changeScene = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(changeScene && Input.GetKey(KeyCode.E))
        {
            switch (SceneToLoad)
            {
                case Scenes.SEWER_1:
                    SceneManager.LoadScene("sewer");                    
                    break;
                case Scenes.CITY_1:
                    SceneManager.LoadScene("city");
                    break;
                default:                    
                    SceneManager.LoadScene("city");
                    break;
            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            
            changeScene = true;
        }
        else
        {
            changeScene = false;
        }
    }
}
