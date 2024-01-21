using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Для корректировки вращения оружия
    public float offset;
    // Пуля
    public GameObject bullet;
    public float startTimeBtwShots;
    // Место спавна пули
    public Transform shotPoint;
    public Joystick joystick;

    // Переменные для отслеживания перезарядки
    private float timeBtwShots;
    private float rotZ;
    private Vector3 difference;
    private Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        // Если режим ПК то джостик стельбы отключаем
        if (player.controlerType == Player.ControlerType.PC)
        {
            joystick.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Слежение пушки за курсором если включен режим ПК
        if (player.controlerType == Player.ControlerType.PC)
        {
        difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        }
        // Слежение пушки за курсором если включен режим Android
        else if (player.controlerType == Player.ControlerType.Android && (Mathf.Abs(joystick.Horizontal) > 0.3f || Mathf.Abs(joystick.Vertical) > 0.3f))
        {
            rotZ = Mathf.Atan2(joystick.Vertical, joystick.Horizontal) * Mathf.Rad2Deg;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        // Стрельба из пушки
        if (timeBtwShots <= 0f)
        {
            // На ПК
            if (Input.GetMouseButtonDown(0) && player.controlerType == Player.ControlerType.PC)
            {
                Shoot();
            }
            // На Android
            else if (player.controlerType == Player.ControlerType.Android)
            {
                if (joystick.Vertical > 0.3f || joystick.Horizontal > 0.3f) 
                {
                    Shoot();
                }
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }

    // Сам выстрел
    public void Shoot()
    {
        Instantiate(bullet, shotPoint.position, transform.rotation);
        timeBtwShots = startTimeBtwShots;
    }
}
