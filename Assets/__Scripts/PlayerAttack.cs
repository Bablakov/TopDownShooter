using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Аттака мечом
public class PlayerAttack : MonoBehaviour
{
    // Перезарядка
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public LayerMask enemy;
    public float attackRange;
    public int damage;
    public Animator anim;

    private void Update()
    {
        if (timeBtwAttack <= 0)
        {
            if (Input.GetMouseButton(0))
            {
                anim.SetTrigger("attack");
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    public void OnAttack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<Enemy>().TakeDamage(damage);
        }
        timeBtwAttack = startTimeBtwAttack;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
