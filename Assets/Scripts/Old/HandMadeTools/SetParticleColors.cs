using UnityEngine;

public class SetParticleColors : MonoBehaviour
{
    public ParticleSystem[] particles;

    public bool ad;

    private void Start()
    {
        if (ad) Destroy(gameObject, 5);
    }

    public void SetParticlesColors(Color color)
    {
        foreach (var item in particles)
        {
            var itemMain = item.main;
            itemMain.startColor = color;
        }
    }
}