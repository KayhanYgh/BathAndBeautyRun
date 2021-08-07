using UnityEngine;

namespace HandMadeTools
{
    public class BezierCurve : MonoBehaviour
    {
        public Transform startPoint;
        public Transform curvePoint;
        public Transform endPoint;

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 1, 1, 0.5f);
            Gizmos.DrawCube(startPoint.position, Vector3.one / 3);
            Gizmos.DrawCube(curvePoint.position, Vector3.one / 3);
            Gizmos.DrawCube(endPoint.position, Vector3.one / 3);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(startPoint.position, curvePoint.position);
            Gizmos.DrawLine(curvePoint.position, endPoint.position);
            for (float i = 0; i < 1; i += 0.001f)
            {
                Gizmos.color = new Color(1f, 0.16f, 0.16f, 0.4f);
                Gizmos.DrawWireSphere(GetBqcPoint(i, startPoint.position, curvePoint.position, endPoint.position),
                    0.01f);
            }
        }

        public Vector3 MoveAlongTheCurve(float time)
        {
            return GetBqcPoint(time, startPoint.position, curvePoint.position, endPoint.position);
        }

        private Vector3 GetBqcPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            Vector3 p = (uu * p0) + (2 * u * t * p1) + (tt * p2);
            return p;
        }
    }
}