using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Объявление полей
    [Header("Controls")]
    // Тип управления
    public ControlerType controlerType;
    // Джостик передвежения
    public Joystick joystick;
    // Джостик оружия
    public Joystick joystickGun;
    public float speed;

    [Header("Health")]
    public int health = 10;
    public GameObject potionEffect;
    public Text healthDisplay;

    [Header("Shield")]
    public GameObject shield;
    public GameObject shieldEffect;
    public Shield shieldTimer;

    [Header("Weapons")]
    public List<GameObject> unlockedWeapons;
    public GameObject[] allWeapons;
    public Image weaponIcon;

    [Header("Key")]
    public GameObject keyIcon;
    public GameObject wallEffect;

    [Header("Death")]
    public GameObject playerEffect;

    public enum ControlerType { PC, Android }

    private Rigidbody2D rb;
    // Для считывания в какую стороны мы движемся
    private Vector2 moveInput;
    // Итоговая скорость игрока в каком-то направлении
    private Vector2 moveVelocity;
    private Animator anim;

    // Отвечает за поворот игрока
    private bool facingRight = true;
    private bool keyButtonPushed;
    
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

        // Переключение оружия
        if(Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon();
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
    public void ChangeHealth(int healthValue)
    {
        if (!shield.activeInHierarchy || shield.activeInHierarchy && healthValue > 0)
        {
            health += healthValue;
            healthDisplay.text = $"HP: {health}";
        }
        else if (shield.activeInHierarchy && healthValue < 0)
        {
            shieldTimer.ReduceTime(healthValue);
        }
    }

    // Подбор бонусов, оружия, ключа
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
            // Если щит не был подобран, то выполняем его активацию
            if (!shield.activeInHierarchy)
            {
                Instantiate(shieldEffect, rb.position, Quaternion.identity);
                shieldTimer.gameObject.SetActive(true);
                shieldTimer.isCooldown = true;
                shield.SetActive(true);
                Destroy(collision.gameObject);
            }
            // Иначе мы обнуляем таймер
            else if (shield.activeInHierarchy && shieldTimer.isResetTimer)
            {
                Instantiate(shieldEffect, rb.position, Quaternion.identity);
                shieldTimer.ResetTimer();
                Destroy(collision.gameObject);
            }
        }
        else if (collision.CompareTag("Weapon"))
        {
            // Проверяем есть ли такое оружие у нас во всех доступных
            for (int i = 0; i < allWeapons.Length; i++)
            {
                // Если да, то закидываем в разблокированные
                if (collision.name == allWeapons[i].name)
                    unlockedWeapons.Add(allWeapons[i]);
            }
            // Сразу после подбора оружия берём в руки
            SwitchWeapon();
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Key"))
        {
            keyIcon.SetActive(true);
            Destroy(collision.gameObject);
        }
    }

    // Для смены состояния кнопки
    public void OnKeyButtonDown()
    {
        keyButtonPushed = !keyButtonPushed;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Door") && keyButtonPushed && keyIcon.activeInHierarchy)
        {
            Instantiate(wallEffect, collision.transform.position, Quaternion.identity);
            keyIcon.SetActive(false);
            collision.gameObject.SetActive(false);
            keyButtonPushed = false;
        }
    }

    // Переключение между пушками
    public void SwitchWeapon()
    {
        for (int i = 0; i < unlockedWeapons.Count; i++)
        {
            if (unlockedWeapons[i].activeInHierarchy)
            {
                unlockedWeapons[i].SetActive(false);
                if (i != 0)
                {
                    unlockedWeapons[i - 1].SetActive(true);
                    weaponIcon.sprite = unlockedWeapons[i - 1].GetComponent<SpriteRenderer>().sprite;
                }
                else
                {
                    unlockedWeapons[unlockedWeapons.Count - 1].SetActive(true);
                    weaponIcon.sprite = unlockedWeapons[unlockedWeapons.Count - 1].GetComponent<SpriteRenderer>().sprite;
                }
                weaponIcon.SetNativeSize();
                break;
            }
        }
    }

    #region Для правильной работы анимаций
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
    #endregion
}
