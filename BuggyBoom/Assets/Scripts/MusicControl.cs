using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicControl : MonoBehaviour {

    public AudioClip[] musicSelection;

    private new AudioSource audio;
    private PlayerPrefsControlScript playerPrefs;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        audio = GetComponent<AudioSource>();
        playerPrefs = FindObjectOfType<PlayerPrefsControlScript>();
        audio.volume = playerPrefs.GetMusicVolume();
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        AudioClip thisSceneMusic = musicSelection[scene.buildIndex];
        if (thisSceneMusic)
        {
            audio.clip = thisSceneMusic;
            audio.loop = true;
            audio.Play();
        }
    }

    public void ChangeVolume(float newVolume = 1f)
    {
        audio.volume = newVolume;
    }
}
