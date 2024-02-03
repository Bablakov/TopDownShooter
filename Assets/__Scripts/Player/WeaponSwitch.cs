using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public GameObject gun;
    public GameObject sword;

    // Переключение между оружиями
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (gun.activeInHierarchy == true)
            {
                gun.SetActive(false);
                sword.SetActive(true);
            }
            else if (sword.activeInHierarchy == true)
            {
                sword.SetActive(false);
                gun.SetActive(true);
            }
        }
    }
}
