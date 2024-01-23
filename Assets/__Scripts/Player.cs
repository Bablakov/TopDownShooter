using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Controls")]
    // ��� ����������
    public ControlerType controlerType;
    // ������� ������������
    public Joystick joystick;
    public Joystick joystickGun;
    public float speed;

    [Header("Health")]
    public int health = 10;
    public GameObject potionEffect;

    [Header("Shield")]
    public GameObject shield;
    public GameObject shieldEffect;

    [Header("Death")]
    public GameObject playerEffect;

    public enum ControlerType { PC, Android }

    // Rigidbody ������
    private Rigidbody2D rb;
    // ��� ���������� � ����� ������� �� ��������
    private Vector2 moveInput;
    // �������� �������� ������ � �����-�� �����������
    private Vector2 moveVelocity;
    private Animator anim;

    // �������� �� ������� ������
    private bool facingRight = true;

    void Start()
    {
        // �������� Rigidbody2D ������
        rb = GetComponent<Rigidbody2D>();
        // �������� ��������
        anim = GetComponent<Animator>();

        // ����������� ��������, ����� � ��� ������ ��
        if (controlerType == ControlerType.PC)
            joystick.gameObject.SetActive(false);
    }

    void Update()
    {
        // ���������� ������������ ������ � ����������� �� ���� ����������
        if (controlerType == ControlerType.PC)
        {
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        else if (controlerType == ControlerType.Android)
        {
            moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        }

        // �������� �������� ������
        moveVelocity = moveInput.normalized * speed;

        // �������� ������
        if (moveInput.x == 0 && moveInput.y == 0)
        {
            anim.SetBool("isRunning", false);
        }
        else
        {
            anim.SetBool("isRunning", true);
        }

        // ������� ������
        if (!facingRight && moveInput.x > 0)
        {
            Flip();
        }
        else if (facingRight && moveInput.x < 0)
        { 
            Flip();
        }         

        // ������������ ����� ����� ������ ������
        if (health <= 0)
        {
            Instantiate(playerEffect, rb.position, Quaternion.identity);
            /*StartCoroutine(PauseScene());*/
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }    
    }

    // ����������� ������
    private void FixedUpdate()
    {
        // ���� �������� ������ 
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    // �������� ������
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    // ��������� �������� ������
    public void ChangeHealth(int healtValue)
    {
        health += healtValue;
    }

    // ����� ��� ���������� �������� ����� � ����� ����� �����
    public void StartAttack()
    {
        anim.SetBool("animBool", true);
        anim.SetTrigger("attack");
    }
    
    // ������������ � �������� ����� �����
    public void EndAnimation()
    {
        anim.SetBool("animBool", false);
    }

    // ��� ������� �����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Potion"))
        {
            Instantiate(potionEffect, rb.position, Quaternion.identity);
            ChangeHealth(5);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Shield"))
        {
            Instantiate(shieldEffect, rb.position, Quaternion.identity);
            shield.SetActive(true);
            Destroy(collision.gameObject);
        }
    }

    // ��� �������� ����� ������������ �����
    /*IEnumerator PauseScene()
    {
        yield return new WaitForSecondsRealtime(2000);
    }*/
}
