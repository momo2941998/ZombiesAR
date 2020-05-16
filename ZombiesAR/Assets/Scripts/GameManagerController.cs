using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerController : MonoBehaviour
{

    public GameObject player;
    private GunController gun;
    private PlayerController playerController;
    public GameObject bloodScreen;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI powerGunText;
    public TextMeshProUGUI timeLeftText;
    public TextMeshProUGUI scoreText;
    public float playerHpMax = 10000;
    private float timeLeft;
    [SerializeField]private float playTime = 90;
    public bool isGameOver;
    public bool isGameSetup;
    public bool isGamePlaying;

    // Start is called before the first frame update
    void Start()
    {

    }

    void RunGame()
    {

        ScoreManager.GetInstance().ResetScore();

        IncreaseScore(0);
        isGameOver = false;
        timeLeft = playTime;
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerController.InitHp(playerHpMax);
        gun = GameObject.FindWithTag("Gun").GetComponent<GunController>();
        UpdadtePlayerHP();
        InvokeRepeating("UpdateTimeLeft",0,0.5f);
    }
    // Update is called once per frame
    void Update()
    {
        if (isGameSetup) 
        {
            if (isGamePlaying == false)
            {
                RunGame();
                isGamePlaying = true;
                return;
            }
        } else return;
        if (isGameOver) return;
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            GameOver();
        }
        if (playerController.playerHp <= 0)
        {
            GameOver();
        }
    }

    public void ZombieAttack(float damage)
    {
        InvokeRepeating("FlickImage", 0, 0.2f);

        playerController.DecreaseHp(damage);
        UpdadtePlayerHP();
    }

    public void ZombieFinishAttack()
    {
        // bloodScreen.gameObject.SetActive(false);
        CancelInvoke("FlickImage");
    }

    public void UpdadtePlayerHP()
    {
        hpText.text = "" + playerController.playerHp;
    }

    void GameOver()
    {
        Debug.Log("GameOver");
        isGameOver = true;
        StartCoroutine(EndGameWait());

    }
    IEnumerator EndGameWait()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("My Game Over");
    }

    public void UpdatePowerGun(float currentPower)
    {
        powerGunText.text = "" + currentPower + "/" + gun.powerGunMax;
    }

    void FlickImage()
    {
        bloodScreen.gameObject.SetActive(true);
        Invoke("DisActiveBloodScreen", 0.1f);
    }
    void DisActiveBloodScreen()
    {
        bloodScreen.gameObject.SetActive(false);
    }

    void UpdateTimeLeft()
    {
        TimeSpan t = TimeSpan.FromSeconds(Mathf.RoundToInt(timeLeft));
        string str =  t.ToString(@"mm\:ss");
        timeLeftText.text = str;
    }
    public void IncreaseScore(float value)
    {
        ScoreManager.GetInstance().Increase(value);
        scoreText.text = "SCORE: " + ScoreManager.GetInstance().GetScore() ;
    }

    
}
