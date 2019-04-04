using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talk_To_NPC : MonoBehaviour {

    public int scene;
    public int numDialog;
    public int size;
    private Vector3 originalDiraction;
    public bool start = false;
    public string animationName;
    private Animator anim;

    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {
        if (scene == 0)
        {
            anim.SetBool(animationName, true);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(GameObject.Find("Misiones").GetComponent<misions>().PrincipalMision.MisionsCompleted[2] == true && numDialog < 3)
        {
            numDialog = 8;
        }
		if(GameObject.Find("Misiones").GetComponent<misions>().Player.GetComponent<HUD>().finalDialog && start)
        {
            GameObject.Find("Misiones").GetComponent<misions>().playerMovement.state = Controller.playerState.IDLE;
            GameObject.Find("Misiones").GetComponent<misions>().secundaryCamera.SetActive(false);
            gameObject.transform.eulerAngles = originalDiraction;
            start = false;
        }
	}

    private void OnTriggerStay(Collider other)
    {
        //missatge hud
        if(other.tag == "Player" && Input.GetKey(KeyCode.E) && !start)
        {
            originalDiraction = gameObject.transform.eulerAngles;

            GameObject.Find("Misiones").GetComponent<misions>().secundaryCamera.SetActive(true);
            if(GameObject.Find("Misiones").GetComponent<misions>().mainCamera.transform.position.z >= transform.position.z) GameObject.Find("Misiones").GetComponent<misions>().secundaryCamera.transform.position = new Vector3(GameObject.Find("Misiones").GetComponent<misions>().mainCamera.transform.position.x, GameObject.Find("Misiones").GetComponent<misions>().mainCamera.transform.position.y, GameObject.Find("Misiones").GetComponent<misions>().mainCamera.transform.position.z + 2.444f);
            else GameObject.Find("Misiones").GetComponent<misions>().secundaryCamera.transform.position = new Vector3(GameObject.Find("Misiones").GetComponent<misions>().mainCamera.transform.position.x, GameObject.Find("Misiones").GetComponent<misions>().mainCamera.transform.position.y, GameObject.Find("Misiones").GetComponent<misions>().mainCamera.transform.position.z - 2.444f);
            if (GameObject.Find("Misiones").GetComponent<misions>().mainCamera.transform.position.x - transform.position.x >= 0.5f) GameObject.Find("Misiones").GetComponent<misions>().secundaryCamera.transform.position = new Vector3(GameObject.Find("Misiones").GetComponent<misions>().mainCamera.transform.position.x + 1, GameObject.Find("Misiones").GetComponent<misions>().mainCamera.transform.position.y, GameObject.Find("Misiones").GetComponent<misions>().mainCamera.transform.position.z);
            else if (GameObject.Find("Misiones").GetComponent<misions>().mainCamera.transform.position.x - transform.position.x < 0.5f) GameObject.Find("Misiones").GetComponent<misions>().secundaryCamera.transform.position = new Vector3(GameObject.Find("Misiones").GetComponent<misions>().mainCamera.transform.position.x - 1, GameObject.Find("Misiones").GetComponent<misions>().mainCamera.transform.position.y, GameObject.Find("Misiones").GetComponent<misions>().mainCamera.transform.position.z);
            GameObject.Find("Misiones").GetComponent<misions>().secundaryCamera.transform.eulerAngles = new Vector3(20, 30, 0);
            GameObject.Find("Misiones").GetComponent<misions>().secundaryCamera.transform.LookAt (new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z));
            GameObject.Find("Misiones").GetComponent<misions>().playerMovement.state = Controller.playerState.HITTING;
            //if((GameObject.Find("Misiones").GetComponent<misions>().Player.transform.position - gameObject.transform.position).magnitude < 1) GameObject.Find("Misiones").GetComponent<misions>().Player.transform.position = new Vector3(GameObject.Find("Misiones").GetComponent<misions>().secundaryCamera.transform.position.x + 1.919f, GameObject.Find("Misiones").GetComponent<misions>().secundaryCamera.transform.position.y -2.402f, GameObject.Find("Misiones").GetComponent<misions>().secundaryCamera.transform.position.z + 1.412f);
            GameObject.Find("Misiones").GetComponent<misions>().Player.transform.LookAt(new Vector3(transform.position.x, GameObject.Find("Misiones").GetComponent<misions>().Player.transform.position.y, transform.position.z));
            transform.LookAt(new Vector3(GameObject.Find("Misiones").GetComponent<misions>().Player.transform.position.x, transform.position.y, GameObject.Find("Misiones").GetComponent<misions>().Player.transform.position.z));
            GameObject.Find("Misiones").GetComponent<misions>().Player.GetComponent<HUD>().showNpcDialog(scene, numDialog, size);
            start = true;
        }
    }
}
