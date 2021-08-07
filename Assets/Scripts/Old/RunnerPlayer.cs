using Score;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RunnerPlayer : MonoBehaviour
{
    public static RunnerPlayer Instance;

    private void OnEnable()
    {
        Instance = this;
    }

    [TabGroup("Settings")] public Transform playerTransform;
    [TabGroup("Settings")] public float movementSpeed;
    [TabGroup("Settings")] public Animator animator;
    [TabGroup("Settings")] public bool canMove;
    private static readonly int Run = Animator.StringToHash("Run");
    [TabGroup("Settings")] public float stamina;
    [TabGroup("Settings")] public float increaseStaminaRate;
    [TabGroup("Settings")] public GameObject kickTrigger;

    [TabGroup("Death")] public UnityEvent onDeath;

    [TabGroup("UI")] public Slider staminaBar;

    private void Update()
    {
        if (ScoreManager.Instance.Won)
        {
            animator.SetBool("Dance", true);
        }

        if (died || !ScoreManager.Instance.isStarted || ScoreManager.Instance.Won || ScoreManager.Instance.Lost) return;
        Movement();
        ManageStamina();

        if (Input.GetMouseButtonDown(0))
        {
            if (stamina >= 1)
            {
                animator.Play("Kick");
                stamina = 0;
            }
        }
    }

    public bool died;

    public void Death()
    {
        died = true;
        animator.SetBool("Death", true);
        ScoreManager.Instance.Lose("");
        onDeath?.Invoke();
    }

    private void ManageStamina()
    {
        staminaBar.value = stamina;

        if (stamina >= 1)
        {
            stamina = 1;
        }
        else
        {
            stamina += increaseStaminaRate * Time.deltaTime;
        }

        if (stamina >= 1)
        {
            staminaBar.gameObject.SetActive(false);
        }
    }

    public void CheckStamina()
    {
        staminaBar.gameObject.SetActive(stamina < 1);
    }

    private void Movement()
    {
        if (!ScoreManager.Instance.isStarted)
        {
            animator.SetBool(Run, false);
            return;
        }

        animator.SetBool(Run, true);


        //Movement
        if (canMove)
            playerTransform.position += playerTransform.forward * movementSpeed * Time.deltaTime;
    }

    public void ActiveKickCollider() => kickTrigger.SetActive(true);

    public void DeActiveKickCollider() => kickTrigger.SetActive(false);

    public void CanMove() => canMove = true;
    public void CantMove() => canMove = false;
}