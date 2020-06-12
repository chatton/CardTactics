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
        highlightPath(Color.green);
    }

    private void OnMouseExit()
    {
        print("OnMouseExit");
        highlightPath(Color.grey);
    }


    Tile getCurrentTile() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit)) {
       
            return hit.transform.gameObject.GetComponent<Tile>();
        }
        return null;
    }

    private void highlightPath(Color color) {
        Tile startingTile = getCurrentTile();

        List<Tile> toReset = new List<Tile>();
        Queue<Tile> q = new Queue<Tile>();
        q.Enqueue(startingTile);
        while (q.Count > 0) {
            Tile t = q.Dequeue();
            toReset.Add(t);

            // don't look at any tiles that are outside of the movement range
            if (t.visited || t.distance > _movementDistance)
            {
                t.visited = true;
                continue;
            }

            t.SetColour(color);

            var nextNeighbours = t.GetNeighbours();
            foreach (var n in nextNeighbours) {
                if (!n.visited)
                {
                    
                    n.distance = t.distance + 1;
                    q.Enqueue(n);
                }
            }
        }

        foreach (Tile t in toReset) {
            t.Reset();
        }
    }
}
