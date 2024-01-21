using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Перезарядка
    private float timeBtwAttack;
    private float startTimeBtwAttack;

    public int health = 1;
    public float speed;
    public int damage;
    // Время остановки после получения урона
    private float stopTime;
    public float startStopTime;
    public float normalSpeed;

    private Player player;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
        normalSpeed = speed;
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
        
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        stopTime = startStopTime;
        health -= damage;
    }

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

    public void OnEnemyAttack()
    {
        player.health -= damage;
        timeBtwAttack = startTimeBtwAttack;
    }
}
