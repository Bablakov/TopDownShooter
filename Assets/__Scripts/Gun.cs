using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Для корректировки вращения оружия
    public float offset;
    // Пуля
    public GameObject bullet;
    // Место спавна пули
    public Transform shotPoint;

    // Переменные для отслеживания перезарядки
    private float timeBtwShots;
    public float startTimeBtwShots;

    void Update()
    {
        // Слежение пушки за курсором
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        // Стрельба из пушки
        if (timeBtwShots <= 0f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(bullet, shotPoint.position, transform.rotation);
                timeBtwShots = startTimeBtwShots;
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }
}
