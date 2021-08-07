using UnityEngine;
using UnityEngine.AI;

namespace HandMadeTools
{
    public class MoveObjectOnBezierCurve : MonoBehaviour
    {
        public BezierCurve bezierCurve;
        public float movementSpeed = 1;
        private float _time;
        private GameObject throwObject;
        private bool _throwTheObject;
        private bool _disabled;

        private void Update()
        {
            if (throwObject)
            {
                if (_time < 1)
                {
                    _time += Time.deltaTime + movementSpeed;
                    if (throwObject)
                        throwObject.transform.position = bezierCurve.MoveAlongTheCurve(_time);

                    throwObject.GetComponent<BezierCurveFollower>().onAir?.Invoke();

                    if (_time > 0.99f)
                    {
                        if (throwObject.GetComponent<BezierCurveFollower>().hasNavMeshAgent)
                        {
                            throwObject.GetComponent<NavMeshAgent>().enabled = true;
                        }

                        throwObject.GetComponent<BezierCurveFollower>().onArrivedToTheTheEndPoint?.Invoke();

                        throwObject = null;
                    }
                }
            }
            else
            {
                _time = 0;
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (_disabled) return;
            if (other.GetComponent<BezierCurveFollower>())
            {
                if (other.GetComponent<BezierCurveFollower>().hasNavMeshAgent)
                {
                    other.GetComponent<NavMeshAgent>().enabled = false;
                    throwObject = other.gameObject;
                    _throwTheObject = true;
                }
                else
                {
                    throwObject = other.gameObject;
                    _throwTheObject = true;

                    gameObject.GetComponent<Rigidbody>().useGravity = false;
                    gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    gameObject.GetComponent<Collider>().enabled = false;
                }

                _disabled = true;
                Invoke(nameof(CreateNewOne), 0.25f);
                throwObject.GetComponent<BezierCurveFollower>().onJumpStarts?.Invoke();
            }
        }

        private void CreateNewOne()
        {
            GameObject recreatedObject = Instantiate(gameObject, transform.position, transform.rotation);
            recreatedObject.GetComponent<MoveObjectOnBezierCurve>().ResetTime();
            recreatedObject.GetComponent<MoveObjectOnBezierCurve>().gameObject.GetComponent<Rigidbody>()
                .useGravity = true;
            recreatedObject.GetComponent<MoveObjectOnBezierCurve>().gameObject.GetComponent<Rigidbody>()
                .isKinematic = true;
            recreatedObject.GetComponent<MoveObjectOnBezierCurve>().gameObject.GetComponent<Collider>()
                .enabled = true;

            Destroy(gameObject, 3);
        }


        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color = new Color(1, 1, 1, 0.5f);
            Gizmos.DrawCube(Vector3.zero, Vector3.one);

            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }

        private void ResetTime()
        {
            _time = 0;
        }
    }
}