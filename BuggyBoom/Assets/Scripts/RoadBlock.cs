using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class RoadBlock : MonoBehaviour
{

    // Private Variables
    private float originalX;

	// Use this for initialization
	void Start ()
	{
	    originalX = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
	    if (transform.position.x > originalX || transform.position.x < originalX)
	    {
	        transform.position = new Vector3(originalX,
	            transform.position.y,
	            transform.position.z);
        }
	}
}
