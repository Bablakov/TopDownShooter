using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour
{
    public static bool isTrigger = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTrigger)
        {
            isTrigger = true; 
            SceneTransition.SwitchToScene("MenuScene");
        }
    }
}
