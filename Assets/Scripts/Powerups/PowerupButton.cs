using UnityEngine;
using UnityEngine.UI;

public class PowerupButton : MonoBehaviour
{
    private IPowerupCommand command;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void SetCommand(IPowerupCommand newCommand)
    {
        command = newCommand;
    }

    private void OnClick()
    {
        command?.Execute();
    }
}

