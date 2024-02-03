using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region ���������� �����
    [Header("Controls")]
    // ��� ����������
    public ControlerType controlerType;
    // ������� ������������
    public Joystick joystick;
    // ������� ������
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
    // ��� ���������� � ����� ������� �� ��������
    private Vector2 moveInput;
    // �������� �������� ������ � �����-�� �����������
    private Vector2 moveVelocity;
    private Animator anim;

    // �������� �� ������� ������
    private bool facingRight = true;
    private bool keyButtonPushed;
    
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

        // ������������ ������
        if(Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon();
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

    // ������ �������, ������, �����
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
            // ���� ��� �� ��� ��������, �� ��������� ��� ���������
            if (!shield.activeInHierarchy)
            {
                Instantiate(shieldEffect, rb.position, Quaternion.identity);
                shieldTimer.gameObject.SetActive(true);
                shieldTimer.isCooldown = true;
                shield.SetActive(true);
                Destroy(collision.gameObject);
            }
            // ����� �� �������� ������
            else if (shield.activeInHierarchy && shieldTimer.isResetTimer)
            {
                Instantiate(shieldEffect, rb.position, Quaternion.identity);
                shieldTimer.ResetTimer();
                Destroy(collision.gameObject);
            }
        }
        else if (collision.CompareTag("Weapon"))
        {
            // ��������� ���� �� ����� ������ � ��� �� ���� ���������
            for (int i = 0; i < allWeapons.Length; i++)
            {
                // ���� ��, �� ���������� � ����������������
                if (collision.name == allWeapons[i].name)
                    unlockedWeapons.Add(allWeapons[i]);
            }
            // ����� ����� ������� ������ ���� � ����
            SwitchWeapon();
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Key"))
        {
            keyIcon.SetActive(true);
            Destroy(collision.gameObject);
        }
    }

    // ��� ����� ��������� ������
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

    // ������������ ����� �������
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

    #region ��� ���������� ������ ��������
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
    #endregion
}
