using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �����
public class PlayerAttack : MonoBehaviour
{
    private Player player;

    // �����������
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

    // �������� ����������� � ������ �����
    private void Update()
    {
        if (timeBtwAttack <= 0 && player.GetComponent<WeaponSwitch>().sword.activeSelf)
        {
            // �� ��
            if (Input.GetMouseButton(0) && Player.ControlerType.PC == player.controlerType)
            {
                player.StartAttack();
            }
            // �� Android
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
    
    // �����
    public void OnAttack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<Enemy>().TakeDamage(damage);
        }
        timeBtwAttack = startTimeBtwAttack;
    }

    // ����������� ������� �����(�� � ����)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
