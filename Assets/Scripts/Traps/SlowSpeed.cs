using System;
using UnityEngine;

namespace IO.Traps
{
    public class SlowSpeed : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
//            if (other.GetComponent<Character.Character>())
//            {
//                other.GetComponent<Character.Character>().movementSpeedMode = MovementSpeedMode.Slow;
//            }
        }

        private void OnTriggerExit(Collider other)
        {
//            if (other.GetComponent<Character.Character>())
//            {
//                other.GetComponent<Character.Character>().movementSpeedMode = MovementSpeedMode.Normal;
//            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = new Color(0.5f, 0.5f, 1, 0.25f);
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
            Gizmos.color = new Color(0.5f, 0.5f, 1, 1f);
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
    }
}