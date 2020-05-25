

public class LevelGame
{
    private Level level = new Level("", 0);
    private static LevelGame _instance;

    private LevelGame()
    {

    }

    public static LevelGame GetInstance()
    {
        if (_instance == null)
        {
            _instance = new LevelGame();
        }
        return _instance;
    }

    public void SetLevel(Level l)
    {
        level.SetDifficult(l.GetDifficult());
        level.SetName(l.GetName());
    }
    public Level GetLevel()
    {
        return level;
    }
}
