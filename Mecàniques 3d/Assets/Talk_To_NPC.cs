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
    private GameObject NPC_Camera;
    private GameObject CameraCanvas;

    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        if (GameObject.Find("CanvasSecundaryCam") != null) CameraCanvas = GameObject.Find("CanvasSecundaryCam");
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
            NPC_Camera.SetActive(false);
            gameObject.transform.eulerAngles = originalDiraction;
            misions.Instance.mainCamera.transform.parent.GetComponent<CameraFollow>().enabled = true;
            if (CameraCanvas != null) CameraCanvas.SetActive(true);
            start = false;
            Destroy(NPC_Camera.GetComponent<Camera>());
        }
	}

    private void OnTriggerStay(Collider other)
    {
        //missatge hud
        if(other.tag == "Player" && Input.GetKey(KeyCode.E) && !start)
        {            
            originalDiraction = gameObject.transform.eulerAngles;
            NPC_Camera = new GameObject();
            NPC_Camera.AddComponent<Camera>();
            NPC_Camera.SetActive(true);
            misions.Instance.mainCamera.transform.parent.GetComponent<CameraFollow>().enabled = false;
            if (CameraCanvas != null) CameraCanvas.SetActive(false);
            if(misions.Instance.mainCamera.transform.position.z >= transform.position.z) NPC_Camera.transform.position = new Vector3(misions.Instance.mainCamera.transform.position.x, misions.Instance.mainCamera.transform.position.y, misions.Instance.mainCamera.transform.position.z + 2.444f);
            else NPC_Camera.transform.position = new Vector3(misions.Instance.mainCamera.transform.position.x, misions.Instance.mainCamera.transform.position.y, misions.Instance.mainCamera.transform.position.z - 2.444f);
            if (misions.Instance.mainCamera.transform.position.x - transform.position.x >= 0.5f) NPC_Camera.transform.position = new Vector3(misions.Instance.mainCamera.transform.position.x + 1, misions.Instance.mainCamera.transform.position.y, misions.Instance.mainCamera.transform.position.z);
            else if (misions.Instance.mainCamera.transform.position.x - transform.position.x < 0.5f) NPC_Camera.transform.position = new Vector3(misions.Instance.mainCamera.transform.position.x - 1, misions.Instance.mainCamera.transform.position.y, misions.Instance.mainCamera.transform.position.z);
            NPC_Camera.transform.eulerAngles = new Vector3(20, 30, 0);
            NPC_Camera.transform.LookAt (new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z));
            misions.Instance.playerMovement.state = Controller.playerState.HITTING;
            //if((misions.Instance.Player.transform.position - gameObject.transform.position).magnitude < 1) misions.Instance.Player.transform.position = new Vector3(NPC_Camera.transform.position.x + 1.919f, NPC_Camera.transform.position.y -2.402f, NPC_Camera.transform.position.z + 1.412f);
            misions.Instance.Player.transform.LookAt(new Vector3(transform.position.x, misions.Instance.Player.transform.position.y, transform.position.z));
            transform.LookAt(new Vector3(misions.Instance.Player.transform.position.x, transform.position.y, misions.Instance.Player.transform.position.z));
            misions.Instance.Player.GetComponent<HUD>().showNpcDialog(scene, numDialog, size);
            start = true;
        }
    }
}
