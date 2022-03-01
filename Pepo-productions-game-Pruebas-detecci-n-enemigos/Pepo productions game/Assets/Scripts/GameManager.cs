using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public GameObject definitive;
    [HideInInspector]
    public GameObject passive;

    public bool extra;
    private void Start()
    {
        if (!extra)
            DontDestroyOnLoad(this.gameObject);
    }

    public void SetDefinitive(GameObject definitiveChoosed)
    {
        definitive = definitiveChoosed;
    }
    public void SetPassive(GameObject passiveChoosed)
    {
        passive = passiveChoosed;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}