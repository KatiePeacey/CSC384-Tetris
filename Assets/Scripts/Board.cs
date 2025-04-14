using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap;
    public Piece activePiece { get; private set; }
    public TetrominoData[] tetrominoes;
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public GameManagerBehaviour gameManager;
    public GameObject LineBlastEffect;
    public AudioSource sfxSource;
    public AudioClip lineBlastSFX;


    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        }
    }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();

        for (int i = 0; i < this.tetrominoes.Length; i++){
            this.tetrominoes[i].Initialize();
        }
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0, this.tetrominoes.Length);
        TetrominoData data = this.tetrominoes[random];

        this.activePiece.Initialize(this, this.spawnPosition, data);

        if (IsValidPosition(this.activePiece, this.spawnPosition)) {
            Set(this.activePiece);
        } else {
            gameManager.GameOver();
        }
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = this.Bounds;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            if (!bounds.Contains((Vector2Int)tilePosition)) {
                return false;
            }

            if (this.tilemap.HasTile(tilePosition)){
                return false;
            }
        }
        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;

        while (row < bounds.yMax)
        {
            if (IsLineFull(row)) {
                LineClear(row);
                gameManager.SetScore(gameManager.score + 50);
             } else {
                row++;
             }
        }
        
    }

    private bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if (!this.tilemap.HasTile(position)) {
                return false;
            }
        }
        return true;
    }

    private void LineClear(int row)
    {
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(position, null);
        }
        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = this.tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(position, above);
            }
            row++;
        }
    }
    public bool IsTileOccupied(Vector3Int position)
    {
        return tilemap.HasTile(position);
    }

    public bool IsRowWithinBounds(int y)
    {
        return y >= Bounds.yMin && y < Bounds.yMax;
    }

    public void ClearSingleLine(int row)
    {
        RectInt bounds = this.Bounds;

        Vector3 effectPosition = new Vector3(bounds.xMin + (boardSize.x / 2), row, 0);

        effectPosition.y += 0.5f;

        GameObject effect = Instantiate(LineBlastEffect, effectPosition, Quaternion.identity);
        Destroy(effect, 2f); 
        if (sfxSource && lineBlastSFX)
        {
            sfxSource.PlayOneShot(lineBlastSFX);
        }


        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            tilemap.SetTile(position, null);
        }

        for (int y = row + 1; y < bounds.yMax; y++)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int upper = new Vector3Int(col, y, 0);
                TileBase aboveTile = tilemap.GetTile(upper);

                Vector3Int lower = new Vector3Int(col, y - 1, 0);
                tilemap.SetTile(lower, aboveTile);
            }
        }

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int top = new Vector3Int(col, bounds.yMax - 1, 0);
            tilemap.SetTile(top, null);
        }

        gameManager.SetScore(gameManager.score + 100);
    }



}
