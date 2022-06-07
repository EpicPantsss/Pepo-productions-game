using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{

    public bool IsPaused = false;
    public GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
            {
                pauseMenu.SetActive(false);
                ResumeGame(pauseMenu);
            }
            else
            {
                pauseMenu.SetActive(true);
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        IsPaused = true;
        Cursor.visible = true;
    }
    public void ResumeGame(GameObject idk)
    {
        Time.timeScale = 1f;
        idk.SetActive(false);
        IsPaused = false;
        Cursor.visible = false;
    }

    public void Restart(GameObject idk)
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1f;
        IsPaused = false;
    }

    public void Controls (GameObject idk)
    {
        idk.SetActive(true);
    }
    public void exitControls(GameObject idk)
    {
        idk.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
