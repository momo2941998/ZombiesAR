using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private float timeDelay = 3;
    public GameObject FemaleZombiePrefab;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("spawnZombie", 0, timeDelay);
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
                pooledProjectile.transform.position = transform.position ; // position it at player
                pooledProjectile.transform.rotation = transform.rotation;
            }
        GameObject zombieGO = pooledProjectile;
    }

}
