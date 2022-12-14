using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject checkPointPanel;

    public List<GameObject> checkpointButtons;

    private void Start()
    {
        foreach(GameObject button in checkpointButtons)
        {
            button.SetActive(false);
        }
        checkpointButtons[0].SetActive(true);
        foreach(Checkpoint cp in ProgressionManager.instance.checkpoints.list)
        {
            if (cp.id < checkpointButtons.Count && cp.state != Checkpoint.State.Locked)
            {
                checkpointButtons[cp.id].SetActive(true);
            }
        }
    }

    public void StartGame()
    {
        checkPointPanel.gameObject.SetActive(true);
    }

    public void SettingsButton()
    {
        settingsPanel.gameObject.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        settingsPanel.gameObject.SetActive(false);
    }

    public void CloseCheckPointPanel()
    {
        checkPointPanel.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }



    public void LoadCheckpoint(int checkpointId)
    {
        ProgressionManager.instance.ActivateCheckpoint(checkpointId);
        SceneManager.LoadScene("Game");
    }
}
