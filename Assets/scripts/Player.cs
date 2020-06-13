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

                return BuildPathFromTile(t);
            }
        }
        return null;
    }
}
