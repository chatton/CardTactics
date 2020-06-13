using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Enemy : TacticsMove
{

    public EnemyStats enemyStats;
    public int enemyHP = 0;
    public GameObject explosion;


    private void Start()
    {
        //get HP
        enemyHP = enemyStats.hitPoint;

        //overwrite distance
        _movementDistance = enemyStats._movementDistance;

        //overwrite speed
        moveSpeed = enemyStats.moveSpeed;
        Debug.Log(_movementDistance);
        Debug.Log(moveSpeed);
        if (enemyHP <= 0)
        {
            Explode(transform.position);
            Destroy(gameObject);
        }
    }

    private void Update()
    {

    }

    public void Explode(Vector3 position)
    {
        Instantiate(explosion, position, Quaternion.identity);
        Debug.Log("BOOOOM");
    }

    public override void BuildPath()
    {
        throw new System.NotImplementedException();
    }
}
