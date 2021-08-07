using UnityEngine;

public class KillRunnerPlayerEvent : MonoBehaviour
{
    public void KillPlayer()
    {
        try
        {
            RunnerPlayer.Instance.Death();
        }
        catch
        {
        }
    }
}