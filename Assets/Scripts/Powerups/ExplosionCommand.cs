public class ExplosionCommand : IPowerupCommand
{
    private GameManagerBehaviour gameManager;

    public ExplosionCommand(GameManagerBehaviour gm)
    {
        gameManager = gm;
    }

    public void Execute()
    {
        gameManager.UseExplosionPowerup();
    }
}

