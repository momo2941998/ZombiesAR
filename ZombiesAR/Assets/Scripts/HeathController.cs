using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



// this MonoBehaviour attach to HeathBar
public class HeathController : MonoBehaviour
{
    public float maxHeath;
    public float currentHeath;
    public Slider heathFill;
    private Camera myCam;
    public GameObject parentGO;
    private ZombieController zombieController;

    // Start is called before the first frame update
    void Start()
    {
        myCam = Camera.main;
        zombieController = parentGO.GetComponent<ZombieController>();
    }

    public void GetParentInfor()
    {
        maxHeath = zombieController.zombieHpMax;
        currentHeath = zombieController.zombieHp;
    }

    // Update is called once per frame
    void Update()
    {
        PositionChanged();
    }

    public void ChangeHeath(float amount)
    {
        currentHeath += amount;
        if (currentHeath <= 0) currentHeath = 0;
        if (currentHeath >= maxHeath) currentHeath = maxHeath;
        heathFill.value = currentHeath / maxHeath;
    }

    private void PositionChanged()
    {
        transform.LookAt(transform.position + myCam.transform.rotation * Vector3.back,
            myCam.transform.rotation * Vector3.up);
        transform.Rotate(Vector3.up, 180);
    }
}
