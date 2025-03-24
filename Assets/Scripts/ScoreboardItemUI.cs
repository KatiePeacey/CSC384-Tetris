using UnityEngine;
using UnityEngine.UI;

public class ScoreboardItemUI : MonoBehaviour
{
    public Text Index;
    public Text PlayerName;
    public Text Score;
    public Text LevelCompleted;

    public void SetData(int rank, ScoreboardItem item)
    {
        Index.text = rank.ToString();
        PlayerName.text = item.playerName;
        Score.text = item.score.ToString();
        LevelCompleted.text = item.levelCompleted.ToString();
    }
}
