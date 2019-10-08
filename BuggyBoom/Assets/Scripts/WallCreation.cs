using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCreation : MonoBehaviour
{
    //Public Variables
    public GameObject explosion;

    // Private Variables
    public float wallCurrentSpeed = 10f;
    // private float scaleX, scaleY, scaleZ;
    private Color col;

    // Use this for initialization
    void Start () {
        /* Used when generating random cubes */
        // GetRandomSize(); 
        // transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        // Reference: https://docs.unity3d.com/ScriptReference/Random.ColorHSV.html 
        // GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        /*  */
    }

    // Update is called once per frame
    void Update () {
		transform.Translate(0f, 0f, -(wallCurrentSpeed * Time.deltaTime));
	}

    /* Prepares a random size and shape for the next spawned building */
    void GetRandomSize()
    {
       // scaleX = Random.Range(2f, 8f);
       // scaleY = Random.Range(2f, 10f);
       // scaleZ = Random.Range(2f, 5f);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag != "Road" && col.gameObject.tag != "Player")
        {
            Instantiate(explosion, col.transform.position, Quaternion.identity);
            if (col.gameObject.tag == "Projectile")
            {
                Destroy(col.transform.parent.gameObject);
            }
            else
            {
                Destroy(col.gameObject);
            }
        }
    }
}
