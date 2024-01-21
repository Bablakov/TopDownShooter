using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Тип управления
    public ControlerType controlerType;
    // Джостик передвежения
    public Joystick joystick;
    public Joystick joystickGun;
    public float speed;
    public int health = 10;
    public enum ControlerType { PC, Android }

    // Rigidbody игрока
    private Rigidbody2D rb;
    // Для считывания в какую стороны мы движемся
    private Vector2 moveInput;
    // Итоговая скорость игрока в каком-то направлении
    private Vector2 moveVelocity;
    private Animator anim;

    // Отвечает за поворот игрока
    private bool facingRight = true;

    void Start()
    {
        // Получаем Rigidbody2D игрока
        rb = GetComponent<Rigidbody2D>();
        // Получаем аниматор
        anim = GetComponent<Animator>();

        // Отключались джостика, когда у нас выбран ПК
        if (controlerType == ControlerType.PC)
            joystick.gameObject.SetActive(false);
    }

    void Update()
    {
        // Считывание передвежения игрока в зависимости от типа управления
        if (controlerType == ControlerType.PC)
        {
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        else if (controlerType == ControlerType.Android)
        {
            moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        }

        // Конечная скорость игрока
        moveVelocity = moveInput.normalized * speed;

        // Анимация игрока
        if (moveInput.x == 0)
        {
            anim.SetBool("isRunning", false);
        }
        else
        {
            anim.SetBool("isRunning", true);
        }

        // Поворот игрока
        if (!facingRight && moveInput.x > 0)
        {
            Flip();
        }
        else if (facingRight && moveInput.x < 0)
        { 
            Flip();
        }         

        // Перезагрузка сцены после смерти игрока
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }    
    }

    // Перемещение игрока
    private void FixedUpdate()
    {
        // Само движение игрока 
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    // Разворот игрока
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    // Изменение здоровья игроку
    public void ChangeHealth(int healtValue)
    {
        health += healtValue;
    }
}
