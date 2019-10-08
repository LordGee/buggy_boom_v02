using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsCollect : MonoBehaviour
{

    // Public Variables
    public AudioClip clip;

    // Private Variables
    private float speed = 8f;
    private int pointValue = 20;
    private GameControlScript gameControl;
   
    // Use this for initialization
    void Start ()
    {
        gameControl = FindObjectOfType<GameControlScript>();
    }
	
	// Update is called once per frame
	void Update () {
		transform.Translate(0f, 0f, -(speed * Time.deltaTime));
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            gameControl.AddPoints(pointValue);
            gameControl.PlayAudioClip(clip);
        }
        Destroy(gameObject);
    }
}
