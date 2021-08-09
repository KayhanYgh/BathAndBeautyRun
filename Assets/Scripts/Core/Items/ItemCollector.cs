using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<CollectableItems>())
        {
            other.GetComponent<CollectableItems>().Collect();
            Player.Instance.ManageStage();
        }
    }
}
