using UnityEngine.Tilemaps;
using UnityEngine;
public enum Tetromino
{
    I,
    O,
    T,
    J,
    L,
    S,
    Z,
    Custom
}

[System.Serializable]
public struct TetrominoData
{
    public Tetromino tetromino;
    public Tile tile;
    public Vector2Int[] cells {get; private set;}
    public Vector2Int[,] wallKicks {get; private set;}

    public void Initialize()
    {
        this.cells = Data.Cells[this.tetromino];
        this.wallKicks = Data.WallKicks[this.tetromino];
    }
    
    public void SpawnTetromino(Tilemap tilemap, Tetromino tetromino, Vector2Int spawnPosition)
    {
        Vector2Int[] cells;
        if (tetromino == Tetromino.Custom)
        {
            cells = Data.Cells[Tetromino.Custom];
        }
        else
        {
            cells = Data.Cells[tetromino];
        }

        foreach (Vector2Int cell in cells)
        {
            Vector3Int position = new Vector3Int(spawnPosition.x + cell.x, spawnPosition.y + cell.y, 0);
            tilemap.SetTile(position, tile);
        }
    }
    public TetrominoData(TetrominoData other)
    {
        this.tetromino = other.tetromino;
        this.tile = other.tile;
        this.cells = (Vector2Int[])other.cells.Clone();
        this.wallKicks = (Vector2Int[,])other.wallKicks.Clone();
    }
}
