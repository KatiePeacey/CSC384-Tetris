using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CustomTetrominoBuilder : MonoBehaviour
{
    public Toggle togglePrefab;         // Prefab for each toggle cell
    public Transform gridParent;        // Parent for grid layout (should have Grid Layout Group)
    public Button submitButton;
    public Button resetButton;
    public Text messageText;

    private List<Toggle> gridToggles = new List<Toggle>();
    private List<Vector2Int> customShape = new List<Vector2Int>();

    private int gridSizeX = 5;
    private int gridSizeY = 5;

    void Start()
    {
        InitializeGrid();
        submitButton.onClick.AddListener(OnSubmit);
        resetButton.onClick.AddListener(OnReset);
    }

    private void InitializeGrid()
    {
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Toggle newToggle = Instantiate(togglePrefab, gridParent);
                int xCopy = x;
                int yCopy = y;

                newToggle.onValueChanged.AddListener(isOn => OnToggleChanged(xCopy, yCopy, isOn));
                newToggle.isOn = false;
                gridToggles.Add(newToggle);
            }
        }
    }

    private void OnToggleChanged(int x, int y, bool isOn)
    {
        Vector2Int pos = new Vector2Int(x, y);

        if (isOn && !customShape.Contains(pos))
            customShape.Add(pos);
        else if (!isOn && customShape.Contains(pos))
            customShape.Remove(pos);
    }

    public void OnSubmit()
    {
        if (customShape.Count == 0)
        {
            messageText.text = "Please select some cells!";
            messageText.color = Color.red;
            return;
        }

        // Normalize coordinates to center the shape
        Vector2Int min = new Vector2Int(int.MaxValue, int.MaxValue);
        foreach (Vector2Int cell in customShape)
        {
            min.x = Mathf.Min(min.x, cell.x);
            min.y = Mathf.Min(min.y, cell.y);
        }

        Vector2Int[] normalizedShape = new Vector2Int[customShape.Count];
        for (int i = 0; i < customShape.Count; i++)
        {
            normalizedShape[i] = customShape[i] - min;
        }

        // Save the custom shape
        Data.Cells[Tetromino.Custom] = normalizedShape;

        messageText.text = "Custom piece saved!";
        messageText.color = Color.green;
    }

    public void OnReset()
    {
        customShape.Clear();

        foreach (var toggle in gridToggles)
            toggle.isOn = false;

        messageText.text = "";
    }
    public void ClearCustomTetromino()
    {
        if (Data.Cells.ContainsKey(Tetromino.Custom))
        {
            Data.Cells.Remove(Tetromino.Custom);
        }

        customShape.Clear();
    }
}
