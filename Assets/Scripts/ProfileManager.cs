using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager Instance;

    private void OnEnable()
    {
        Instance = this;
    }

    public Image first_flagImage;
    public Text first_nameText;

    public Image second_flagImage;
    public Text second_nameText;

    public Profile player;
    public Profile opponent;

    public Sprite[] flags;
    public string[] names;

    private void Start()
    {
        opponent.flag = flags[Random.Range(0, flags.Length - 1)];
        opponent.name = names[Random.Range(0, names.Length - 1)];

        SetRanking(player, opponent);
    }

    public void SetRanking(Profile firstPlayer, Profile secondPlayer)
    {
        first_nameText.text = firstPlayer.name;
        first_flagImage.sprite = firstPlayer.flag;

        second_nameText.text = secondPlayer.name;
        second_flagImage.sprite = secondPlayer.flag;
    }
}

[System.Serializable]
public class Profile
{
    public Sprite flag;
    public string name;
}