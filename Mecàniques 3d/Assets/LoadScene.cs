using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

    public enum Scenes {SEWER_1, SEWER_2, CITY_1, CITY_2, PUB, HOUSE, PALACE};
    public Scenes SceneToLoad;
    private bool changeScene;
    Vector3 city_1Respawn = new Vector3(-84.99f, -27.56327f, -41.88f);
    Vector3 sewer_1Respawn = new Vector3(-64.511f, -9.288946f, 92.9324f);

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
                    PlayerPrefs.SetFloat("KarlinusPosX", sewer_1Respawn.x);
                    PlayerPrefs.SetFloat("KarlinusPosY", sewer_1Respawn.y);
                    PlayerPrefs.SetFloat("KarlinusPosZ", sewer_1Respawn.z);
                    SceneManager.LoadScene("sewer");                    
                    break;
                case Scenes.CITY_1:
                    PlayerPrefs.SetFloat("KarlinusPosX", city_1Respawn.x);
                    PlayerPrefs.SetFloat("KarlinusPosY", city_1Respawn.y);
                    PlayerPrefs.SetFloat("KarlinusPosZ", city_1Respawn.z);
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
