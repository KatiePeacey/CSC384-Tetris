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
        if (gameManager.gameOver == true)
        {
            Leaderboard.SetActive(false);
            gameOverMenu.SetActive(true);
            game.SetActive(false);
            MainMenu.SetActive(false);
        }
        else if (gameManager.mainMenu == true)
        {
            Leaderboard.SetActive(false);
            gameOverMenu.SetActive(false);
            game.SetActive(false);
            MainMenu.SetActive(true);
        }
    }
}
