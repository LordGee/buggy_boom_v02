using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawning : MonoBehaviour
{
    // Public Variables

    [Tooltip("Attach multiple meshes of houses")]
    public GameObject[] house;
    /* // Used wen generating the objects from a simple cube
        [Tooltip("Attach Prefab of cube with appropriate Rigidbody and Constraints")]
        public GameObject wall;
    */

    // Private Variables
    private float timeBeforeFirstSpawn = 2f;
    private float spawnInterval = 0.75f; // was 0.5f
    private float timeSinceLastSpawn = 0f;
    private bool startSpawning = false;
    

    // Update is called once per frame
    void Update () {
        if (!startSpawning && Time.timeSinceLevelLoad > timeBeforeFirstSpawn)
        {
            startSpawning = true;
        }
	    if (startSpawning && Time.timeSinceLevelLoad - timeSinceLastSpawn > spawnInterval)
	    {
	        Vector3 pos = transform.position;
	        GameObject newHouse = Instantiate(house[Random.Range(0, house.Length)], new Vector3(pos.x, pos.y, pos.z), Quaternion.identity);
	        newHouse.transform.parent = gameObject.transform;
	        timeSinceLastSpawn = Time.timeSinceLevelLoad;
	    }
	}
}
