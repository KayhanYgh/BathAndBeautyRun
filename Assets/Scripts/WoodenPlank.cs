using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class WoodenPlank : MonoBehaviour
{
    [Space] public bool leftSideIsFull;
    public bool rightSideIsFull;
    public Animator animator;
    private static readonly int State = Animator.StringToHash("State");

    private void Update()
    {
//        if (!leftSideIsFull && !rightSideIsFull)
//        {
//            animator.SetInteger(State, 0);
//        }
    }

    //States:
    //Idle = 0
    //LightHitLeft = 1
    //LightHitRight = 2
    //HeavyHitLeft = 3
    //HeavyHitRight = 4
    [Button]
    public void HeavyHit(bool attackIsComingFromLeft)
    {
        if (attackIsComingFromLeft)
        {
            StartCoroutine(nameof(HeavyHitProcess));
        }
        else
        {
            StartCoroutine(nameof(HeavyHitProcess2));
        }
    }

    [Button]
    public void LightHit(bool attackIsComingFromLeft)
    {
        if (attackIsComingFromLeft)
        {
            StartCoroutine(nameof(LightHitProcess));
        }
        else
        {
            StartCoroutine(nameof(LightHitProcess2));
        }
    }

    private IEnumerator HeavyHitProcess()
    {
        animator.SetInteger(State, 4);
        yield return new WaitForSeconds(0.3f);
        animator.SetInteger(State, 0);
        yield return null;
    }

    private IEnumerator HeavyHitProcess2()
    {
        animator.SetInteger(State, 3);
        yield return new WaitForSeconds(0.3f);
        animator.SetInteger(State, 0);
        yield return null;
    }

    private IEnumerator LightHitProcess()
    {
        animator.SetInteger(State, 2);
        yield return new WaitForSeconds(0.3f);
        animator.SetInteger(State, 0);
        yield return null;
    }

    private IEnumerator LightHitProcess2()
    {
        animator.SetInteger(State, 1);
        yield return new WaitForSeconds(0.3f);
        animator.SetInteger(State, 0);
        yield return null;
    }
}