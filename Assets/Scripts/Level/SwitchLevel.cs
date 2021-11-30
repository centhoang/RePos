using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchLevel : MonoBehaviour
{
    [SerializeField] private string nextLevelToLoad;

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        if (nextLevelToLoad != null && nextLevelToLoad != "")
        {
            SceneManager.LoadScene(nextLevelToLoad);
        }
    }
}
