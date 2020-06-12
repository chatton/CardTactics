using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TacticsMove : MonoBehaviour {


    [SerializeField] private int _movementDistance = 5;
    
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnMouseOver()
    {
        highlightPath();
    }


    Tile getCurrentTile() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit)) {
       
            return hit.transform.gameObject.GetComponent<Tile>();
        }
        return null;
    }

    private void highlightPath() {

        Tile startingTile = getCurrentTile();
        startingTile.walkable = true;
     
        Queue<Tile> q = new Queue<Tile>();
        q.Enqueue(startingTile);
        while (q.Count > 0) {
            Tile t = q.Dequeue();

            // don't look at any tiles that are outside of the movement range
            if (t.visited || t.distance > _movementDistance)
            {
                t.visited = true;
                continue;
            }

            //if (t.visited) {
                //continue;
            //}

            t.visited = true;
            t.walkable = true;
            

            var nextNeighbours = t.GetNeighbours();
            foreach (var n in nextNeighbours) {
                if (!n.visited)
                {
                    
                    n.distance = t.distance + 1;
                    q.Enqueue(n);
                }
            }
        }
    }
}
