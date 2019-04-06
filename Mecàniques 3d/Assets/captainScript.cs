using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class captainScript : MonoBehaviour {

    private GameObject batallafinal;
    private GameObject player;
    private Vector3 playerResDest;
    private bool respawning;
    private bool talked;
    private Vector3 originalDiraction;
    private GameObject CameraCanvas;
    private GameObject NPC_Camera;

    // Use this for initialization
    void Start () {
        batallafinal = GameObject.Find("batalla final");
        player = GameObject.Find("Jugador");
        respawning = false;
        playerResDest = new Vector3(9.79f, 81.264f, -308.37f);
        if(misions.respawnIndex == 4)
        {
            for (int i = 0; i < batallafinal.transform.GetChild(0).childCount - 6; i++) batallafinal.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
            respawning = true;
            StartCoroutine(ExecuteAfterTime(3));
        }
        if (GameObject.Find("CanvasSecundaryCam") != null) CameraCanvas = GameObject.Find("CanvasSecundaryCam");
        talked = false;
    }

    private void FixedUpdate()
    {
        if (respawning)
        {
            player.GetComponent<Controller>().state = Controller.playerState.HITTING;
            player.GetComponent<liquidState>().showLiquid();
            player.transform.position = Vector3.Lerp(player.transform.position, playerResDest, 1.25f * Time.deltaTime);
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        player.GetComponent<Controller>().state = Controller.playerState.IDLE;
        player.GetComponent<liquidState>().hideLiquid();
        player.transform.LookAt(transform.position);
        respawning = false;
    }

    IEnumerator ExecuteAfterTime2(float time)
    {
        yield return new WaitForSeconds(time);

        for (int i = 0; i < batallafinal.transform.GetChild(0).childCount - 6; i++) batallafinal.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
        batallafinal.GetComponent<finalBattleManager>().restartBattle();
        batallafinal.GetComponent<finalBattleManager>().expandSphere();
        if (CameraCanvas != null) CameraCanvas.SetActive(true);
        misions.Instance.playerMovement.state = Controller.playerState.IDLE;
        Destroy(NPC_Camera.GetComponent<Camera>());
        misions.nextEvent = true;
        respawning = true;
        StartCoroutine(StartBattle(2));
    }

    IEnumerator StartBattle(float time)
    {
        yield return new WaitForSeconds(time);

        loadScreen.Instancia.CargarEscena("StoneChamber");
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && !talked)
        {
            originalDiraction = gameObject.transform.eulerAngles;
            if (CameraCanvas != null) CameraCanvas.SetActive(false);
            NPC_Camera = new GameObject();
            NPC_Camera.AddComponent<Camera>();
            if (misions.Instance.mainCamera.transform.position.z >= transform.position.z) NPC_Camera.transform.position = new Vector3(misions.Instance.mainCamera.transform.position.x, misions.Instance.mainCamera.transform.position.y, misions.Instance.mainCamera.transform.position.z + 2.444f);
            else NPC_Camera.transform.position = new Vector3(misions.Instance.mainCamera.transform.position.x, misions.Instance.mainCamera.transform.position.y, misions.Instance.mainCamera.transform.position.z - 2.444f);
            if (misions.Instance.mainCamera.transform.position.x - transform.position.x >= 0.5f) NPC_Camera.transform.position = new Vector3(misions.Instance.mainCamera.transform.position.x + 1, misions.Instance.mainCamera.transform.position.y, misions.Instance.mainCamera.transform.position.z);
            else if (misions.Instance.mainCamera.transform.position.x - transform.position.x < 0.5f) NPC_Camera.transform.position = new Vector3(misions.Instance.mainCamera.transform.position.x - 1, misions.Instance.mainCamera.transform.position.y, misions.Instance.mainCamera.transform.position.z);
            NPC_Camera.transform.eulerAngles = new Vector3(20, 30, 0);
            NPC_Camera.transform.LookAt(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z));
            misions.Instance.playerMovement.state = Controller.playerState.HITTING;
            //if((misions.Instance.Player.transform.position - gameObject.transform.position).magnitude < 1) misions.Instance.Player.transform.position = new Vector3(NPC_Camera.transform.position.x + 1.919f, NPC_Camera.transform.position.y -2.402f, NPC_Camera.transform.position.z + 1.412f);
            misions.Instance.Player.transform.LookAt(new Vector3(transform.position.x, misions.Instance.Player.transform.position.y, transform.position.z));
            transform.LookAt(new Vector3(misions.Instance.Player.transform.position.x, transform.position.y, misions.Instance.Player.transform.position.z));
            misions.Instance.Player.GetComponent<HUD>().showNpcDialog(2, 0, 35);
            StartCoroutine(ExecuteAfterTime2(4));
            talked = true;
        }
    }
}
