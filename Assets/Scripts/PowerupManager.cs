using UnityEngine;
using UnityEngine.Tilemaps;

public class PowerupManager : MonoBehaviour
{
    public Tilemap powerupTilemap;
    public Tile coinTile; // Assign this in the inspector
    public int coinsCollected = 0;

    public bool CheckAndCollectCoin(Vector3Int cellPosition)
    {
        TileBase tile = powerupTilemap.GetTile(cellPosition);
        if (tile == coinTile)
        {
            powerupTilemap.SetTile(cellPosition, null);
            coinsCollected++;
            Debug.Log("Coin Collected! Total: " + coinsCollected);
            return true;
        }

        return false;
    }
}
