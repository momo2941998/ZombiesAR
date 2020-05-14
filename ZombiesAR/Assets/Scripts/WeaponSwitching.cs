using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitching : MonoBehaviour
{
    [SerializeField] private int selectedWeapon = 0;
    public Button changeWeapon;

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
        changeWeapon.onClick.AddListener(ChangeWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            } else 
            {        
                weapon.GetComponent<GunController>().StopAllCoroutines();
                weapon.gameObject.SetActive(false);
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
}
