using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PasivesMenu : MonoBehaviour
{
    public void EscenaDefinitivas()
    {
        SceneManager.LoadScene("DefinitivesMenu");
    }

    public void EscenaMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
