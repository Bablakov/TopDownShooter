using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    public float cooldown;

    [HideInInspector]public bool isCooldown;

    private Image shieldImage;
    private Player player;

    public bool isResetTimer => shieldImage != null;
    void Start()
    {
        shieldImage = GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        isCooldown = true;
    }

    void Update()
    {
        // Если отображение щита включено, то выполняем
        if (isCooldown)
        {
            // Уменьшаем его общее время отображения
            shieldImage.fillAmount -= 1 / cooldown * Time.deltaTime;
            // Если время вышло, то выключаем отображение щита на персонаже, в UI
            // И востанавливаем shieldImage.fillAmount
            if (shieldImage.fillAmount <= 0)
            {
                shieldImage.fillAmount = 1;
                isCooldown = false;
                player.shield.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }

    public void ResetTimer()
    {
        shieldImage.fillAmount = 1;
    }

    public void ReduceTime(int damage)
    {
        shieldImage.fillAmount += damage / 5f;
    }
}
