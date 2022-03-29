using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void EscenaPassivas()
    {
        SceneManager.LoadScene("PassivesMenu");
    }
    public void EscenaOpciones()
    {
        SceneManager.LoadScene("Options");
    }

    public void EscenaControles()
    {
        SceneManager.LoadScene("ControlMenu");
    }
    public void Salir()
    {
        Application.Quit();
    }
}
