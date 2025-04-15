[System.Serializable]
public enum PowerupType
{
    Explosion,
    Freeze,
    Laser
}

public class PowerupInventory
{
    // Declare the counts for the various powerups
    public int explosionCount = 0;
    public int freezeCount = 0;
    public int laserCount = 0; // This is likely missing in your class

    // Method to add a powerup (example)
    public void AddPowerup(PowerupType type)
    {
        switch (type)
        {
            case PowerupType.Explosion:
                explosionCount++;
                break;
            case PowerupType.Freeze:
                freezeCount++;
                break;
            case PowerupType.Laser:
                laserCount++; // Increment the laser count when the laser powerup is added
                break;
            default:
                break;
        }
    }

    // Method to use a powerup (example)
    public bool UsePowerup(PowerupType type)
    {
        switch (type)
        {
            case PowerupType.Explosion:
                if (explosionCount > 0)
                {
                    explosionCount--;
                    return true;
                }
                break;
            case PowerupType.Freeze:
                if (freezeCount > 0)
                {
                    freezeCount--;
                    return true;
                }
                break;
            case PowerupType.Laser:
                if (laserCount > 0)
                {
                    laserCount--; // Decrease laser count when used
                    return true;
                }
                break;
            default:
                break;
        }

        return false; // If the powerup is not available, return false
    }
}
