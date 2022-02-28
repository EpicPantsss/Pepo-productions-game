using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void EscenaJuego()
    {
        SceneManager.LoadScene("SampleScene");
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
