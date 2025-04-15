using UnityEngine;
using UnityEngine.InputSystem;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }
    public int rotationIndex { get; private set; }
    public float stepDelay = 1f;
    public float lockDelay = 0.5f;
    private float stepTime;
    private float lockTime;
    private bool isSlowTimeActive = false;
    private float slowTimeEnd = 0f;
    private float originalStepDelay;
    public GameObject FreezeEffect;
    private GameObject activeSlowEffect;


    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        this.rotationIndex = 0;
        this.stepTime = Time.time + this.stepDelay;
        this.lockTime = 0f;

        if (this.cells == null) {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < data.cells.Length; i++) {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }

    private void Update()
    {
        this.board.Clear(this);
        this.lockTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Q)) {
            Rotate(-1);
        } else if (Input.GetKeyDown(KeyCode.E)) {
            Rotate(1);
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)) {
            Move(Vector2Int.left);
        } else if (Input.GetKeyDown(KeyCode.D)|| Input.GetKeyUp(KeyCode.RightArrow)) {
            Move(Vector2Int.right);
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) {
            Move(Vector2Int.down);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            HardDrop();
        }
        
        if (Input.GetKeyDown(KeyCode.L)) {
            UseLineBlaster();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ActivateSlowTime(7f, 2f);
        }

        if (isSlowTimeActive && Time.time >= slowTimeEnd)
        {
            ResetSlowTime();
            if (activeSlowEffect != null)
            {
                Destroy(activeSlowEffect);
            }
        }

 
        if (Time.time >= this.stepTime) {
            Step();
        }
        if (isSlowTimeActive && activeSlowEffect != null)
        {
            // Convert piece's tile position to world space
            Vector3 worldPos = board.tilemap.CellToWorld(this.position);
            
            // Offset if needed (to center)
            worldPos += new Vector3(0.5f, 0.5f, 0f);

            activeSlowEffect.transform.position = worldPos;
        }


        this.board.Set(this);
    }
    private void ResetSlowTime()
    {
        // Reset the game speed after slow time ends
        this.stepDelay = originalStepDelay;
        isSlowTimeActive = false;

        // Reset the music pitch
        AudioManager.Instance?.ResetMusicPitch();
    }

    private void Step()
    {
        this.stepTime = Time.time + this.stepDelay;
        Move(Vector2Int.down);

        if (this.lockTime >= this.lockDelay) {
            Lock();
        }
    }
    public void SetSpeed(float newStepDelay)
    {
        this.stepDelay = newStepDelay;
    }
    private void HardDrop()
    {
        while (Move(Vector2Int.down)) {
            continue;
        }
        Lock();
    }

    private void Lock()
    {
        this.board.Set(this);
        this.board.ClearLines();
        this.board.SpawnPiece();
    }
    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = this.board.IsValidPosition(this, newPosition);

        if(valid) {
            this.position = newPosition;
            this.lockTime = 0f;
        }

        return valid;
    }

    private void Rotate(int direction)
    {
        int originalRotation = this.rotationIndex;
        this.rotationIndex = Wrap(this.rotationIndex + direction, 0, 4);

        ApplyRotationMatrix(direction);
        if (!TestWallKicks(this.rotationIndex, direction))
        {
            this.rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }
    }

    private void ApplyRotationMatrix(int direction)
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3 cell = this.cells[i];

            int x, y;

            switch (this.data.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }

            this.cells[i] = new Vector3Int(x, y, 0);
        }
    }
    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < this.data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = this.data.wallKicks[wallKickIndex, i];

            if (Move(translation)) {
                return true;
            }
    
        }
        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0) {
            wallKickIndex--;

        }

        return Wrap(wallKickIndex, 0, this.data.wallKicks.GetLength(0));
    }
    private int Wrap(int input, int min, int max)
    {
        if (input < min) {
            return max - (min - input) % (max - min);
        } else {
            return min + (input - min) % (max - min);
        }
    }

    private void UseLineBlaster()
    {
        int y = this.position.y; // Get current Y row of the piece

        if (board.IsRowWithinBounds(y)) {
            board.ClearSingleLine(y);
        }
    }
    public void ActivateSlowTime(float duration, float slowSpeed)
    {
        if (FreezeEffect != null)
        {
            activeSlowEffect = Instantiate(FreezeEffect, transform);
            activeSlowEffect.transform.localPosition = Vector3.zero;

        }

        // Slow down the music pitch
        AudioManager.Instance?.SetMusicPitch(0.5f);  // Adjust music pitch for effect

        // Adjust game speed
        if (!isSlowTimeActive)
        {
            originalStepDelay = this.stepDelay;  // Save original speed before slow time
        }

        this.stepDelay = slowSpeed;  // Adjust step delay to slow down the game
        slowTimeEnd = Time.time + duration;  // Calculate when slow time should end
        isSlowTimeActive = true;
    }



}