using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitching : MonoBehaviour
{
    [SerializeField] private int selectedWeapon = 0;
    public Button changeWeapon;
    public GameObject[] guns;
    public Button fire;
    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
        fire.onClick.AddListener(Shoot);
        changeWeapon.onClick.AddListener(ChangeWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (GameObject gun in guns)
        {
            if (i == selectedWeapon)
            {
                gun.gameObject.SetActive(true);
            } else 
            {        
                gun.GetComponent<GunController>().StopAllCoroutines();
                gun.gameObject.SetActive(false);
            }
            i++;

        }
    }
    void ChangeWeapon()
    {
        if (selectedWeapon >= transform.childCount -1) selectedWeapon = 0;
        else selectedWeapon ++ ;
        SelectWeapon();
    }

    public void Shoot()
    {
        guns[selectedWeapon].GetComponent<GunController>().Shoot();
    }    



}
