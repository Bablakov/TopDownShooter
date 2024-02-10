using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScene : MonoBehaviour
{
    public GameObject PausePanel;
    private static GameObject pauseButton;

    public void PauseButtonPressed()
    {
        PausePanel.SetActive(true);
        pauseButton = this.gameObject;
        this.gameObject.SetActive(false);
        Time.timeScale = 0f;
    }

    public void ContinueButtonPressed()
    {
        PausePanel.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f;
    }

    public void RestartButtonPressed()
    {
        PausePanel.SetActive(false);
        pauseButton.SetActive(false);
        SceneTransition.SwitchToScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }    

    public void ExitButtonPressed()
    {
        PausePanel.SetActive(false);
        pauseButton.SetActive(false);
        SceneTransition.SwitchToScene("MenuScene");
        Time.timeScale = 1f;
    }
}
