﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerController : MonoBehaviour
{
    public GameObject player;
    private PlayerController playerController;
    public GameObject bloodScreen;
    public TextMeshProUGUI hpText;

    public float playerHpMax = 100;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerController.InitHp(playerHpMax);
        UpdadtePlayerHP();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.playerHp <= 0) 
		{
            Debug.Log("GameOver");
            StartCoroutine(EndGameWait());
			
		}
    }

    public void ZombieAttack(float damage)
    {
        bloodScreen.gameObject.SetActive(true);
        playerController.DecreaseHp(damage);
        UpdadtePlayerHP();
    }

    public void ZombieFinishAttack()
    {
        bloodScreen.gameObject.SetActive(false);
    }

    public void UpdadtePlayerHP()
    {
        hpText.text = "HP: " + playerController.playerHp;
    }
    IEnumerator EndGameWait()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene ("GameOver");
    }
}
