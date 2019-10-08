using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public Variables
    [Tooltip("Speed the Buggy will translate")] public float buggySpeed = 10f;
    [Tooltip("Attach Projectile object for the player")] public GameObject projectile;
    [Tooltip("Explode effect when damaged")] public GameObject explode;
    [Tooltip("Auto fire (On / Off)")] public bool autoFire;
    [Tooltip("Turn Accelerometer (One/Off)")] public bool accelerometer;
    public AudioClip laser, explosion;

    // Private Variables
    private GameControlScript gameControl;
    private Animator anim;
    private new AudioSource audio;
    private PlayerPrefsControlScript playerPrefs;
    private float StartZ;

    private float projectileRate = 0.1f;
    private float projectileCooldown;
    private float projectileLife = 1.75f;

    private RaycastHit detector;
    private float detectorDistance = 30f;
    private float projectileCounter = 0;

    void Start()
    {
        StartZ = transform.position.z;
        projectileCooldown = Time.timeSinceLevelLoad;
        anim = GetComponent<Animator>();
        gameControl = FindObjectOfType<GameControlScript>();
        audio = GetComponent<AudioSource>();
        playerPrefs = FindObjectOfType<PlayerPrefsControlScript>();
        audio.volume = playerPrefs.GetSfXVolume();
        autoFire = playerPrefs.GetAutoFire();
        accelerometer = playerPrefs.GetAccelerometer();
    }

	// Update is called once per frame
	void Update ()
	{
        // https://docs.unity3d.com/Manual/PlatformDependentCompilation.html
#if UNITY_EDITOR
        StandardBuilds();
#endif

#if UNITY_PS4
        PS4Builds();
#endif

#if UNITY_IOS
	    MobileBuilds();
#endif

#if UNITY_ANDROID
         MobileBuilds();
#endif

#if UNITY_WEBGL
        StandardBuilds();
#endif

#if UNITY_STANDALONE_WIN
	    StandardBuilds();
#endif

    }

    /* Added this to prevent game continuing if player has already been 
     * destroyed and it has not been detected. Interesting but doidn't 
     * work like I hoped */
    void OnDestroy()
    {
        gameControl.currentyGameState = GameControlScript.GAME_STATE.GameOver;
    }

    private void StandardBuilds()
    {
        if (!accelerometer)
            CheckHorizontalMovement();
        else
            CheckAccelerometerMovement();
        if (!autoFire)
            CheckManualFire();
        else
            CheckAutoFire();
    }

    private void PS4Builds()
    {
        if (!accelerometer)
            CheckHorizontalMovement();
        else
            // CheckTouchMovement();
            CheckAccelerometerMovement();
        if (!autoFire)
            CheckManualFire();
        else
            CheckAutoFire();
    }

    private void MobileBuilds()
    {
        if (!accelerometer)
            CheckTouchMovement();
        else
            CheckAccelerometerMovement();
        if (!autoFire)
            FireProjectile(); // still fires automatically but permenantly
        else
            CheckAutoFire();
    }

    /* Any horizontal inputs are translated to the new position of the player. */
    private void CheckHorizontalMovement()
    {
        transform.Translate(Input.GetAxis("Horizontal") * buggySpeed * Time.deltaTime, 0f, 0f);
        CheckBuggyRotation(Input.GetAxis("Horizontal"));
    }

    /* Any accelerometer inputs are translated on the x axis to the new position of the player. */
    private void CheckAccelerometerMovement()
    {
        Debug.Log(Input.acceleration);
        transform.Translate(Input.acceleration.x * buggySpeed * Time.deltaTime, 0, 0f);
        CheckBuggyRotation(Input.acceleration.x);
    }

    private void CheckTouchMovement()
    {
        if (Input.GetTouch(0).position.x < Screen.width / 2)
        {
            transform.Translate(-1f * buggySpeed * Time.deltaTime, 0, 0f);
            CheckBuggyRotation(-1f);
        }
        else if (Input.GetTouch(0).position.x > Screen.width / 2)
        {
            transform.Translate(1f * buggySpeed * Time.deltaTime, 0, 0f);
            CheckBuggyRotation(1f);
        }
        else
        {
            transform.Translate(0f * buggySpeed * Time.deltaTime, 0, 0f);
            CheckBuggyRotation(0f);
        }
        FireProjectile();
    }

    /* Checks are made for the direction that the player is moving and applys
     * the correct animation.Rotation was originally done through the code. */
    private void CheckBuggyRotation(float _direction)
    {
        if (_direction != 0)
        {
            // transform.Rotate(0f, GetTranslatedPosition(buggySpeedRot), 0f);
            if (_direction > 0)
            {
                anim.SetBool("TurnRight", true);
                anim.SetBool("TurnLeft", false);
            }
            else if (_direction < 0)
            {
                anim.SetBool("TurnRight", false);
                anim.SetBool("TurnLeft", true);
            }
        }
        else
        {
            // transform.Rotate(0f, GetTranslatedPosition(transform.rotation.y * -1, buggySpeedRot * 20f), 0f);
            anim.SetBool("TurnRight", false);
            anim.SetBool("TurnLeft", false);
        }
        CheckPositioningConstraints();
    }

    /* If option is selected autofire will use a Sphere Cast to detect enemies 
     * ahead of the players facing direction. */
    private void CheckAutoFire()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (Physics.SphereCast(transform.position, transform.localScale.y / 2, fwd, out detector, detectorDistance))
        {
            if (detector.transform.tag == "NPCJeep" || detector.transform.tag == "Projectile")
            {
                if (detector.transform.tag == "Projectile")
                {
                    if (projectileCounter <= 5)
                    {
                        if (FireProjectile())
                        {
                            projectileCounter++;
                        }
                    }
                }
                else
                {
                    FireProjectile();
                    projectileCounter = 0;
                }
                FireProjectile();
            }
        }
    }

    /* Checks for fire inputs, if true triggers the Fire Projectile function */
    private void CheckManualFire()
    {
        if (Input.GetAxis("Jump") != 0 || Input.GetAxis("Fire1") != 0)
        {
            FireProjectile();
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "ProjectileEnemy")
        {
            float dmg = col.gameObject.GetComponentInParent<ProjectileSpawnEnemy>().GetProjectileDamage();
            gameControl.DamagePlayer(dmg);
            gameControl.PlayAudioClip(explosion);
            GameObject effect = Instantiate(explode, col.transform.position, Quaternion.identity);
            Destroy(effect, 3f);
            Destroy(col.transform.parent.gameObject);
        }
    }

    /* Spawns an new projectile if sufficient time has elapsed since the last projectile, this
     * game object has a specified life span that will be destroyed after a set time the projectile 
     * cooldown period resets. This function returns true if successfull or false if not, this is 
     * soley for the Auto Fire function. */
    private bool FireProjectile()
    {
        if (Time.timeSinceLevelLoad - projectileCooldown > projectileRate)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
            GameObject newProjectile = Instantiate(projectile, pos, Quaternion.identity);
            Destroy(newProjectile, projectileLife);
            projectileCooldown = Time.timeSinceLevelLoad;
            audio.clip = laser;
            audio.pitch = Random.Range(0.7f, 1.2f);
            audio.Play();
            return true;
        }
        return false;
    }

    /* Checks the position of the player to ensure that the player buggy is always within the 
     * confinds of the player space */
    private void CheckPositioningConstraints()
    {
        if (transform.position.x > 5f)
        {
            transform.position = new Vector3(5f,
                transform.position.y,
                transform.position.z);
        }
        else if (transform.position.x < -5f)
        {
            transform.position = new Vector3(-5f,
                transform.position.y,
                transform.position.z);
        }

        if (transform.position.z != StartZ)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                StartZ);
        }
        // Folling is not longer needed as the rotation is now managed by the animator
        /*
        if (transform.eulerAngles.y > 20f && transform.eulerAngles.y < 180f) // I was expecting the rotation to be represented in positive and negative (as in the inspector) but its not
        {
            transform.eulerAngles = new Vector3(0f, 20f, 0f);
        }
        else if (transform.eulerAngles.y < 340f && transform.eulerAngles.y > 180f)
        {
            transform.eulerAngles = new Vector3(0f, -20f, 0f);
        }
        */
    }

    public void PlayGameOverAnimation()
    {
        anim.SetBool("GameOver", true);
    }
}
