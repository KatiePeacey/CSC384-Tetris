using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public Ghost ghost;
    
    public void TriggerGhostPowerup()
    {
        ghost.Activate();
    }
}
