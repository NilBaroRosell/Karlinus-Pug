using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

    public enum Scenes {SEWER, CITY, PUB, HOUSE, PALACE};
    public Scenes SceneToLoad;
    public bool changeScene;

    // Use this for initialization
    void Start () {
        changeScene = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(changeScene && Input.GetKey(KeyCode.E))
        {
            Debug.Log("HOLA");
            switch (SceneToLoad)
            {
                case Scenes.SEWER:
                    SceneManager.LoadScene("sewer");
                    break;
                case Scenes.CITY:
                    
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
