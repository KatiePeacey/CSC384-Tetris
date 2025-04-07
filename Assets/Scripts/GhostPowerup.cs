using UnityEngine;

public class GhostPowerup : MonoBehaviour
{
    public Ghost ghostSystem;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Piece piece = other.GetComponent<Piece>();
        if (piece != null && ghostSystem != null)
        {
            ghostSystem.Activate(10f);
            Destroy(gameObject); // Remove powerup from board
        }
    }
}
