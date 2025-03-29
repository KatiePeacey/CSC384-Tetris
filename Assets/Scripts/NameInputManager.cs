using UnityEngine;
using UnityEngine.UI;

public class NameInputManager : MonoBehaviour
{
    public InputField PlayerNameInput;
    public Button submitButton;
    public GameManagerBehaviour gameManager;
    public Text errorMessage;

    private string playerName;

    private void Start()
    {
        submitButton.onClick.AddListener(OnSubmit);
    }
    private void OnSubmit()
    {
        playerName = PlayerNameInput.text;

        if (!string.IsNullOrEmpty(playerName))
        {
            Debug.Log("Player name submitted: " + playerName);
            gameManager.SetPlayerName(playerName);
            gameManager.NewGame();
        }
        else
        {
            errorMessage.text = "Please enter a name!";
            errorMessage.color = Color.red;
        }
    }
}
