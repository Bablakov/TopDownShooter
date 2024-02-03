using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // ��� ������������� �������� ������
    public float offset;
    // ����
    public GunType gunType;
    public GameObject bullet;
    public float startTimeBtwShots;
    // ����� ������ ����
    public Transform shotPoint;
    public Joystick joystick;

    // ���������� ��� ������������ �����������
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
        // ���� ����� �� �� ������� ������� ���������
        if (player.controlerType == Player.ControlerType.PC && GunType.Default == gunType)
        {
            joystick.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // ���� ����� ������
        if (gunType == GunType.Default)
        {
            // �������� ����� �� �������� ���� ������� ����� ��
            if (player.controlerType == Player.ControlerType.PC)
            {
                difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            }
            // �������� ����� �� �������� ���� ������� ����� Android
            else if (player.controlerType == Player.ControlerType.Android && (Mathf.Abs(joystick.Horizontal) > 0.3f || Mathf.Abs(joystick.Vertical) > 0.3f))
            {
                rotZ = Mathf.Atan2(joystick.Vertical, joystick.Horizontal) * Mathf.Rad2Deg;
            }
        }
        // ���� ����� �����
        else if(gunType == GunType.Enemy)
        {
            difference = player.transform.position - transform.position;
            rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        // �������� �� �����
        if (timeBtwShots <= 0f)
        {
            if (gunType == GunType.Enemy)
            {
                Shoot();
            }
            // �� �� ��� �������� �����
            else if (player.controlerType == Player.ControlerType.PC)
            {
                if (Input.GetMouseButton(0))
                    Shoot();
            }
            // �� Android
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

    // ��� �������
    public void Shoot()
    {
        Instantiate(bullet, shotPoint.position, shotPoint.rotation);
        timeBtwShots = startTimeBtwShots;
    }
}
