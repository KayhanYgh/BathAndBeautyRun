using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterUtils{
    public class ObjUpDown : MonoBehaviour
    {

        private Vector3 defPos;
        private float timer;

        // Use this for initialization
        void Start()
        {
            defPos = transform.position;
            timer = Random.value * 10f;
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
            transform.position = defPos + Vector3.up * Mathf.Sin(timer * 1f) * 1f;
        }
    }
} //namespace WaterUtils
