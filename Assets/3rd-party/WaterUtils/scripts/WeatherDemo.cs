using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterUtils
{
    public class WeatherDemo : MonoBehaviour
    {
        [SerializeField] Rain rainScr = null;
        [SerializeField] Color SunnyBubbleCol0 = new Color(0.7f,0.7f,0.8f);
        [SerializeField] Color SunnyWaterCol1 = new Color(0.3f, 0.3f, 0.5f);
        [SerializeField] Color CloudyBubbleCol0 = new Color(0.5f, 0.5f, 0.5f);
        [SerializeField] Color CloudyWaterCol1 = new Color(0.2f, 0.2f, 0.2f);
        [SerializeField] ParticleSystem ThunderParticleSys = null;
        [SerializeField] MeshRenderer WaveMeshRenderer = null;

        float val = 0f;
        Color defRainCol;
        Vector3 defRainScale;
        Material skyBoxMat;
        Material waveMaterial;
        float defWaveAmpY;

        // Use this for initialization
        void Start()
        {
            defRainCol = rainScr.gameObject.GetComponent<MeshRenderer>().material.color;
            defRainScale = rainScr.gameObject.transform.parent.localScale;
            skyBoxMat = RenderSettings.skybox;
            ThunderParticleSys.gameObject.SetActive(false);
            waveMaterial = WaveMeshRenderer.material;
            defWaveAmpY = WaveMeshRenderer.gameObject.transform.localScale.y;
        }

        // Update is called once per frame
        void Update()
        {
            rainScr.gameObject.transform.parent.localScale = defRainScale * (1+(1 - val) * 10f);
            Color col = defRainCol;
            col.a = defRainCol.a * val;
            rainScr.gameObject.GetComponent<MeshRenderer>().material.color = col;
            ThunderParticleSys.gameObject.SetActive(val>0.8f);
            waveMaterial.SetColor("_BubbleColor", Color.Lerp(SunnyBubbleCol0, CloudyBubbleCol0, val));
            waveMaterial.SetColor("_WaterColor", Color.Lerp(SunnyWaterCol1, CloudyWaterCol1, val));
        }

        void OnGUI()
        {
            Vector2 scr = new Vector2((float)Screen.width, (float)Screen.height);
            val = GUI.HorizontalScrollbar(new Rect(scr.x * 0.05f, scr.y * 0.05f, scr.x * 0.9f, scr.y * 0.9f), val, 0.01f, 0f, 1f);
            skyBoxMat.SetFloat("_FadeRate", val);
            Vector3 scale = WaveMeshRenderer.gameObject.transform.localScale;
            scale.y = defWaveAmpY * val;
            WaveMeshRenderer.gameObject.transform.localScale = scale;
        }
    }
}