using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
public class CollectableItems : MonoBehaviour
{
    public int levelEffect;
    public UnityEvent onCollect;

    public void Collect()
    {
        onCollect?.Invoke();

        if (levelEffect > 0)
        {
            Score.ScoreManager.Instance.scoreText.text = (int.Parse(Score.ScoreManager.Instance.scoreText.text)+ levelEffect).ToString();
        }

        Debug.Log($"Dirt level {levelEffect}");
        Player.Instance.dirtyLevel += levelEffect;
        Player.Instance.dirtyLevelSlider.value = Player.Instance.dirtyLevel;


        gameObject.GetComponent<BoxCollider>().enabled = false;
    }
}
