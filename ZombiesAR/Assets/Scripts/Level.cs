

using UnityEngine;

public class Level
{
    private string name;
    private float difficult;

    public Level(string name, float difficult)
    {
        this.name = name;
        this.difficult = difficult;
    }

    public float GetDifficult()
    {
        return difficult;
    }
    public string GetName()
    {
        return name;
    }
    public void  SetName(string n)
    {
        name = n;
    }
    public void SetDifficult(float d)
    {
        difficult = d;
    }
}

