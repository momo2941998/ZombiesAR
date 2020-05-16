using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePass : MonoBehaviour
{
    static float scorePass;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScore(float s)
    {
        scorePass = s;
    }

    public float GetScore()
    {
        return scorePass;
    }
}
