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
    public float playerHpMax = 10000;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerController.InitHp(playerHpMax);
        gun = GameObject.FindWithTag("Gun").GetComponent<GunController>();
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
        InvokeRepeating("FlickImage", 0,0.2f);
        
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
        hpText.text = ""+  playerController.playerHp;
    }

    IEnumerator EndGameWait()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene ("GameOver");
    }

    public void UpdatePowerGun(float currentPower)
    {
        powerGunText.text = "" + currentPower;
    }

    void FlickImage ()
    {
        bloodScreen.gameObject.SetActive(true);
        Invoke("DisActiveBloodScreen", 0.1f);
    }
    void DisActiveBloodScreen(){
        bloodScreen.gameObject.SetActive(false);
    }
}
