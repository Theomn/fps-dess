using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string levelToload;
    

    public void StartGame()
    {
        SceneManager.LoadScene(levelToload);
    }

    public void SettingsButton()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
