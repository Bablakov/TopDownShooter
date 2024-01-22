using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Аттака мечом
public class PlayerAttack : MonoBehaviour
{
    private Player player;

    // Перезарядка
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public LayerMask enemy;
    public float attackRange;
    public int damage;
    public Animator anim;

    private void Start()
    {
        player = GetComponent<Player>();
        if (player.controlerType == Player.ControlerType.PC)
        {
            player.joystickGun.gameObject.SetActive(false);
        }
    }

    // Проверка перезорядки и начало атаки
    private void Update()
    {
        if (timeBtwAttack <= 0 && player.GetComponent<WeaponSwitch>().sword.activeSelf)
        {
            // На ПК
            if (Input.GetMouseButton(0) && Player.ControlerType.PC == player.controlerType)
            {
                player.StartAttack();
            }
            // На Android
            else if (player.controlerType == Player.ControlerType.Android && (player.joystickGun.Vertical > 0.3f || player.joystickGun.Horizontal > 0.3f))
            {
                player.StartAttack();
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }
    
    // Атака
    public void OnAttack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<Enemy>().TakeDamage(damage);
        }
        timeBtwAttack = startTimeBtwAttack;
    }

    // Отображение области атаки(не в игре)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
