using Score;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class FightingPlayerSystem : MonoBehaviour
{
    [TabGroup("Setting")] public bool isPlayer;
    [TabGroup("Setting")] public Animator animator;
    [TabGroup("Setting")] private float stamina;
    [TabGroup("Setting")] public float increaseStaminaRate;
    [TabGroup("Setting")] public GameObject attackTrigger;
    [TabGroup("Setting")] public Collider PlayerCollider;
    [TabGroup("Setting")] public bool actions = true;

    [TabGroup("Setting")] public UnityEvent onDeath;
    [TabGroup("Setting")] private static readonly int Death1 = Animator.StringToHash("Death");

    [TabGroup("Npc")] public int dogeChance;

    private static readonly int DeathMotion = Animator.StringToHash("DeathMotion");

    public void EnableActions() => actions = true;
    public void DisableActions() => actions = false;

    private float _delay;
    private bool _flag;

    private void Update()
    {
        if (!ScoreManager.Instance.isStarted || ScoreManager.Instance.Won || ScoreManager.Instance.Lost) return;

        ManageStamina();

        if (!isPlayer)
        {
            if (_delay < Time.time)
            {
                if (_flag)
                    Attack();
                _flag = true;
                _delay = Time.time + Random.Range(3f, 5f);
            }
        }
    }

    public void Attack()
    {
        if (!actions) return;
        animator.Play("Kick");
        stamina = 0;
    }

    public void ActiveAttackCollider() => attackTrigger.SetActive(true);
    public void DeActiveAttackCollider() => attackTrigger.SetActive(false);

    public void ActiveHealthCollider() => PlayerCollider.enabled = true;
    public void DeActiveHealthCollider() => PlayerCollider.enabled = false;


    public void Dodge()
    {
        if (!actions) return;

        animator.Play("Dodge");
        stamina = 0;
    }

    public void Death()
    {
        if (Random.Range(0, 100) < dogeChance && !isPlayer)
        {
            Dodge();
            return;
        }

//        if (Random.Range(0, 100) > 50 && !isPlayer)
//        {
//            animator.enabled = false;
//        }
//        else
//        {
        animator.SetBool(Death1, true);

        if (Random.Range(0, 100) > 50)
        {
            animator.SetInteger(DeathMotion, 1);
        }
        else
        {
            animator.SetInteger(DeathMotion, 2);
        }
//        }

        if (isPlayer)
        {
            ScoreManager.Instance.Lose("");
        }

        onDeath?.Invoke();
    }

    private void ManageStamina()
    {
        if (stamina >= 1)
        {
            stamina = 1;
        }
        else
        {
            stamina += increaseStaminaRate * Time.deltaTime;
        }
    }
}