﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    public float damageGun = 10;
    public float range = 100f;
    public Camera fpsCam;
    public ParticleSystem muzzleFlashPartical;
    public GameObject impact;
    public float hitForce = 10;
    public float powerGunMax = 50;
    public float powerGun;
    public float refillTime = 0.1f;
    GameManagerController gameManagerController;

    public Button fire;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerController = GameObject.FindWithTag("GameManager").GetComponent<GameManagerController>();
        powerGun = powerGunMax;
        fpsCam = Camera.main;
        fire.onClick.AddListener(Shoot);
        muzzleFlashPartical.gameObject.SetActive(true);
        impact.gameObject.SetActive(true);
        InvokeRepeating("RefillPower", 0.5f , 0.5f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Shoot(){
        powerGun -= 1;
        gameManagerController.UpdatePowerGun(powerGun);
        StopAllCoroutines();
        muzzleFlashPartical.Play();
        StartCoroutine(GapShoot());
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position,fpsCam.transform.forward, out hit, range))
        {
            ZombieController zombieController = hit.transform.GetComponent<ZombieController>();
            if(zombieController != null)
            {
                zombieController.TakeDamage(damageGun);
            }
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(hit.normal * hitForce);
            }
            GameObject impactGO = Instantiate(impact,hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2);

        }
        
    }

    IEnumerator GapShoot()
    {
        yield return new WaitForSeconds(0.2f);
        muzzleFlashPartical.Stop();
    }
    
    void RefillPower()
    {
        if (powerGun +1 <= powerGunMax)
        {
            powerGun += 1;
        }
        gameManagerController.UpdatePowerGun(powerGun);
    }

}
