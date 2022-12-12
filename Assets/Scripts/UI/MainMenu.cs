using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel;

        
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void SettingsButton()
    {
        settingsPanel.gameObject.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        settingsPanel.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
