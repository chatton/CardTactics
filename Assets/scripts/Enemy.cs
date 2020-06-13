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


    public void Start()
    {
        base.Start();
        //get HP
        // enemyHP = enemyStats.hitPoint;

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
        base.Update();
        Instantiate(explosion, position, Quaternion.identity);
        Debug.Log("BOOOOM");
        
        if (enemyHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    public override Stack<Tile> BuildPath()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Tile t = hit.collider.GetComponent<Tile>();
                if (t == null)
                {
                    return null;
                }

                if (t.parent == null)
                {
                    return null;
                }

                if (t.distance > _movementDistance)
                {
                    return null;
                }

                BuildPathFromTile(t);
            }
        }
        return null;
    }
}
