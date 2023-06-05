using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenuScene : MonoBehaviour
{


    public void OnStartButtonClick()
    {
        // Load the specified scene
         SceneManager.LoadScene("Menu");
    }
}
