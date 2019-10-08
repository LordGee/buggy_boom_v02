using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawnEnemy : MonoBehaviour {

    // Public Variables
    [Tooltip("Set the speed of the projectile")]
    public float projectileSpeed = 30f;
    [Tooltip("Value is SET by another Script")]
    public float projectileDamage;

    // Private Variables
    private GameObject player;
    private Vector3 originalPlayerPosision;
    private float projectileLife = 5f;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        originalPlayerPosision = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 1);
        Destroy(gameObject, projectileLife);
    }

    // Update is called once per frame
    void Update()
    {
        // https://docs.unity3d.com/ScriptReference/Vector3.MoveTowards.html
        transform.position = Vector3.MoveTowards(
            transform.position,
            originalPlayerPosision,
            projectileSpeed * Time.deltaTime);
        if (transform.position == originalPlayerPosision)
        {
            Destroy(gameObject);
        }
    }

    public float GetProjectileDamage()
    {
        return projectileDamage;
    }
}
