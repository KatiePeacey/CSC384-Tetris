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
    public int laserCount = 0;
    private const int MAX_POWERUP_COUNT = 10;


    // Method to add a powerup (example)
    public void AddPowerup(PowerupType type)
    {
        switch (type)
        {
            case PowerupType.Explosion:
                if (explosionCount < MAX_POWERUP_COUNT)
                    explosionCount++;
                break;
            case PowerupType.Freeze:
                if (freezeCount < MAX_POWERUP_COUNT)
                    freezeCount++;
                break;
            case PowerupType.Laser:
                if (laserCount < MAX_POWERUP_COUNT)
                    laserCount++;
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
