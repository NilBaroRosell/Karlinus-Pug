using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class servantUnlock_NPC : MonoBehaviour {

    private bool talked;
    public static bool canTalk;
    private Vector3 originalDiraction;
    private GameObject CameraCanvas;
    private GameObject NPC_Camera;
    Sprite interact;

    private void Start()
    {
        if (GameObject.Find("CanvasSecundaryCam") != null) CameraCanvas = GameObject.Find("CanvasSecundaryCam");
        talked = false;
        canTalk = false;
        interact = Resources.Load<Sprite>("Sprites/TalkSprite");
    }

    private void OnTriggerStay(Collider other)
    {
        if (canTalk && other.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.E) && !talked)
        {
            other.gameObject.GetComponentInParent<HUD>().hideInteractSprite();
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
            misions.Instance.Player.GetComponent<HUD>().showNpcDialog(1, 6, 40);
            StartCoroutine(ExecuteAfterTime(5));
            talked = true;
        }
        else if (other.gameObject.tag == "Player" && !talked) other.gameObject.GetComponentInParent<HUD>().showInteractSprite(this.transform.position, interact);
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player") collision.gameObject.GetComponentInParent<HUD>().hideInteractSprite();
    }

    public IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        if (CameraCanvas != null) CameraCanvas.SetActive(true);
        Destroy(NPC_Camera.GetComponent<Camera>());
        misions.nextEvent = true;
    }
}
