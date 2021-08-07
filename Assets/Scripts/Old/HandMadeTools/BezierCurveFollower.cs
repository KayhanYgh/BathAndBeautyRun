using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace HandMadeTools
{
    public class BezierCurveFollower : MonoBehaviour
    {
        [HideInInspector]public bool hasNavMeshAgent;
        public UnityEvent onJumpStarts;
        public UnityEvent onAir;
        public UnityEvent onArrivedToTheTheEndPoint;
        private void Start()
        {
            hasNavMeshAgent = GetComponent<NavMeshAgent>();
        }
    }
}