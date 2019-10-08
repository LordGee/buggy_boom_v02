using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawn : MonoBehaviour
{
    // Public Variables
    [Tooltip("Speed at which the projectile travels")]
    public float projectileSpeed = 20f;

    // Private Variables
    private GameObject buggyRotation;
    private Vector3 originalForward;

    // Use this for initialization
    void Start()
    {
        buggyRotation = GameObject.FindGameObjectWithTag("Player");
        originalForward = buggyRotation.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(originalForward * projectileSpeed * Time.deltaTime);
    }
}
