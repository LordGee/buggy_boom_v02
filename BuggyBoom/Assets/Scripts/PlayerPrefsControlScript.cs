using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerPrefsControlScript : MonoBehaviour {

    // Options
    private const string MUSIC_VOLUME = "music_volume";
    private const string SFX_VOLUME = "sfx_volume";
    private const string AUTO_FIRE = "auto_fire";
    private const string ACCELEROMETER = "accelerometer";

    // Player Results
    private const string GAME_MONEY = "game_money";
    private const string CURRENT_MONEY = "current_money";
    private const string ACC_MONEY = "accumalitive_money";

    // Buggy Upgrades
    private const string MIN_HEALTH = "minimum_health";
    private const string FIRE_POWER = "fire_power";
    private const string MIN_MULTI = "minimum_multiplier";

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey(MUSIC_VOLUME)) { SetMusicVolume(1f); }
        if (!PlayerPrefs.HasKey(SFX_VOLUME)) { SetSfxVolume(1f); }
    }

    /* Set values */
    public void SetMusicVolume(float _value)
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME, _value);
    }

    public void SetSfxVolume(float _value)
    {
        PlayerPrefs.SetFloat(SFX_VOLUME, _value);
    }

    public void SetAutoFire(bool _value)
    {
        PlayerPrefs.SetInt(AUTO_FIRE, _value ? 1 : 0);
    }

    public void SetAccelerometer(bool _value)
    {
        PlayerPrefs.SetInt(ACCELEROMETER, _value ? 1 : 0);
    }

    public void SetGameMoney(int _value)
    {
        PlayerPrefs.SetInt(GAME_MONEY, _value);
    }

    public void SetCurrentGameMoney(int _value)
    {
        PlayerPrefs.SetInt(CURRENT_MONEY, _value);
    }

    public void SetAccumalitiveMoney(int _value)
    {
        PlayerPrefs.SetInt(ACC_MONEY, _value);
    }

    public void SetMinimumHealth(float _value)
    {
        PlayerPrefs.SetFloat(MIN_HEALTH, _value);
    }

    public void SetFirePower(float _value)
    {
        PlayerPrefs.SetFloat(FIRE_POWER, _value);
    }

    public void SetMinimumMultipler(float _value)
    {
        PlayerPrefs.SetFloat(MIN_MULTI, _value);
    }

    /* Get values */
    public float GetMusicVolume() { return PlayerPrefs.GetFloat(MUSIC_VOLUME); }
    public float GetSfXVolume() { return PlayerPrefs.GetFloat(SFX_VOLUME); }
    public bool GetAutoFire() { return PlayerPrefs.GetInt(AUTO_FIRE) == 1; }
    public bool GetAccelerometer() { return PlayerPrefs.GetInt(ACCELEROMETER) == 1; }
    public int GetGameMoney() { return PlayerPrefs.GetInt(GAME_MONEY); }
    public int GetCurrentGameMoney() { return PlayerPrefs.GetInt(CURRENT_MONEY); }
    public int GetAccumalitiveMoney() { return PlayerPrefs.GetInt(ACC_MONEY); }
    public float GetMinimumHealth() { return PlayerPrefs.GetFloat(MIN_HEALTH); }
    public float GetFirePower() { return PlayerPrefs.GetFloat(FIRE_POWER); }
    public float GetMinimumMultipler() { return PlayerPrefs.GetFloat(MIN_MULTI); }
}
