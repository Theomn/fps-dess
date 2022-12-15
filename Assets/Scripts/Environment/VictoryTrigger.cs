using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        Invoke("LoadVictoryScene", 3f);
    }

    void LoadVictoryScene()
    {
        SceneManager.LoadScene("Victory Menu");
    }



    /*void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("Player");
            StartCoroutine(loadScene());
        }
    }
    IEnumerator loadScene()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("Victory Menu");
    }*/
}
