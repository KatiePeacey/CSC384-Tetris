public class FreezeCommand : IPowerupCommand
{
    private GameManagerBehaviour gameManager;

    public FreezeCommand(GameManagerBehaviour gm)
    {
        gameManager = gm;
    }

    public void Execute()
    {
        gameManager.UseFreezePowerup();
    }
}

