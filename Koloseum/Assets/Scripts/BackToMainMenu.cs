using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadMainMenuScene();
        }
    }

    void LoadMainMenuScene()
    {
        SceneManager.LoadScene("Main menu");
    }
}