using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finalBattleManager : MonoBehaviour {

    private enum battleStates { LOOKING, WAITING, EXPANDING1, EXPANDING2, RETURNING1, RETURNING2}
    private battleStates battleState;
    private GameObject player;
    private GameObject forceSphere;
    private GameObject forceSphereDest;
    private GameObject forceSphereStart;
    private GameObject hideSphere;
    private GameObject hideSphereDest;
    private GameObject hideSphereStart;
    public GameObject[] ConoVisions;
    public GameObject[] Bosses;
    private int bossI;

    // Use this for initialization
    void Start () {
        battleState = battleStates.LOOKING;
        player = GameObject.Find("Jugador");
        forceSphere = GameObject.Find("ForceSphere");
        forceSphereDest = GameObject.Find("ForceSphereDest");
        forceSphereStart = GameObject.Find("ForceSphereStart");
        hideSphere = GameObject.Find("HideSphere");
        hideSphereDest = GameObject.Find("HideSphereDest");
        hideSphereStart = GameObject.Find("HideSphereStart");
        forceSphere.GetComponent<SphereCollider>().enabled = false;
        for (int i = Bosses.Length - 1; i > 0; i--)
        {
            Bosses[i].SetActive(false);
            Bosses[i].GetComponent<bossIA>().enabled = false;
        }
        if (misions.respawnIndex == 4)
            bossI = 0;
        else
        {
            bossI = 1;
            Bosses[bossI].SetActive(true);
            Bosses[bossI].GetComponent<bossIA>().enabled = true;
        }
}
	
	// Update is called once per frame
	void Update () {
		switch(battleState)
        {
            case battleStates.LOOKING:               
                break;
            case battleStates.EXPANDING1:
                forceSphere.transform.LookAt(player.transform.position);
                forceSphere.transform.localScale = Vector3.Lerp(forceSphere.transform.localScale, hideSphereDest.transform.localScale,  Time.deltaTime);
                hideSphere.transform.localScale = Vector3.Lerp(hideSphere.transform.localScale, hideSphereDest.transform.localScale, Time.deltaTime);
                break;
            case battleStates.EXPANDING2:
                forceSphere.transform.LookAt(player.transform.position);
                forceSphere.transform.localScale = Vector3.Lerp(forceSphere.transform.localScale, forceSphereDest.transform.localScale, Time.deltaTime);
                break;
            case battleStates.RETURNING1:
                forceSphere.transform.LookAt(player.transform.position);
                forceSphere.transform.localScale = Vector3.Lerp(forceSphere.transform.localScale, hideSphereDest.transform.localScale, 2.0f * Time.deltaTime);
                break;
            case battleStates.RETURNING2:
                forceSphere.transform.LookAt(player.transform.position);
                Bosses[bossI].transform.LookAt(player.transform.position);
                forceSphere.transform.localScale = Vector3.Lerp(forceSphere.transform.localScale, forceSphereStart.transform.localScale, Time.deltaTime);
                hideSphere.transform.localScale = Vector3.Lerp(hideSphere.transform.localScale, hideSphereStart.transform.localScale, Time.deltaTime);
                break;
            default:
                break;
        }
	}
    public void restartBattle()
    {
        Start();
        bossI = 0;
    }
    public void expandSphere()
    {
        battleState = battleStates.EXPANDING1;
        forceSphere.GetComponent<SphereCollider>().enabled = true;
        for (int i = 0; i < ConoVisions.Length; i++) ConoVisions[i].GetComponent<MeshRenderer>().enabled = false;
        Bosses[bossI].GetComponent<CapsuleCollider>().enabled = false;
        StartCoroutine(ExecuteAfterTime(3.5f));
    }
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        switch (battleState)
        {
            case battleStates.EXPANDING1:
                battleState = battleStates.EXPANDING2;
                StartCoroutine(ExecuteAfterTime(4.5f));
                break;
            case battleStates.EXPANDING2:
                battleState = battleStates.RETURNING1;
                Bosses[bossI].SetActive(false);
                StartCoroutine(ExecuteAfterTime(3.5f));
                break;
            case battleStates.RETURNING1:
                battleState = battleStates.RETURNING2;
                forceSphere.GetComponent<SphereCollider>().enabled = false;
                bossI++;
                if (Bosses.Length <= bossI) misions.nextEvent = true;
                Bosses[bossI].SetActive(true);
                StartCoroutine(ExecuteAfterTime(3.5f));
                break;
            case battleStates.RETURNING2:
                battleState = battleStates.LOOKING;
                Bosses[bossI].GetComponent<bossIA>().enabled = true;
                for (int i = 0; i < ConoVisions.Length; i++)
                {
                    ConoVisions[i].GetComponent<MeshRenderer>().enabled = true;
                    ConoVisions[i].GetComponent<AreaVisionBoss>().restartCone();
                }
                player.GetComponent<Controller>().state = Controller.playerState.IDLE;
                break;
            default:
                break;
        }
    }
}
