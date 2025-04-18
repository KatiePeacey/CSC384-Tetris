using UnityEngine;
using UnityEngine.UI;

public class BackButtonManager : MonoBehaviour
{
    public Button BackButton;
    public GameManagerBehaviour gameManager;
    public GameObject Leaderboard;
    public GameObject gameOverMenu;
    public GameObject game;
    public GameObject MainMenu;
    private void Start()
    {
        BackButton.onClick.AddListener(OnSubmit);
    }
    private void OnSubmit()
    {
        switch (gameManager.currentState)
        {
            case GameState.Leaderboard:
                gameManager.ChangeState(GameState.MainMenu);
                break;

            case GameState.GameOver:
                gameManager.ChangeState(GameState.MainMenu);
                break;

            case GameState.CustomMaker:
                gameManager.ChangeState(GameState.MainMenu);
                break;

            case GameState.Playing:
                gameManager.ChangeState(GameState.MainMenu);
                break;

            default:
                break;
        }
    }
}
