﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    bool hasStartSpawn = false;
    GameManagerController gameManagerController;
    [SerializeField] private float timeDelay = 3;
    public GameObject FemaleZombiePrefab;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerController = GameObject.FindWithTag("GameManager").GetComponent<GameManagerController>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!hasStartSpawn)
        {
            if (gameManagerController.isGamePlaying)
            {
                InvokeRepeating("spawnZombie", 0, timeDelay);
                hasStartSpawn = true;
            }
        }
    }

    private void spawnZombie()
    {
        Vector3 position = gameObject.transform.position;
        GameObject pooledProjectile = ObjectPooler.SharedInstance.GetPooledObject();
            if (pooledProjectile != null)
            {
                pooledProjectile.SetActive(true); // activate it
                pooledProjectile.transform.position = transform.position ; // position it at player
                pooledProjectile.transform.rotation = transform.rotation;
            }
        GameObject zombieGO = pooledProjectile;
    }

}
