using Score;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public Transform player;
    public Transform opponent;
    public Transform endPoint;

    private bool _playerIsTheWinner;

    private void Update()
    {
        if (ScoreManager.Instance.Won || ScoreManager.Instance.Lost) return;

        if (!_opponentArrived)
        {
            _playerIsTheWinner = Vector3.Distance(player.position, endPoint.position) <
                                 Vector3.Distance(opponent.position, endPoint.position);
        }
        else
        {
            _playerIsTheWinner = false;
            ScoreManager.Instance.Lose("");
        }

        if (_playerIsTheWinner)
        {
            ProfileManager.Instance.SetRanking(ProfileManager.Instance.player, ProfileManager.Instance.opponent);
        }
        else
        {
            ProfileManager.Instance.SetRanking(ProfileManager.Instance.opponent, ProfileManager.Instance.player);
        }
    }

    private bool _opponentArrived;

    private void OnTriggerEnter(Collider other)
    {
        if (ScoreManager.Instance.Won || ScoreManager.Instance.Lost) return;

        if (other.GetComponent<Player.Player>())
        {
//            if (!other.GetComponent<Player.Player>().npc)
//            {
//                if (_playerIsTheWinner && !_opponentArrived)
//                {
//                    ScoreManager.Instance.Win("");
//                }
//                else
//                {
//                    ScoreManager.Instance.Lose("");
//                }
//            }
//            else
//            {
//                _opponentArrived = true;
//            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}