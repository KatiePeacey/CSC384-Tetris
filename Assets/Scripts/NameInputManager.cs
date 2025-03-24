using UnityEngine;
using UnityEngine.UI;

public class NameInputManager : MonoBehaviour
{
    public InputField PlayerNameInput;  // Drag the InputField here in the Inspector
    public Button submitButton;              // Drag the Submit Button here in the Inspector
    public GameManagerBehaviour gameManager; // Reference to GameManagerBehaviour

    private void Start()
    {
        // Ensure the button triggers the OnSubmit function when clicked
        submitButton.onClick.AddListener(OnSubmit);
    }

    // Called when the submit button is clicked
    private void OnSubmit()
    {
        // Get the player name from the InputField
        string playerName = PlayerNameInput.text;

        // Make sure the name isn't empty
        if (!string.IsNullOrEmpty(playerName))
        {
            // Add the player's name, score, and level to the scoreboard
            ScoreboardManager.Instance.AddNewEntry(playerName, gameManager.GetScore(), gameManager.GetLevel());

            // Optionally, you can show a message that the name has been submitted
            Debug.Log("Player name submitted: " + playerName);
        }
        else
        {
            // Show a message that the player name is empty
            Debug.Log("Please enter a valid name.");
        }
    }
}

