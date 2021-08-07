using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterUtils
{
    public class WaterLineHeightCtrl : MonoBehaviour {
        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            Material[] matArr = GetComponent<MeshRenderer>().materials;
            if (matArr.Length > 1) { // [1]=WaterLineMaterial
                matArr[1].SetFloat("_SurfaceHeight", transform.position.y);
            }
        }
    }
} //namespace WaterUtils
