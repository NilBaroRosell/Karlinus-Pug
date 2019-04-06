using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class captainScript : MonoBehaviour {

    private GameObject batallafinal;
    private GameObject player;
    private Vector3 playerResDest;
    private bool respawning;
    private bool talked;
    private Sprite interact;
    // Use this for initialization
    void Start () {
        batallafinal = GameObject.Find("batalla final");
        player = GameObject.Find("Jugador");
        respawning = false;
        talked = false;
        interact = Resources.Load<Sprite>("Sprites/TalkSprite");
        playerResDest = new Vector3(9.79f, 81.264f, -308.37f);
        if(misions.respawnIndex == 4)
        {
            for (int i = 0; i < batallafinal.transform.GetChild(0).childCount - 6; i++) batallafinal.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
            respawning = true;
            StartCoroutine(ExecuteAfterTime(3));
        }
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
    IEnumerator StartBattle(float time)
    {
        yield return new WaitForSeconds(time);

        loadScreen.Instancia.CargarEscena("StoneChamber");
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.E) && !talked)
        {
            for (int i = 0; i < batallafinal.transform.GetChild(0).childCount - 6; i++) batallafinal.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
            batallafinal.GetComponent<finalBattleManager>().restartBattle();
            batallafinal.GetComponent<finalBattleManager>().expandSphere();
            misions.nextEvent = true;
            respawning = true;
            talked = true;
            StartCoroutine(StartBattle(2));
        }
        else if (collision.gameObject.tag == "Player" && !talked) collision.gameObject.GetComponentInParent<HUD>().showInteractSprite(this.transform.position, interact);
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player") collision.gameObject.GetComponentInParent<HUD>().hideInteractSprite();
    }
}
