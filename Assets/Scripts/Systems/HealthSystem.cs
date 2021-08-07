using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Systems
{
    public class HealthSystem : MonoBehaviour
    {
        public bool isDead;
        public bool knockedOut;

        [TabGroup("Events")] public UnityEvent onDeath;
        [TabGroup("Events")] public UnityEvent onKnockOut;
        [TabGroup("Events")] public UnityEvent onRevive;

        public void Death()
        {
            onDeath?.Invoke();
        }

        public void KnockOut()
        {
            onKnockOut?.Invoke();
        }

        public void Revive()
        {
            onRevive?.Invoke();
        }
    }
}