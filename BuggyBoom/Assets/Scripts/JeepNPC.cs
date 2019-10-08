using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JeepNPC : MonoBehaviour {

    // Public Variables
    [Tooltip("Two Materials to define type of jeep")]
    public Material[] materials;
    [Tooltip("Projectile that the jeep shoots")]
    public GameObject projectile;

    // Private Variables
    private GameControlScript gameControl;
    private PlayerPrefsControlScript playerPrefs;
    private new AudioSource audio;
    private int[] roadLaneArray = { -3, -1, 1, 3 };
    private bool shooter, changer;
    private float actionTimer = 0f, actionFreq = 1.5f, minAction = 0.5f, maxAction = 2.5f;
    private float projectileLife = 4f;
    private float npcDamage, npcHealth;
    private int currentIndex, playerIndex;
    private float minMaxPos = 0.99f;
    private NPCObstacle moverScript;
    GameObject jeepBody, player;
    private float changerMoveCount = 0f;


    // Use this for initialization
    void Start()
    {
        gameControl = FindObjectOfType<GameControlScript>();
        playerPrefs = FindObjectOfType<PlayerPrefsControlScript>();
        moverScript = FindObjectOfType<NPCObstacle>();
        audio = GetComponent<AudioSource>();
        audio.volume = playerPrefs.GetSfXVolume();
        player = GameObject.FindGameObjectWithTag("Player");
        npcDamage = gameControl.GetNpcDamage();
        npcHealth = gameControl.GetNpcHealth();
        IsShooterAndMaterial();
        DestroyGlitchedObject();
    }

    /* Randomly chooses if next spawn Jeep is a normal, shooter or changer. */
    void IsShooterAndMaterial()
    {
        jeepBody = transform.Find("JEEP_BODY").gameObject;
        int whichJeep = Random.Range(0, 4);
        if (whichJeep == 1)
        {
            shooter = true;
            SetUpJeep(1);
        }
        else if (whichJeep == 2)
        {
            changer = true;
            moverScript.npcDamage += moverScript.npcDamage;
            SetUpJeep(2);
        }
        else
        {
            changer = false;
            shooter = false;
            jeepBody.GetComponent<Renderer>().material = materials[0];
        }
    }

    
    void Update()
    {
        if (Time.timeSinceLevelLoad - actionTimer > actionFreq)
        {
            if (shooter)
            {
                ShootProjectile();   
                audio.Play();
            }
            else if (changer)
            {
                currentIndex = FindCurrentIndex(transform.position.x, minMaxPos);
                playerIndex = FindCurrentIndex(player.gameObject.transform.position.x, minMaxPos);
                
                if (playerIndex < currentIndex)
                {
                    changerMoveCount = -2f;
                }
                else if (playerIndex > currentIndex)
                {
                    changerMoveCount = 2f;
                }
            }
            actionTimer = Time.timeSinceLevelLoad;
            actionFreq = Random.Range(minAction, maxAction);
        }
        if (changerMoveCount < -0.1f)
        {
            moverScript.ChangeLane(Vector3.left.x * 0.1f);
            changerMoveCount += 0.1f;
        }
        else if (changerMoveCount > 0.1f)
        {
            moverScript.ChangeLane(Vector3.right.x * 0.1f);
            changerMoveCount -= 0.1f;
        }
        else
        {
            moverScript.ChangeLane(0f);
        }
    }

    /* Adds the correcct material to the Jeep depending on type */
    void SetUpJeep(int _value)
    {
        jeepBody.GetComponent<Renderer>().material = materials[_value];
        actionTimer = Time.timeSinceLevelLoad;
    }

    /* Finds the correct index in the array of road lane positions */
    private int FindCurrentIndex(float _posX, float _minMaxX)
    {
        int returnIndex = currentIndex;
        for (int i = 0; i < roadLaneArray.Length; i++)
        {
            if (_posX == roadLaneArray[i] 
                || _posX < roadLaneArray[i] + minMaxPos
                && _posX > roadLaneArray[i] - minMaxPos)
            {
                returnIndex = i;
            }
        }
        return returnIndex;
    }

    /* Projectile is instantiated and the unique damage for the object
     * is passed to the projectile script */
    void ShootProjectile()
    {
        GameObject proj = Instantiate(projectile,
                new Vector3(transform.position.x, transform.position.y, transform.position.z - 1),
                Quaternion.identity);
        proj.GetComponent<ProjectileSpawnEnemy>().projectileDamage = npcDamage;
        Destroy(proj, projectileLife);
    }

    public int[] GetLaneArray()
    {
        return roadLaneArray;
    }

    /* After a boss and before road blocks a glitched Jeep is spawned with is 
     * super doper powerful, it has the same attributes as a road block so if 
     * crashed into or hits with projectile will instantly kill the player. 
     * This is a tempory fix. */
    private void DestroyGlitchedObject()
    {
        if (npcHealth > 10000)
        {
            Destroy(gameObject);
        }
    }
}
