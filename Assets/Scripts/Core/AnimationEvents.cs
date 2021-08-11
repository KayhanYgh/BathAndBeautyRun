using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
   public void TakingHitFinished()
    {
        Player.Instance.stop = false;
        foreach (var item in Player.Instance.characterAnimators)
        {
            item.SetBool("TakingDamage", false);
        }
    }
}
