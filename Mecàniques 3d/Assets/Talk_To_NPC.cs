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
        if(misions.Instance.PrincipalMision.MisionsCompleted[2] == true && numDialog < 3 && scene == 0)
        {
            numDialog = 8;
        }
        if (misions.Instance.ActualMision == misions.Misions.M2 && scene == 1)
        {
            switch(numDialog)
            {
                case 0:
                    numDialog = 4;
                    break;
                case 1:
                    numDialog = 5;
                    break;
            }
        }
		if(misions.Instance.Player.GetComponent<HUD>().finalDialog && start)
        {
            misions.Instance.playerMovement.state = Controller.playerState.IDLE;
            misions.Instance.secundaryCamera.SetActive(false);
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

            misions.Instance.secundaryCamera.SetActive(true);
            if(misions.Instance.mainCamera.transform.position.z >= transform.position.z) misions.Instance.secundaryCamera.transform.position = new Vector3(misions.Instance.mainCamera.transform.position.x, misions.Instance.mainCamera.transform.position.y, misions.Instance.mainCamera.transform.position.z + 2.444f);
            else misions.Instance.secundaryCamera.transform.position = new Vector3(misions.Instance.mainCamera.transform.position.x, misions.Instance.mainCamera.transform.position.y, misions.Instance.mainCamera.transform.position.z - 2.444f);
            if (misions.Instance.mainCamera.transform.position.x - transform.position.x >= 0.5f) misions.Instance.secundaryCamera.transform.position = new Vector3(misions.Instance.mainCamera.transform.position.x + 1, misions.Instance.mainCamera.transform.position.y, misions.Instance.mainCamera.transform.position.z);
            else if (misions.Instance.mainCamera.transform.position.x - transform.position.x < 0.5f) misions.Instance.secundaryCamera.transform.position = new Vector3(misions.Instance.mainCamera.transform.position.x - 1, misions.Instance.mainCamera.transform.position.y, misions.Instance.mainCamera.transform.position.z);
            misions.Instance.secundaryCamera.transform.eulerAngles = new Vector3(20, 30, 0);
            misions.Instance.secundaryCamera.transform.LookAt (new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z));
            misions.Instance.playerMovement.state = Controller.playerState.HITTING;
            //if((misions.Instance.Player.transform.position - gameObject.transform.position).magnitude < 1) misions.Instance.Player.transform.position = new Vector3(misions.Instance.secundaryCamera.transform.position.x + 1.919f, misions.Instance.secundaryCamera.transform.position.y -2.402f, misions.Instance.secundaryCamera.transform.position.z + 1.412f);
            misions.Instance.Player.transform.LookAt(new Vector3(transform.position.x, misions.Instance.Player.transform.position.y, transform.position.z));
            transform.LookAt(new Vector3(misions.Instance.Player.transform.position.x, transform.position.y, misions.Instance.Player.transform.position.z));
            misions.Instance.Player.GetComponent<HUD>().showNpcDialog(scene, numDialog, size);
            start = true;
        }
    }
}
