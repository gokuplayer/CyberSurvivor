using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class StartManager : MonoBehaviour
{
    public GameObject StartMenuObject;
    public GameObject OptionsMenuObject;
    public GameObject CharMenuObject;
    public GameObject MapMenuObject;
    public GameObject CreditMenuObject;
    public AudioMixer MainMixer;

    public Toggle fullScreenToggle;
    public TMP_Dropdown qualityDropdown;
    public Slider volumeSlider;

    void Awake()
    {
        StartMenuObject = GameObject.Find("StartMenu");
        OptionsMenuObject = GameObject.Find("OptionsMenu");
        CharMenuObject = GameObject.Find("CharMenu");
        MapMenuObject = GameObject.Find("MapMenu");
        CreditMenuObject = GameObject.Find("CreditMenu");

        StartMenuObject.SetActive(true);
        OptionsMenuObject.SetActive(false);
        CharMenuObject.SetActive(false);
        MapMenuObject.SetActive(false);
        CreditMenuObject.SetActive(false);
    }

    //Start Menu Functions

    public void PlayButton()
    {
        StartMenuObject.SetActive(false);
        CharMenuObject.SetActive(true);
    }

    public void BackButton()
    {
        StartMenuObject.SetActive(true);
        CharMenuObject.SetActive(false);
    }

    public void NextButton()
    {
        CharMenuObject.SetActive(false);
        MapMenuObject.SetActive(true);
    }

    public void MapBackButton()
    {
        CharMenuObject.SetActive(true);
        MapMenuObject.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.UnloadSceneAsync("StartMenu");
        SceneManager.LoadScene("MapScene");
    }

    public void OptionsMenu()
    {
        StartMenuObject.SetActive(false);
        OptionsMenuObject.SetActive(true);

        if (fullScreenToggle != null)
        {
            fullScreenToggle.isOn = Screen.fullScreen;
        }
        if (qualityDropdown != null)
        {
            qualityDropdown.value = QualitySettings.GetQualityLevel();
        }
        if (volumeSlider != null)
        {
            float volVal;
            MainMixer.GetFloat("volume", out volVal);
            volumeSlider.value = volVal;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CreditMenu()
    {
        StartMenuObject.SetActive(false);
        CreditMenuObject.SetActive(true);
    }

    //Options Menu Functions

    public void SetFullScreen(bool isFullscreen)
    {
        Debug.Log("Fullscreen: " + isFullscreen.ToString());

        Screen.fullScreen = isFullscreen;
    }

    public void SetQuality(int qualityIndex)
    {
        Debug.Log("Quality Level: " + qualityIndex.ToString());

        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetVolume(float volume)
    {
        Debug.Log("Volume Level: " + volume.ToString());

        MainMixer.SetFloat("volume", volume);
    }

    public void OptionsBack()
    {
        StartMenuObject.SetActive(true);
        OptionsMenuObject.SetActive(false);
    }

    //Credit Menu Functions

    public void CreditBack()
    {
        StartMenuObject.SetActive(true);
        CreditMenuObject.SetActive(false);
    }
}