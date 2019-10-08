using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialOffset : MonoBehaviour
{
    // Private Variables
    private Renderer rend;
    public float offsetSpeed = 0.1f;
    private float initialThreshold = 2.9f;
    private float preIncrementSpeed = 0.001f;

	// Use this for initialization
	void Start ()
	{
	    rend = GetComponent<Renderer>();
	    offsetSpeed = 0.1f;
	    initialThreshold = 2.9f;
	    preIncrementSpeed = 0.001f;
}
	
	// Update is called once per frame
	void Update ()
    {
		rend.material.SetTextureOffset("_MainTex", new Vector2(0f, -(Time.timeSinceLevelLoad * offsetSpeed)));
	    if (offsetSpeed < initialThreshold)
	    {
	        offsetSpeed += preIncrementSpeed;
	    }
	}

    
}
