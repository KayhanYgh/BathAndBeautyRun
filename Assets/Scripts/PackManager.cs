using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Events;

public class PackManager : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}