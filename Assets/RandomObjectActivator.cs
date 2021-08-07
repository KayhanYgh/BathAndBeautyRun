using UnityEngine;
using Random = UnityEngine.Random;

public class RandomObjectActivator : MonoBehaviour
{
    public GameObject[] objects;

    private void Start()
    {
        objects[Random.Range(0, objects.Length - 1)].SetActive(true);
    }
}