using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Перезарядка
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public int health = 1;
    private float speed;
    public int damage;
    // Время остановки после получения урона
    private float stopTime;
    public float startStopTime;
    public float normalSpeed;
    public GameObject deathEffect;

    private Player player;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        // Для контроля остановки игрока после получения урона
        if (stopTime <= 0)
        {
            speed = normalSpeed;
        }
        else
        {
            speed = 0;
            stopTime -= Time.deltaTime;
        }
        
        // Для уничтожения врага, когда у него осталось мало здоровья
        if (health <= 0)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        // Разворот врага
        if(player.transform.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        // Следования врага за игроком
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    // Получения урона врагом
    public void TakeDamage(int damage)
    {
        stopTime = startStopTime;
        health -= damage;
    }

    // При попадании в зону действия триггера проверят кто попал в неё
    public void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(timeBtwAttack <= 0)
            {
                anim.SetTrigger("attack");
            }
            else
            {
                timeBtwAttack -= Time.deltaTime;
            }
        }
    }

    // Нанисение урона врагу
    public void OnEnemyAttack()
    {
        player.ChangeHealth(-damage);
        timeBtwAttack = startTimeBtwAttack;
    }
}
