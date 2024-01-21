using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // ��� ����������
    public ControlerType controlerType;
    // ������� ������������
    public Joystick joystick;
    public Joystick joystickGun;
    public float speed;
    public int health = 10;
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
        if (moveInput.x == 0)
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
}
