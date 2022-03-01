using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void EscenaPasivas()
    {
        SceneManager.LoadScene("PasivesMenu");
    }
    public void EscenaOpciones()
    {
        SceneManager.LoadScene("Options");
    }
    public void Salir()
    {
        Application.Quit();
    }
}
