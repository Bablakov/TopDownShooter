using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject floatingDamage;
    
    // �����������
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public int health = 1;
    private float speed;
    public int damage;
    // ����� ��������� ����� ��������� �����
    private float stopTime;
    public float startStopTime;
    public float normalSpeed;
    public GameObject deathEffect;

    private Player player;
    private Animator anim;
    private AddRoom room;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
        room = GetComponentInParent<AddRoom>();
    }

    // ����������� �����
    private void Update()
    {
        // ��� �������� ��������� ������ ����� ��������� �����
        if (stopTime <= 0)
        {
            speed = normalSpeed;
        }
        else
        {
            speed = 0;
            stopTime -= Time.deltaTime;
        }

        // ��� ����������� �����, ����� � ���� �� �������� ��������
        if (health <= 0)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            room.enemies.Remove(gameObject);
        }

        // �������� �����
        if (player.transform.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        // ���������� ����� �� �������
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    // ��������� ����� ������
    public void TakeDamage(int damage)
    {
        stopTime = startStopTime;
        health -= damage;
        Vector2 damagePos = new Vector2(transform.position.x, transform.position.y + 2.75f);
        Instantiate(floatingDamage, damagePos, Quaternion.identity);
        floatingDamage.GetComponentInChildren<FloatingDamage>().damage = damage;
    }

    // ��� ��������� � ���� �������� �������� �������� ��� ����� � ��
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (timeBtwAttack <= 0)
            {
                anim.SetTrigger("attack");
            }
            else
            {
                timeBtwAttack -= Time.deltaTime;
            }
        }
    }

    // ��������� ����� �����
    public void OnEnemyAttack()
    {
        player.ChangeHealth(-damage);
        timeBtwAttack = startTimeBtwAttack;
    }

    // ������������ �� �������� ����
    public void OnEnemyRun()
    {
        anim.SetTrigger("run");
    }
}