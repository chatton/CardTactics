using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[RequireComponent(typeof(HealthBar))]
public class Enemy : TacticsMove
{

    public EnemyStats enemyStats;
    public int enemyHP = 0;
    public GameObject explosion;

    public HealthBar healthBar;

    public void Awake()
    {
        base.Awake();
        healthBar = GetComponent<HealthBar>();
    }

    public void Start()
    {
        base.Start();
        //get HP
        healthBar.MaxHealth = enemyStats.hitPoint;

        //overwrite distance
        _movementDistance = enemyStats._movementDistance;

        //overwrite speed
        moveSpeed = enemyStats.moveSpeed;

    }

    public void OnMouseDown()
    {
        healthBar.TakeDamage(10);
    }

    public void Update()
    {
        base.Update();
        if (healthBar.currentHealth <= 0)
        {
            Explode(transform.position);
            Destroy(gameObject);
        }
    }

    public void Explode(Vector3 position)
    {
        Instantiate(explosion, position, Quaternion.identity);
        Debug.Log("BOOOOM");
        if (healthBar.currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

   

}
