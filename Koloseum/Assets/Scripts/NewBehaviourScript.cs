using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private static MusicManager instance;

    private void Awake()
    {
        // Check if an instance already exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // If no instance exists, set this as the instance and mark it as persistent
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
