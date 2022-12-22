using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Invoke("LoadVictoryScene", 10f);
    }

    void LoadVictoryScene()
    {
        SceneManager.LoadScene("Victory Menu");
    }
}
