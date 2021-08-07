using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSkybox : MonoBehaviour {
    Material skyBoxMat;
    float timer;

    // Use this for initialization
    void Start () {
        skyBoxMat = RenderSettings.skybox;
        timer = 0f;
    }

    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime*0.2f;
        float rate = Mathf.PingPong(timer, 1f);
        skyBoxMat.SetFloat("_FadeRate", rate);
		
	}
}
