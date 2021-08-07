using Score;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    public int remaining;
    public float spawnEvery;
    public GameObject enemyPrefab;

    private void OnEnable()
    {
        Instance = this;
    }

    private float _delay;

    private void Update()
    {
        if (!ScoreManager.Instance.isStarted) return;
        if (remaining <= 0) return;

        if (_delay < Time.time)
        {
            Instantiate(enemyPrefab, transform.position, transform.rotation);
            remaining--;
            _delay = spawnEvery + Time.time;
        }
    }
}