using UnityEngine;
using System.Collections;

public class Player : TacticsMove
{
    [SerializeField] private int playerHitPoint;
    [SerializeField] private int playerAttack;
    public override void BuildPath()
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
                    return;
                }

                if (t.parent == null)
                {
                    return;
                }

                if (t.distance > _movementDistance)
                {
                    return;
                }

                BuildPathFromTile(t);
            }
        }
    }
}
