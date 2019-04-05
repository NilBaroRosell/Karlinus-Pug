using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class servantUnlock_NPC : MonoBehaviour {

    private bool talked;
    public static bool canTalk;
    private Vector3 originalDiraction;

    private void Start()
    {
        talked = false;
        canTalk = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (canTalk && other.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.E) && !talked)
        {
            originalDiraction = gameObject.transform.eulerAngles;

            misions.Instance.secundaryCamera.SetActive(true);
            if (misions.Instance.mainCamera.transform.position.z >= transform.position.z) misions.Instance.secundaryCamera.transform.position = new Vector3(misions.Instance.mainCamera.transform.position.x, misions.Instance.mainCamera.transform.position.y, misions.Instance.mainCamera.transform.position.z + 2.444f);
            else misions.Instance.secundaryCamera.transform.position = new Vector3(misions.Instance.mainCamera.transform.position.x, misions.Instance.mainCamera.transform.position.y, misions.Instance.mainCamera.transform.position.z - 2.444f);
            if (misions.Instance.mainCamera.transform.position.x - transform.position.x >= 0.5f) misions.Instance.secundaryCamera.transform.position = new Vector3(misions.Instance.mainCamera.transform.position.x + 1, misions.Instance.mainCamera.transform.position.y, misions.Instance.mainCamera.transform.position.z);
            else if (misions.Instance.mainCamera.transform.position.x - transform.position.x < 0.5f) misions.Instance.secundaryCamera.transform.position = new Vector3(misions.Instance.mainCamera.transform.position.x - 1, misions.Instance.mainCamera.transform.position.y, misions.Instance.mainCamera.transform.position.z);
            misions.Instance.secundaryCamera.transform.eulerAngles = new Vector3(20, 30, 0);
            misions.Instance.secundaryCamera.transform.LookAt(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z));
            misions.Instance.playerMovement.state = Controller.playerState.HITTING;
            //if((misions.Instance.Player.transform.position - gameObject.transform.position).magnitude < 1) misions.Instance.Player.transform.position = new Vector3(misions.Instance.secundaryCamera.transform.position.x + 1.919f, misions.Instance.secundaryCamera.transform.position.y -2.402f, misions.Instance.secundaryCamera.transform.position.z + 1.412f);
            misions.Instance.Player.transform.LookAt(new Vector3(transform.position.x, misions.Instance.Player.transform.position.y, transform.position.z));
            transform.LookAt(new Vector3(misions.Instance.Player.transform.position.x, transform.position.y, misions.Instance.Player.transform.position.z));
            misions.Instance.Player.GetComponent<HUD>().showNpcDialog(1, 6, 40);
            StartCoroutine(misions.Instance.ExecuteAfterTime(5));
            talked = true;
        }
    }
}
