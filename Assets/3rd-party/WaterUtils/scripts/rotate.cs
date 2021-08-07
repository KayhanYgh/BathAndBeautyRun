using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterUtils {
    public class rotate : MonoBehaviour
    {
        [SerializeField] Vector3 rotVec=Vector3.zero;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector3 spd = rotVec * Time.deltaTime;
            transform.Rotate(spd.x, spd.y, spd.z);
        }
    }
} // namespace WaterUtils
