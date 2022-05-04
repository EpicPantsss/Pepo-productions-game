using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefinitivesMenu : MonoBehaviour
{
    public void EscenaJuego()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void PasivesMenu()
    {
        SceneManager.LoadScene("PassivesMenu");
    }
}
