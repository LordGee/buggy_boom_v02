using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCObstacle : MonoBehaviour {

    //Public Variables
    
    [Tooltip("Gameobject instantiated and provides effects when the object collides with the player")]
    public GameObject DeathParticleEffect;

    // Private Variables
    private GameControlScript gameControl;
    private float npcHealth;
    public float npcDamage;
    private float npcSpeed;
    private float npcPoints;
    private float changerLane;

    void Start()
    {
        gameControl = FindObjectOfType<GameControlScript>();
        npcHealth = gameControl.GetNpcHealth();
        npcDamage = gameControl.GetNpcDamage();
        npcSpeed = gameControl.GetNpcSpeed();
        npcPoints = gameControl.GetNpcPoints();
        changerLane = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(changerLane, 0f, -(npcSpeed * Time.deltaTime));
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            NpcDeathEffect();
            gameControl.DamagePlayer(npcDamage);
            gameControl.AddPoints(npcSpeed);
            Destroy(this.gameObject, 0.1f); // added this small amount of time makes such a difference
        }
        else if (col.gameObject.tag == "Projectile")
        {
            gameControl.DamageNPC(this.gameObject, npcPoints, gameControl.GetPlayerDamage(), ref npcHealth);
            Destroy(col.transform.parent.gameObject);
        }
    }

    void NpcDeathEffect()
    {
        GameObject death = Instantiate(DeathParticleEffect, transform.position, Quaternion.identity);
        Destroy(death, 3f);
    }

    public void ChangeLane(float _newX)
    {
        changerLane = _newX;
    }
}
