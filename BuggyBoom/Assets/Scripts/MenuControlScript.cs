using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

public class MenuControlScript : MonoBehaviour
{
    public GameObject optionStartButton, upgradeStartButton, mainStartButton;
    private GameObject optionCanvas, upgradeCanvas;
    private PlayerPrefsControlScript prefsControl;
    private MusicControl musicControl;
    private Text hording, level, firePower, health, multipler, error;
    private const int levelIndicator = 10000;
    private const int FIRE_POWER = 10000;
    private const int MIN_HEALTH = 50000;
    private const int MULTIPLIER = 250000;
    private EventSystem evSystem;

    // Use this for initialization
    void Start ()
	{
	    optionCanvas = GameObject.Find("OptionsCanvas");
	    upgradeCanvas = GameObject.Find("UpgradeCanvas");
        prefsControl = FindObjectOfType<PlayerPrefsControlScript>();
	    musicControl = FindObjectOfType<MusicControl>();
	    evSystem = FindObjectOfType<EventSystem>();
	    hording = GameObject.Find("Txt_Hordings").GetComponent<Text>();
	    level = GameObject.Find("Txt_Level").GetComponent<Text>();
	    firePower = GameObject.Find("Txt_FirePower").GetComponent<Text>();
	    health = GameObject.Find("Txt_Health").GetComponent<Text>();
	    multipler = GameObject.Find("Txt_Multiplier").GetComponent<Text>();
	    error = GameObject.Find("Txt_Error").GetComponent<Text>();
        CalculateLevel();
        SetDefaultValues();
	}

    private void Update()
    {
        /* On PS4 version the slightest touch on the track pad deactivates the navigation 
         * by setting the current selected object to null. The following should provide a reset. */
        if (Input.GetAxis("Fire2") != 0)
        {
            if (evSystem.currentSelectedGameObject == null)
            {
                SetDefaultValues();
            }
        }
        
    }

    private void SetDefaultValues()
    {
        optionCanvas.GetComponent<Canvas>().sortingOrder = -1;
        upgradeCanvas.GetComponent<Canvas>().sortingOrder = -1;
        evSystem.SetSelectedGameObject(mainStartButton);
        var sliders = optionCanvas.gameObject.GetComponentsInChildren<Slider>();
        foreach (var slider in sliders)
        {
            if (slider.name == "Sld_MusicVol")
                slider.value = prefsControl.GetMusicVolume();
            else if (slider.name == "Sld_SFXVol")
                slider.value = prefsControl.GetSfXVolume();
        }
        var toggles = optionCanvas.gameObject.GetComponentsInChildren<Toggle>();
        foreach (var toggle in toggles)
        {
            if (toggle.name == "Tog_AutoFire")
                toggle.isOn = prefsControl.GetAutoFire();
            else if (toggle.name == "Tog_Accelerometer")
                toggle.isOn = prefsControl.GetAccelerometer();
        }
    }

    private void CalculateLevel()
    {
        int accValue = prefsControl.GetAccumalitiveMoney();
        level.text = "Level : " + (Mathf.FloorToInt(accValue / levelIndicator) + 1).ToString();
        firePower.text = "Level : " + prefsControl.GetFirePower();
        health.text = "Level : " + prefsControl.GetMinimumHealth();
        multipler.text = "Level : " + prefsControl.GetMinimumMultipler();
        error.gameObject.SetActive(false);
        UpdateHordingValue();
    }

    private void UpdateHordingValue()
    {
        hording.text = prefsControl.GetCurrentGameMoney().ToString("N0");
    }

    public void ShowOptionCanvas()
    {
        optionCanvas.GetComponent<Canvas>().sortingOrder = 1;
        evSystem.SetSelectedGameObject(optionStartButton);
    }

    public void HideOptionCanvas()
    {
        optionCanvas.GetComponent<Canvas>().sortingOrder = -1;
        evSystem.SetSelectedGameObject(mainStartButton);
    }

    public void ShowUpgradeCanvas()
    {
        upgradeCanvas.GetComponent<Canvas>().sortingOrder = 1;
        evSystem.SetSelectedGameObject(upgradeStartButton);
    }

    public void HideUpgradeCanvas()
    {
        upgradeCanvas.GetComponent<Canvas>().sortingOrder = -1;
        evSystem.SetSelectedGameObject(mainStartButton);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SaveOptionsToPlayerPrefs()
    {
        var sliders = optionCanvas.gameObject.GetComponentsInChildren<Slider>();
        foreach (var slider in sliders)
        {
            if (slider.name == "Sld_MusicVol")
                prefsControl.SetMusicVolume(slider.value);
            else if (slider.name == "Sld_SFXVol")
                prefsControl.SetSfxVolume(slider.value);
        }
        var toggles = optionCanvas.gameObject.GetComponentsInChildren<Toggle>();
        foreach (var toggle in toggles)
        {
            if (toggle.name == "Tog_AutoFire")
                prefsControl.SetAutoFire(toggle.isOn);
            else if (toggle.name == "Tog_Accelerometer")
                prefsControl.SetAccelerometer(toggle.isOn);
        }
    }

    public void UpgradeFirePower()
    {
        if (prefsControl.GetCurrentGameMoney() >= FIRE_POWER)
        {
            prefsControl.SetFirePower(prefsControl.GetFirePower() + 1);
            prefsControl.SetCurrentGameMoney(prefsControl.GetCurrentGameMoney() - FIRE_POWER);
            CalculateLevel();
        }
        else
        {
            error.gameObject.SetActive(true);
        }
    }

    public void UpgradeHealth()
    {
        if (prefsControl.GetCurrentGameMoney() >= MIN_HEALTH)
        {
            prefsControl.SetMinimumHealth(prefsControl.GetMinimumHealth() + 1);
            prefsControl.SetCurrentGameMoney(prefsControl.GetCurrentGameMoney() - MIN_HEALTH);
            CalculateLevel();
        }
        else
        {
            error.gameObject.SetActive(true);
        }
    }

    public void UpgradeMultiplier()
    {
        if (prefsControl.GetCurrentGameMoney() >= MULTIPLIER)
        {
            prefsControl.SetMinimumMultipler(prefsControl.GetMinimumMultipler() + 1);
            prefsControl.SetCurrentGameMoney(prefsControl.GetCurrentGameMoney() - MULTIPLIER);
            CalculateLevel();
        }
        else
        {
            error.gameObject.SetActive(true);
        }
    }

    public void ChangeMusicVolume()
    {
        GameObject slider = GameObject.Find("Sld_MusicVol");
        musicControl.ChangeVolume(slider.GetComponent<Slider>().value);
    }
}
