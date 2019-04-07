using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setAllVisibleScript : MonoBehaviour {

    Vector3[] position;
    GameObject[] childs;
    Vector3 playerDist;
    public static float maxDist = 200;

	// Use this for initialization
	void Start () {
        childs = new GameObject[this.gameObject.transform.childCount];
        position = new Vector3[this.gameObject.transform.childCount];
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            childs[i] = this.gameObject.transform.GetChild(i).gameObject;
            position[i] = childs[i].transform.position;
        }

   }

    private void Update()
    {

        for (int i = 0; i < childs.Length; i++)
        {
            playerDist = new Vector3(GameObject.Find("Jugador").transform.position.x - position[i].x, 0.0f, GameObject.Find("Jugador").transform.position.z - position[i].z);
            if (childs[i].GetComponent<Renderer>() != null)
            {
                if (!checkVisible(childs[i].transform.position) && childs[i].GetComponent<Renderer>().enabled) childs[i].GetComponent<Renderer>().enabled = false;
                else if (checkVisible(childs[i].transform.position) && !childs[i].GetComponent<Renderer>().enabled) childs[i].GetComponent<Renderer>().enabled = true;
            }
            else
            {
                if (!checkVisible(childs[i].transform.position) && childs[i].activeSelf) childs[i].SetActive(false);
                else if (checkVisible(childs[i].transform.position) && !childs[i].activeSelf) childs[i].SetActive(true);
            }
        }
    }

    private bool checkVisible(Vector3 childPos)
    {
        if (playerDist.magnitude < 50) return true;
        else if (playerDist.magnitude < maxDist && (Vector3.Angle(Camera.main.transform.forward, childPos - misions.Instance.Player.transform.position) < 90)) return true;
        return false;
    }
    //
}
