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
            Score.ScoreManager.Instance.scoreText.text = (int.Parse(Score.ScoreManager.Instance.scoreText.text) + levelEffect).ToString();
        }
        else
        {
            Player.Instance.stop = true;
            foreach (var item in Player.Instance.characterAnimators)
            {
                item.SetBool("TakingDamage", true);
            }
        }

        Player.Instance.dirtyLevel += levelEffect;
        Player.Instance.dirtyLevelSlider.value = Player.Instance.dirtyLevel;


        gameObject.GetComponent<BoxCollider>().enabled = false;
    }
}
