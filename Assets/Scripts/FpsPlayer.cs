using System;
using System.Collections;
using Score;
using Sirenix.OdinInspector;
using UnityEngine;

public class FpsPlayer : MonoBehaviour
{
    public static FpsPlayer Instance;

    [TabGroup("Health System")] public int health;
    [TabGroup("Health System")] public Animator takingHitAnimator;
    [TabGroup("Health System")] public GameObject indicator;

    private void OnEnable()
    {
        Instance = this;
    }

    [TabGroup("Animation")] public Animator animator;
    [TabGroup("Movement")] private float rotationOnX;

    [TabGroup("Movement")] [TabGroup("Movement")]
    public float mouseSensitivity = 90f;

    [TabGroup("Movement")] public Transform player;
    [TabGroup("Movement")] public FixedJoystick joystick;

    private void Update()
    {
        Movement();

        try
        {
            indicator.transform.LookAt(new Vector3(attackerTransform.position.x, indicator.transform.position.y,
                attackerTransform.position.z));
        }
        catch
        {
            //ignore
        }
    }

    private void Movement()
    {
        float mouseY = joystick.Vertical * Time.deltaTime * mouseSensitivity;
        float m_X = joystick.Horizontal * Time.deltaTime * mouseSensitivity;

        rotationOnX -= mouseY;

        rotationOnX = Mathf.Clamp(rotationOnX, 12, 45);
        transform.localEulerAngles = new Vector3(rotationOnX, 0, 0);

        player.Rotate(Vector3.up * m_X);


        if (player.eulerAngles.y < 190)
        {
            player.eulerAngles = new Vector3(0, 190, 0);
        }

        if (player.eulerAngles.y > 350)
        {
            player.eulerAngles = new Vector3(0, 350, 0);
        }
    }

    public void Kick()
    {
        animator.Play("Kick");
    }


    private Transform attackerTransform;

    public void TakeDamage(int damageValue, Transform attackerPosition)
    {
        attackerTransform = attackerPosition;
        indicator.transform.LookAt(new Vector3(attackerTransform.position.x, indicator.transform.position.y,
            attackerTransform.position.z));
        health -= damageValue;
        takingHitAnimator.Play("Hit");
        StartCoroutine(nameof(ShowingIndicatorProcess));

        if (health <= 0)
        {
            ScoreManager.Instance.Lose("");
        }
    }

    private IEnumerator ShowingIndicatorProcess()
    {
        indicator.SetActive(true);
        yield return new WaitForSeconds(2);
        indicator.SetActive(false);
        yield break;
    }
}