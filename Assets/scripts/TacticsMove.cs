using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TacticsMove : MonoBehaviour {


    [SerializeField] private int _movementDistance = 5;
    
    // Use this for initialization
    void Start()
    {
        getCurrentTile().walkable = true;
    }

    // Update is called once per frame
    void Update()
    {
        findPath();
    }


    private void OnMouseOver()
    {
        highlightPath();
    }

    private void highlightPath()
    {
        print("Highlighting path!");
        var path = findPath();
        foreach (Tile t in path) {
            t.walkable = true;
        }
    }

    Tile getCurrentTile() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit)) {
       
            return hit.transform.gameObject.GetComponent<Tile>();
        }
        return null;
    }

    private List<Tile> findPath() {

        Tile startingTile = getCurrentTile();
        
        List<Tile> neighbours = startingTile.GetNeighbours();
        int currentDepth = 0;
        Queue<Tile> q = new Queue<Tile>();
        q.Enqueue(startingTile);
        startingTile.visited = true;
        startingTile.walkable = true;

        while (q.Count > 0) {
            Tile t = q.Dequeue();
            t.visited = true;
            t.walkable = true;

            if (!t.visited) {
                var nextNeighbours = t.GetNeighbours();
                foreach (var n in neighbours) {
                    q.Enqueue(t);
                    t.visited = true;
                    t.walkable = true;
                }
            }

        }

        return neighbours;
    }

}
