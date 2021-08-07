using UnityEngine;
using Random = UnityEngine.Random;

public class RandomizeCharacterMesh : MonoBehaviour
{
    public Mesh[] characters;

    private SkinnedMeshRenderer _skinnedMeshRenderer;

    private void Start()
    {
        _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        _skinnedMeshRenderer.sharedMesh = characters[Random.Range(0, characters.Length - 1)];
    }
}