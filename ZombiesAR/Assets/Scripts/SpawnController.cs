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

        GameObject zombieGO = Instantiate(FemaleZombiePrefab, new Vector3(position.x, position.y, position.z), Quaternion.Euler(0, Random.Range(0f, 360f), 0));
    }

}
