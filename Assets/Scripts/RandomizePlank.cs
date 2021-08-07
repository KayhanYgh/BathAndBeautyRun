using System.Collections;
using Score;
using UnityEngine;

public class RandomizePlank : MonoBehaviour
{
    private float _delay;
    public GameObject normalPlank;
    public GameObject doubleDamagePlank;
    public GameObject stunPlank;

    private bool _activated;

    void Update()
    {
        if (!ScoreManager.Instance.isStarted) return;

        if (_delay < Time.time)
        {
            if (_activated)
            {
                int chance = Random.Range(1, 100);

                if (chance > 90)
                {
                    int plankToActivate = Random.Range(1, 100);

                    if (plankToActivate >= 50)
                    {
                        normalPlank.SetActive(false);
                        doubleDamagePlank.SetActive(true);
                        stunPlank.SetActive(false);
                    }
                    else
                    {
                        normalPlank.SetActive(false);
                        doubleDamagePlank.SetActive(false);
                        stunPlank.SetActive(true);
                    }

                    StartCoroutine(nameof(ResetEveryThing));
                }
            }

            _activated = true;
            _delay = Time.time + Random.Range(3, 6);
        }
    }

    private IEnumerator ResetEveryThing()
    {
        yield return new WaitForSeconds(5);

        normalPlank.SetActive(true);
        doubleDamagePlank.SetActive(false);
        stunPlank.SetActive(false);
        yield return new WaitForSeconds(0);
        yield break;
    }
}