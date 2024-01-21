using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public float distance;
    public int damage;
    public LayerMask whatIsSolid;

    public GameObject destroyEffect;

    // ����������� ���� ����� ��������� � ����� �����
    public void Start()
    {
        Invoke("DestroyBullet", lifeTime);
    }

    // ���������� �������� ���� � ��������� ����� �����
    private void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, whatIsSolid);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                hitInfo.collider.GetComponent<Enemy>().TakeDamage(damage);
            }
            DestroyBullet();
        }
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    // ����������� ����
    public void DestroyBullet()
    {
        /*Instantiate(destroyEffect, transform.position, Quaternion.identity);*/
        Destroy(gameObject);
    }
}
