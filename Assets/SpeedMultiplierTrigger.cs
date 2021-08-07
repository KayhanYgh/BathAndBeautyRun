using UnityEngine;

public class SpeedMultiplierTrigger : MonoBehaviour
{
    public float speedMultiplier;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<RunnerPlayer>())
        {
            RunnerPlayer.Instance.movementSpeed *= speedMultiplier;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = new Color(1, 1, 1, 0.5f);
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}