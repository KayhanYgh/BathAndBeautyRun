using UnityEngine;
using UnityEngine.UI;

public class BonusManager : MonoBehaviour
{
    public static BonusManager Instance;
    public int bonusMultiplier;
    public Text coinText;

    private void OnEnable()
    {
        Instance = this;
    }

    public bool startCalculation;

    private int _lastSavedCoin;

    private void Start()
    {
        if (PlayerPrefs.HasKey("coin"))
        {
            _lastSavedCoin = PlayerPrefs.GetInt("coin");

            coinText.text = _lastSavedCoin.ToString();
        }
    }

    private float _delay;
    private int _coinsToAdd;

    private bool _flag;
    public void SetCoinsToAdd()
    {
        if (_flag) return;

        _coinsToAdd = int.Parse(Score.ScoreManager.Instance.scoreText.text) + int.Parse(Score.ScoreManager.Instance.scoreText.text) * bonusMultiplier;

        PlayerPrefs.SetInt("coin", _coinsToAdd);
        _flag = true;
    }


    private void Update()
    {
        if (startCalculation)
        {
            SetCoinsToAdd();

            if (_delay < Time.time)
            {
                if (_coinsToAdd > 0)
                {
                    coinText.text = (int.Parse(coinText.text) + 1).ToString();
                    _coinsToAdd--;
                }
                _delay = Time.time + 0.001f;
            }
        }
    }
}
