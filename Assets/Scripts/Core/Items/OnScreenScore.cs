using UnityEngine;
using UnityEngine.UI;

public class OnScreenScore : MonoBehaviour
{
    public Text scoreText;

    public void SetScore(int scoreValue)
    {
        if(scoreValue < 0)
        {
            scoreText.color = Color.red;
        }

        scoreText.text = scoreValue > 0 ?  "+" + scoreValue : scoreValue.ToString();
    }
}
