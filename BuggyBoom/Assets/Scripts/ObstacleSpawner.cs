using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{

    // Public Variables
    [Tooltip("Define how often an obstacle is Spawned (Min / Max)")]
    public float spawnMin, spawnMax;
    
    [Tooltip("Define time until the first object is spawned")]
    public float timeBeforeFirstSpawn = 5f;

    // Private Variables
    private float timeSinceLastSpawn = 0f;
    private bool startSpawning = false;
    private float spawnInterval;
    
    private int[] laneArray;
    private GameObject obstacle;
    private GameControlScript gameControl;
    private bool bossSpawned, bossTime, bossExists;

    private int roadBlockCount, blockMin = 2, blockMax = 6;

    void Start()
    {
        gameControl = FindObjectOfType<GameControlScript>();
    }

    // Update is called once per frame
    void Update()
    {
        // checks to see if enough time has elapsed before starting the spawn process
        if (!startSpawning && Time.timeSinceLevelLoad > timeBeforeFirstSpawn)
        {
            startSpawning = true;
        }
        
        // Perform checks to check if its time to spawn a boss or if a boss already exists
        bossTime = gameControl.currentSpawn == GameControlScript.SPAWN_NPC.Boss;
        bossExists = GameObject.FindWithTag("NPCBoss");

        /*  once the spawn process has starteded a new a new car object will be spawned at
            specific intervals within the bounds af a random lane on the grid. The object 
            will be child of the parent spawner and the time counter since the last spawn 
            is then reset   */
        if (startSpawning && Time.timeSinceLevelLoad - timeSinceLastSpawn > spawnInterval && !bossTime && !bossExists)
        {
            PrepareObstacleToSpawn(false);
            if (obstacle.tag == "RoadBlock")
            {
                RoadBlockSpawn();
            }
            else
            {
                Spawn();
            }
            timeSinceLastSpawn = Time.timeSinceLevelLoad;
        }
        else if (bossTime && !bossSpawned && !bossExists)
        {
            PrepareObstacleToSpawn(true);
            StartCoroutine(SpawnBoss());
        }
    }

    /* Spawn a new game object within a random lane within the game grid as a child of the 
     * spawner game object the next spawn interval will then be randomly defined. */
    void Spawn()
    {
        GameObject newObstacle = Instantiate(obstacle, new Vector3(laneArray[Random.Range(0, laneArray.Length)], transform.position.y, transform.position.z), Quaternion.identity);
        newObstacle.transform.parent = gameObject.transform;
        spawnInterval = Random.Range(spawnMin, spawnMax);
    }

    /* Prepares the next spawn conditions by getting the next spawn object and defining 
     * which lanes are allowed. */
    void PrepareObstacleToSpawn(bool _boss)
    {
        bossSpawned = _boss;
        obstacle = gameControl.GetNpcGameObjectToSpawn();
        laneArray = gameControl.GetLaneArray();
    }

    /* Having a short delay before the boss spawns improves the experience slightly */
    private IEnumerator SpawnBoss()
    {
        yield return new WaitForSeconds(2.0f);
        Spawn();
    }

    /* Spawns a random amount of road blocks in a row between 2 and 5 as various positions 
     * then spawns the objects in their respective positions.
     * TODO: This needs refactoring */
    void RoadBlockSpawn()
    {
        roadBlockCount = Random.Range(blockMin, blockMax);
        int[] tempLanes = new int[roadBlockCount];
        bool test = true;
        for (int i = 0; i < tempLanes.Length; i++)
        {
            tempLanes[i] = Random.Range(0, laneArray.Length);
            for (int j = 0; j < tempLanes.Length; j++)
            {
                if (tempLanes[i] == tempLanes[j] && i != j)
                {
                    test = false;
                }
            }
            if (!test)
            {
                i--;
                test = true;
            }
        }
        for (int i = 0; i < tempLanes.Length; i++)
        {
            GameObject newObstacle = Instantiate(obstacle, new Vector3(laneArray[tempLanes[i]], transform.position.y, transform.position.z), Quaternion.identity);
            newObstacle.transform.parent = gameObject.transform;
            spawnInterval = Random.Range(spawnMin + 1f, spawnMax + 1f);
        }
    }
}
