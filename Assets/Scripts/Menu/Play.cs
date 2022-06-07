using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    private Animator anim;

    public Button button;

    private AudioSource audioSource;

    public List<AudioClip> audiosToPlay;


    [SerializeField] Slider sliderVolume;
    [SerializeField] Toggle toggleScreenMode;

    private GameObject menuOpened;

    Scene actualScene;

    private void Start()
    {
        actualScene = SceneManager.GetActiveScene();
        if (actualScene.name == "Menu")
        {
            anim = GameObject.Find("Gallo").GetComponent<Animator>();
            anim.Play("Menu_animation");

            AudioListener.volume = 0.5f;
        }

        audioSource = GetComponent<AudioSource>();

        // Ajustar el valor del slider al del volumen general
        sliderVolume.value = AudioListener.volume;
    }

    private void Update()
    {
        if ((audioSource.clip == audiosToPlay[1] || audioSource.clip == audiosToPlay[2]) && !audioSource.isPlaying)
        {
            if (audioSource.clip == audiosToPlay[2] && actualScene.name == "Menu")
                anim.Play("Menu_animation");
        }

        if (menuOpened != null && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu(menuOpened);
        }
    }

    public void PlayGame()
    {
        StartCoroutine(WaitToContinue());
    }

    public void ButtonSelected()
    {
        audioSource.clip = audiosToPlay[1];// Sonido de selección
        audioSource.loop = false;
        audioSource.Play();
    }

    public void SelectedChickenAnimation()
    {
        anim.Play("Menu_scream_animation");
        audioSource.clip = audiosToPlay[2];// Chillido del gallo
        audioSource.loop = false;
        audioSource.Play();
    }

    IEnumerator WaitToContinue()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Level 2");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenMenu(GameObject menutoOpen)
    {
        menuOpened = menutoOpen;
        menutoOpen.SetActive(true);
    }
    public void CloseMenu(GameObject menutoOpen)
    {
        menuOpened = null;
        menutoOpen.SetActive(false);
    }


    // Cambiar volumen
    public void ChangeVolume()
    {
        AudioListener.volume = sliderVolume.value;
    }
    public void ChangeScreenMode()
    {
        switch (toggleScreenMode.isOn)
        {
            case true:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case false:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }
    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}