using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Level[] levels;
    public Button easyLevelButton;
    public Button mediumLevelButton;
    public Button hardLevelButton;
    public Level currentLevel;
    // Start is called before the first frame update
    void Start()
    {
        levels = new Level[] { new Level("easy", 1) , new Level("medium", 1.5f) , new Level("hard", 2) };
        currentLevel = levels[0];
        easyLevelButton.onClick.AddListener(SelLevel0);
        mediumLevelButton.onClick.AddListener(SelLevel1);
        hardLevelButton.onClick.AddListener(SelLevel2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelLevel0()
    {
        currentLevel = levels[0];
        SetLevelChosen();
        SceneManager.LoadScene("My Game");

    }
    public void SelLevel1()
    {
        currentLevel = levels[1];
        SetLevelChosen();
        SceneManager.LoadScene("My Game");

    }
    public void SelLevel2()
    {
        currentLevel = levels[2];
        SetLevelChosen();
        SceneManager.LoadScene("My Game");
    }
    void SetLevelChosen()
    {
        LevelGame.GetInstance().SetLevel(currentLevel);
    }
}
