public class LaserCommand : IPowerupCommand
{
    private GameManagerBehaviour gameManager;

    public LaserCommand(GameManagerBehaviour gm)
    {
        gameManager = gm;
    }

    public void Execute()
    {
        gameManager.UseLaserPowerup();
    }
}

