using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Set the time scale to 1
            Time.timeScale = 1f;

            // Restart the game by loading Scene 1
            SceneManager.LoadScene(0);
        }
    }
}
