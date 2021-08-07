using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RandomIdentity : MonoBehaviour
{
    public Text nameText;
    public Image flagImage;
    public string[] names;
    public Sprite[] flags;

    private void Start()
    {
        nameText.text = names[Random.Range(0, names.Length - 1)];
        flagImage.sprite = flags[Random.Range(0, flags.Length - 1)];
    }
}