using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public Vector2Int gridSize = new Vector2Int(10, 20);

    public float spawnInterval = 10f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnCoin();
            timer = 0;
        }
    }

    void SpawnCoin()
    {
        Vector2 spawnPosition = new Vector2(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y));
        Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
    }
}
