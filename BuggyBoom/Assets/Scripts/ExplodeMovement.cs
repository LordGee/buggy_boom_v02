using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeMovement : MonoBehaviour
{

    // Private Variable
    private float movementSpeed = 5f;
	
	// Update is called once per frame
	void Update ()
    {
	    transform.Translate(0f, 0f, -(movementSpeed * Time.deltaTime));
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Shredder")
        {
            Destroy(this.gameObject);
        }
    }
}
