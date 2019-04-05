using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderFade : MonoBehaviour {

    public GameObject renderFadeImage;

    private void Awake()
    {
        if (GameObject.Find("Canvas") != null) renderFadeImage = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
    }

    public void CameraFade()
    {
        renderFadeImage.GetComponent<Animation>().Play("CrossFade");
    }
}
