

public class ScoreManager 
{
    private float score = 0;
    private static ScoreManager _instance;

    private ScoreManager()
    {

    }

    public static ScoreManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ScoreManager();
        }
        return _instance;
    }
    public void Increase(float point)
    {
        score += point;
    }

    public void Decrease(float point)
    {
        Increase(-point);
    }

    public float GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        score = 0;
    }

}
