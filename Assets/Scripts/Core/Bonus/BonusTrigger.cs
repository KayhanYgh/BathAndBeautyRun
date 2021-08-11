using UnityEngine;
using UnityEngine.Events;

public class BonusTrigger : MonoBehaviour
{
    public int bonus;

    public UnityEvent onTriggerEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            BonusManager.Instance.bonusMultiplier = bonus;
            onTriggerEnter?.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
