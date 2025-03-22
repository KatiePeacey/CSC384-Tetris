using TMPro;
using UnityEngine;
using TMPro;

public class ScoreboardItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI indexLabel;
    [SerializeField] private TextMeshProUGUI playerNameLabel;
    [SerializeField] private TextMeshProUGUI scoreLabel;
    [SerializeField] private TextMeshProUGUI levelsCompletedLabel;

    public void SetData(int index, ScoreboardItem scoreItem)
    {
        indexLabel.text = string.Format("{0}", index);
        playerNameLabel.text = string.Format("{0}", scoreItem.playerName);
        scoreLabel.text = string.Format("{0}", scoreItem.score);
        levelsCompletedLabel.text = string.Format("{0}", scoreItem.levelsCompleted);
    }
}
