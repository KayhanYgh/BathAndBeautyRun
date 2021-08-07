using UnityEngine;

public class WeightDetector : MonoBehaviour
{
    public WoodenPlank woodenPlank;
    public bool left;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (left)
            {
                woodenPlank.leftSideIsFull = true;

                if (other.GetComponent<Player.Player>())
                {
                    other.gameObject.GetComponent<Player.Player>().standingWoodenPlate = woodenPlank;
                }
            }
            else
            {
                woodenPlank.rightSideIsFull = true;

                if (other.GetComponent<Player.Player>())
                {
                    other.gameObject.GetComponent<Player.Player>().standingWoodenPlate = woodenPlank;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (left)
            {
                woodenPlank.leftSideIsFull = false;
            }
            else
            {
                woodenPlank.rightSideIsFull = false;
            }

            if (other.GetComponent<Player.Player>())
            {
                if (other.gameObject.GetComponent<Player.Player>().standingWoodenPlate == woodenPlank)
                {
                    other.gameObject.GetComponent<Player.Player>().standingWoodenPlate = null;
                }
            }
        }
    }
}