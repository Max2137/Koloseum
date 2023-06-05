using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayWendigoScene : MonoBehaviour
{


    public void OnStartButtonClick()
    {
        // Load the specified scene
         SceneManager.LoadScene("wendigo");
    }
}
