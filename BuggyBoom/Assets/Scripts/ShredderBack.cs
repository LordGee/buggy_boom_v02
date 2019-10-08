using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShredderBack : MonoBehaviour
{

    private GameControlScript gameControl;

    private void Start()
    {
        gameControl = FindObjectOfType<GameControlScript>();
    }

    private void OnTriggerEnter(Collider col)
    {
        Destroy(col.gameObject);
        if (col.gameObject.tag == "Player")
        {
            gameControl.currentyGameState = GameControlScript.GAME_STATE.GameOver;
        }
    }
}
