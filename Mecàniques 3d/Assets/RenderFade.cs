using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderFade : MonoBehaviour {

    public GameObject renderFadeImage;

    private void Awake()
    {
        if (GameObject.Find("CanvasSecundaryCam") != null) renderFadeImage = GameObject.Find("CanvasSecundaryCam").transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        
    }

    public void CameraFade()
    {
        renderFadeImage.GetComponent<Animation>().Play("CrossFade");
    }
}
