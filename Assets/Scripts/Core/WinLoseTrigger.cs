using UnityEngine;

public class WinLoseTrigger : MonoBehaviour
{
    public bool winTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>())
        {
            if(winTrigger)
            {
                Score.ScoreManager.Instance.Win("");
            }
            else
            {
                Score.ScoreManager.Instance.Lose("");
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;

        if (winTrigger)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
