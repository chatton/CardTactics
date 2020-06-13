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

    public void Explode(Vector3 position)
    {
        Instantiate(explosion, position, Quaternion.identity);
        Debug.Log("BOOOOM");
        if (enemyHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    public override Stack<Tile> BuildPath()
    {
        return new Stack<Tile>();
        //    if (Input.GetMouseButtonUp(0))
        //    {
        //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //        RaycastHit hit;
        //        if (Physics.Raycast(ray, out hit))
        //        {
        //            Tile t = hit.collider.GetComponent<Tile>();
        //            if (t == null)
        //            {
        //                print("1");
        //                return new Stack<Tile>();
        //            }

        //            if (t.parent == null)
        //            {
        //                print("2");
        //                return new Stack<Tile>();
        //            }

        //            if (t.distance > _movementDistance)
        //            {
        //                print("3");
        //                return new Stack<Tile>();
        //            }

        //            print("4");
        //            return BuildPathFromTile(t);
        //        }
        //    }
        //    print("5");
        //    return new Stack<Tile>();
        //}
    }
}
