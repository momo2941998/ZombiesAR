using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject FemaleZombiePrefab;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("spawnZombie", 2, 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void spawnZombie()
    {
        Vector3 position = gameObject.transform.position;
        GameObject pooledProjectile = ObjectPooler.SharedInstance.GetPooledObject();
            if (pooledProjectile != null)
            {
                pooledProjectile.SetActive(true); // activate it
                pooledProjectile.transform.position = transform.position + new Vector3(Random.Range(-3,3),0,0); // position it at player
            }
        GameObject zombieGO = pooledProjectile;
    }

}
