using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerScene : MonoBehaviour
{
    public GameObject settingPanel;

    public void PlayGame()
    {
        SceneTransition.SwitchToScene("StartGameScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenSettingPanel()
    {
        settingPanel.SetActive(true);
    }

    public void ExitSettingPannel()
    {
        settingPanel.SetActive(false);
    }
}
