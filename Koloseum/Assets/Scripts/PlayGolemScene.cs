using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGolemScene : MonoBehaviour
{


    public void OnStartButtonClick()
    {
        // Load the specified scene
         SceneManager.LoadScene("golem");
    }
}
