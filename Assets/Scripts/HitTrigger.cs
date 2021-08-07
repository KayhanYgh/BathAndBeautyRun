using UnityEngine;

public class HitTrigger : MonoBehaviour
{
    public bool sendMessage;
    public bool doubleDamage;
    public bool stun;
    public bool heavyHit;
    public FightingPlayerSystem fightingPlayerSystem;

    private void OnTriggerEnter(Collider other)
    {
        if (sendMessage)
        {
            if (other.GetComponent<FightingPlayerSystem>())
            {
                if (other.GetComponent<FightingPlayerSystem>() != fightingPlayerSystem)
                {
                    other.GetComponent<FightingPlayerSystem>().Death();
                }
            }

            return;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            if (heavyHit)
            {
                if (doubleDamage)
                {
                    other.gameObject.SendMessage("TakeHeavyHit");
                }
                else if (stun)
                {
                    other.gameObject.SendMessage("TakeHeavyHit_Stun");
                }
                else
                {
                    other.gameObject.SendMessage("TakeHeavyHit");
                }
            }
            else
            {
                if (doubleDamage)
                {
                    other.gameObject.SendMessage("TakeLightHit_DoubleDamage");
                }
                else if (stun)
                {
                    other.gameObject.SendMessage("TakeHeavyHit_Stun");
                }
                else
                {
                    other.gameObject.SendMessage("TakeLightHit");
                }
            }
        }
    }
}