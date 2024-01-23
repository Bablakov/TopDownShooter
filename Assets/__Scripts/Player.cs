using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Controls")]
    // Тип управления
    public ControlerType controlerType;
    // Джостик передвежения
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
        if (moveInput.x == 0 && moveInput.y == 0)
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
            Instantiate(playerEffect, rb.position, Quaternion.identity);
            /*StartCoroutine(PauseScene());*/
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

    // Нужно для правильной анимации атаки и самой атаки мечом
    public void StartAttack()
    {
        anim.SetBool("animBool", true);
        anim.SetTrigger("attack");
    }
    
    // Используется в анимации атаки мечом
    public void EndAnimation()
    {
        anim.SetBool("animBool", false);
    }

    // Для бодбора зелья
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

    // Для ожидания перед перезапуском сцены
    /*IEnumerator PauseScene()
    {
        yield return new WaitForSecondsRealtime(2000);
    }*/
}
