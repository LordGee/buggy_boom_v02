using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNPC : MonoBehaviour {

    //Public Variables
    [Tooltip("Gameobject it include upon destroy")]
    public GameObject DeathParticleEffect;
    [Tooltip("Projectile that the boss shoots")]
    public GameObject projectile;
    [Tooltip("Small explosion SFX")]
    public AudioClip clip;

    // Private Variables
    private GameControlScript gameControl;

    private new AudioSource audio;
    private float npcHealth;
    private float npcDamage;
    private float npcSpeed;
    private float npcPoints;

    private float actDeltaSpeed;
    private bool moveType;
    private bool direction;
    
    private float shootTimer;
    private float shootFreq = 1f;

    void Start()
    {
        gameControl = FindObjectOfType<GameControlScript>();
        npcHealth = gameControl.GetNpcHealth();
        npcDamage = gameControl.GetNpcDamage();
        npcSpeed = gameControl.GetNpcSpeed();
        npcPoints = gameControl.GetNpcPoints();
        direction = true;
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        actDeltaSpeed = npcSpeed * Time.deltaTime;
        moveType = false;
        MoveForward();
        if (moveType)
        {
            MoveSideToSide();
        }
        if (Time.timeSinceLevelLoad - shootTimer > shootFreq)
        {
            ShootProjectile();
            audio.Play();
            shootTimer = Time.timeSinceLevelLoad;
            shootFreq = Random.Range(0.2f, 1f);
        }
    }

    /* Move boss forward torward player
     * At set position change move type to false
     * alloing for a side to side movement to take effect
     * This will continue to be check in case the boss is 
     * pushed back, will then be re-adjusted */
    void MoveForward()
    {
        if (transform.position.z > 3f)
        {
            transform.Translate(0f, 0f, -actDeltaSpeed);
            moveType = false;
        }
        else
        {
            moveType = true;
        }
    }

    /* Move the boss the right side of the screen, once
     * a set amount of distance has been reached will reverse
     * the direction */
    void MoveSideToSide()
    {
        if (direction)
        {
            transform.Translate(actDeltaSpeed, 0f, 0f);
            if (transform.position.x > 4.0f)
            {
                direction = false;
            }
        }
        else
        {
            transform.Translate(-actDeltaSpeed, 0f, 0f);
            if (transform.position.x < -4)
            {
                direction = !direction;
            }
        }
    }

    /* Projectile is instantiated and the unique damage for the object
     * is passed to the projectile script */
    void ShootProjectile()
    {
        GameObject proj = Instantiate(projectile,
                new Vector3(transform.position.x, transform.position.y, transform.position.z - 1),
                Quaternion.identity);
        proj.GetComponent<ProjectileSpawnEnemy>().projectileDamage = npcDamage;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Projectile")
        {
            Instantiate(DeathParticleEffect, transform.position, Quaternion.identity);
            gameControl.PlayAudioClip(clip);
            gameControl.DamageNPC(this.gameObject, npcPoints, gameControl.GetPlayerDamage(), ref npcHealth);
            Destroy(col.transform.parent.gameObject);
        }
    }

    
}
