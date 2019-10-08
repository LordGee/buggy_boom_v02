using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResultsControlScript : MonoBehaviour
{
    public GameObject continueButton;
    private int gameScore, currentScore;
    private PlayerPrefsControlScript playerPrefs;
    private Text earned, total;
    private EventSystem evSystem;

    // Use this for initialization
    void Start () {
        playerPrefs = FindObjectOfType<PlayerPrefsControlScript>();
        gameScore = playerPrefs.GetGameMoney();
        currentScore = playerPrefs.GetCurrentGameMoney();
	    currentScore += gameScore;
	    earned = GameObject.Find("Txt_GameScore").GetComponent<Text>();
	    total = GameObject.Find("Txt_CurrentGameScore").GetComponent<Text>();
	    earned.text = gameScore.ToString();
	    total.text = currentScore.ToString();
	    playerPrefs.SetAccumalitiveMoney(playerPrefs.GetAccumalitiveMoney() + gameScore);
        gameScore = 0;
	    playerPrefs.SetGameMoney(gameScore);
	    playerPrefs.SetCurrentGameMoney(currentScore);
        evSystem = FindObjectOfType<EventSystem>();
    }

    private void Update()
    {
        /* On PS4 version the slightest touch on the track pad deactivates the navigation 
        * by setting the current selected object to null. The following should provide a reset. */
        if (Input.GetAxis("Fire2") != 0)
        {
            if (evSystem.currentSelectedGameObject == null)
            {
                evSystem.SetSelectedGameObject(continueButton);
            }
        }
    } 
}
