using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : TacticsMove
{
    [SerializeField] private int playerHitPoint;
    [SerializeField] private int playerAttack;


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
                    return new Stack<Tile>();
                }

                if (t.parent == null)
                {
                    return new Stack<Tile>();
                }

                if (t.distance > _movementDistance)
                {
                    return new Stack<Tile>();
                }

                return BuildPathFromTile(t);
            }
        }
        return new Stack<Tile>();
    }
}
