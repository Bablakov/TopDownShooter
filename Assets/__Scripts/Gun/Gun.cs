using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Для корректировки вращения оружия
    public float offset;
    // Пуля
    public GunType gunType;
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

    public enum GunType
    {
        Default,
        Enemy
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        // Если режим ПК то джостик стельбы отключаем
        if (player.controlerType == Player.ControlerType.PC && GunType.Default == gunType)
        {
            joystick.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Если пушка игрока
        if (gunType == GunType.Default)
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
        }
        // Если пушка врага
        else if(gunType == GunType.Enemy)
        {
            difference = player.transform.position - transform.position;
            rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        // Стрельба из пушки
        if (timeBtwShots <= 0f)
        {
            if (gunType == GunType.Enemy)
            {
                Shoot();
            }
            // На ПК или стрельба врага
            else if (player.controlerType == Player.ControlerType.PC)
            {
                if (Input.GetMouseButton(0))
                    Shoot();
            }
            // На Android
            else if (player.controlerType == Player.ControlerType.Android)
            {
                if (joystick.Vertical > 0.3f || joystick.Horizontal > 0.3f || joystick.Horizontal < -0.3f || joystick.Vertical < -0.3f) 
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
        Instantiate(bullet, shotPoint.position, shotPoint.rotation);
        timeBtwShots = startTimeBtwShots;
    }
}
