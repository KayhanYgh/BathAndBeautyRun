using Systems;
using Score;
using UnityEngine;

namespace Traps
{
    public class DeathTrigger : MonoBehaviour
    {
        public bool kill;

        private void OnTriggerEnter(Collider other)
        {
            if (ScoreManager.Instance.Finished || ScoreManager.Instance.Won || ScoreManager.Instance.Lost) return;

            if (other.GetComponent<HealthSystem>())
            {
                if (kill)
                {
                    other.GetComponent<HealthSystem>().Death();
                }
                else
                {
                    if (!other.GetComponent<HealthSystem>().knockedOut)
                    {
                        other.GetComponent<HealthSystem>().KnockOut();
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
        }
    }
}