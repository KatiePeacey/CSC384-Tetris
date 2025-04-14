using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    public int totalCoins = 0;
    public GameObject coinPrefab;
    public float spawnInterval = 10f;

    private float timer;
    private List<CoinPowerup> activeCoins = new List<CoinPowerup>();
    public Board board;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        board = FindFirstObjectByType<Board>();

        if (board == null)
        {
            Debug.LogError("CoinManager: Board not found in the scene!");
        }
    }

    private void Update()
    {
        if (board == null) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnCoin();
            timer = 0f;
        }
    }

    public void AddCoins(int amount)
    {
        totalCoins += amount;
        Debug.Log($"Coins: {totalCoins}");
    }

    public void SpawnCoin()
    {
        if (board == null) return;

        // Remove existing coins
        for (int i = 0; i < activeCoins.Count; i++)
        {
            Destroy(activeCoins[i].gameObject);
        }
        activeCoins.Clear();

        Vector3Int position = GetValidRandomPosition(board);
        Vector3 worldPosition = board.tilemap.CellToWorld(position) + new Vector3(0.5f, 0.5f); // center of tile

        GameObject coinGO = Instantiate(coinPrefab, worldPosition, Quaternion.identity);
        CoinPowerup coin = coinGO.GetComponent<CoinPowerup>();
        coin.tilePosition = position;

        activeCoins.Add(coin);
    }

    private Vector3Int GetValidRandomPosition(Board board)
    {
        Vector3Int position;
        RectInt bounds = board.Bounds;

        do
        {
            position = new Vector3Int(
                Random.Range(bounds.xMin, bounds.xMax),
                Random.Range(bounds.yMin, bounds.yMax),
                0
            );
        } while (board.tilemap.HasTile(position));

        return position;
    }

    public void CheckCoinCollection(Vector3Int[] pieceTiles)
    {
        for (int i = activeCoins.Count - 1; i >= 0; i--)
        {
            CoinPowerup coin = activeCoins[i];
            foreach (var tile in pieceTiles)
            {
                if (tile == coin.tilePosition)
                {
                    CollectCoin(coin);
                    break;
                }
            }
        }
    }

    private void CollectCoin(CoinPowerup coin)
    {
        AddCoins(1);
        activeCoins.Remove(coin);
        Destroy(coin.gameObject);
    }
}
