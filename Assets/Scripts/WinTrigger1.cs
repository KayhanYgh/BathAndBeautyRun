using Score;
using UnityEngine;

public class WinTrigger1 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<RunnerPlayer>())
        {
            ScoreManager.Instance.Win("");
        }
    }
}
