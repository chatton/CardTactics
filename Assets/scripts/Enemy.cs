using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Enemy : TacticsMove
{

    public EnemyStats enemyStats;
    public int enemyHP;


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

    }

    private void Update()
    {
        if (enemyHP <=0 )
        {
            Destroy(gameObject);
        }
    }

    public override void BuildPath()
    {
        throw new System.NotImplementedException();
    }
}
