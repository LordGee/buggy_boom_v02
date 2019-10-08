using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Private variables
    private GameObject player;

	// Use this for initialization
	void Start () {
        // find the player object within the scene
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
	{
        // Changed the position of the camera arm to reflect the player position
	    transform.position = player.transform.position;
	}
}
