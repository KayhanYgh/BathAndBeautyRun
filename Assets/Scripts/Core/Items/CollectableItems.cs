using UnityEngine;
using UnityEngine.Events;

public class CollectableItems : MonoBehaviour
{
    public int levelEffect;
    public UnityEvent onCollect;

    public void Collect()
    {
        if (levelEffect > 0)
        {
            Score.ScoreManager.Instance.scoreText.text = (int.Parse(Score.ScoreManager.Instance.scoreText.text)+ levelEffect).ToString();
        }

        Player.Instance.dirtyLevel += levelEffect;
        Player.Instance.dirtyLevelSlider.value = Player.Instance.dirtyLevel;

        onCollect?.Invoke();

        gameObject.GetComponent<BoxCollider>().enabled = false;
    }
}
