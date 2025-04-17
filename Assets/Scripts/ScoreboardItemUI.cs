using UnityEngine;
using UnityEngine.UI;

public class ScoreboardItemUI : MonoBehaviour
{
    public Text Index;
    public Text PlayerName;
    public Text Score;
    public Text LevelCompleted;


    public void SetData(int rank, ScoreboardItem item,  bool isPlayer)
    {
        Index.text = rank.ToString();
        PlayerName.text = item.playerName;
        Score.text = item.score.ToString();
        LevelCompleted.text = item.levelCompleted.ToString();
        if (isPlayer)
        {
            PlayerName.color = Color.yellow;
            Score.color = Color.yellow;
            LevelCompleted.color = Color.yellow;

            PlayerName.fontStyle = FontStyle.Bold;
            Score.fontStyle = FontStyle.Bold;
            LevelCompleted.fontStyle = FontStyle.Bold;
        }
        else
        {
            PlayerName.color = Color.white;
            Score.color = Color.white;
            LevelCompleted.color = Color.white;

            PlayerName.fontStyle = FontStyle.Normal;
            Score.fontStyle = FontStyle.Normal;
            LevelCompleted.fontStyle = FontStyle.Normal;
        }
    }
}
