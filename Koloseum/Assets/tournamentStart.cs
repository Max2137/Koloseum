using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class tournamentStart : MonoBehaviour
{
    public string sampleScene;
    private Button przycisk;

    private void Start()
    {
        przycisk = GetComponent<Button>();
        przycisk.onClick.AddListener(ZmienScene);
    }

    private void ZmienScene()
    {
        SceneManager.LoadScene(sampleScene);

    }
}
