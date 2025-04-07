using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class PowerupManager : MonoBehaviour
{
    public Tilemap powerupTilemap;  // Reference to the Powerup Tilemap
    public Tile ghostPowerupTile;   // Reference to the Ghost Powerup tile
    public Board board;            // Reference to the Board
    public float spawnInterval = 10f; // Time interval between spawning powerups
    private float timeSinceLastSpawn = 0f;

    private bool isGhostActive = false;  // To check if ghost mode is active
    private float ghostDuration = 10f;   // Duration of ghost powerup (seconds)

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        // Spawn powerup at random intervals
        if (timeSinceLastSpawn >= spawnInterval)
        {
            timeSinceLastSpawn = 0f;  // Reset timer
            SpawnPowerup();  // Spawn a new powerup
        }
    }

    public void SpawnPowerup()
    {
        // Get a random position within the board boundaries
        int randomX = Random.Range(board.Bounds.xMin, board.Bounds.xMax);
        int randomY = Random.Range(board.Bounds.yMin, board.Bounds.yMax);

        Vector3Int randomPosition = new Vector3Int(randomX, randomY, 0);

        // Check if this position is empty, then spawn the powerup
        if (!board.tilemap.HasTile(randomPosition) && !powerupTilemap.HasTile(randomPosition))
        {
            // Set the powerup tile in the Tilemap at the random position
            powerupTilemap.SetTile(randomPosition, ghostPowerupTile);
        }
        else
        {
            // Try again if the tile is already occupied
            SpawnPowerup();
        }
    }

    // Activate ghost powerup (called when a piece touches the powerup tile)
    public void ActivateGhostPowerup()
    {
        if (!isGhostActive)
        {
            isGhostActive = true;
            Debug.Log("Ghost Powerup Activated!");

            // Activate ghost mode: Show ghost for 10 seconds
            StartCoroutine(GhostEffectDuration());
        }
    }

    // Deactivate ghost mode after 10 seconds
    private IEnumerator GhostEffectDuration()
    {
        // Show ghost mode on active piece
        yield return new WaitForSeconds(ghostDuration);

        // After 10 seconds, deactivate ghost mode
        isGhostActive = false;
        Debug.Log("Ghost Powerup Deactivated!");
    }
}
