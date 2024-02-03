using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Скрипт нужен для уничтожение тех стен которые смотрят в обычные зелёные блоки
public class Wall : MonoBehaviour
{
    public GameObject block;
    private bool worked = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Block") && !worked)
        {
            Instantiate(block, transform.GetChild(0).position, Quaternion.identity);
            Instantiate(block, transform.GetChild(1).position, Quaternion.identity);
            worked = true;
            Destroy(gameObject);
        }
    }
}
