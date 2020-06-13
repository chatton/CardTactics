using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// this class is the base class for Player and AI movement
public abstract class TacticsMove : MonoBehaviour {


    [SerializeField] private int _movementDistance = 5;
    [SerializeField] private bool _selected;
    private bool _pathIsHighlighted;
    private Color _originalColour;
    private Renderer _renderer;

    private void Update()
    {
        //if (_selected) {
        //    _pathIsHighlighted = true;
        //    highlightPath(Color.green);
        //}
    }

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _originalColour = _renderer.material.color;
    }

    private void OnMouseOver()
    {
        // display the movable tiles in green
        highlightPath(Color.green);
    }

    private void OnMouseExit()
    {
        // reset the tiles to hide the generated path
        if (!_selected) {
            highlightPath(Color.grey);
        }
        
    }


    private Tile GetCurrentTile() {
        // shoot a ray vertically downwards to check for current tile
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit)) {
       
            return hit.transform.gameObject.GetComponent<Tile>();
        }
        return null;
    }

    private void OnMouseDown()
    {
        _selected = !_selected;
        if (_selected) {
            _renderer.material.color = Color.blue;
        } else {
            _renderer.material.color = _originalColour;
        }
        highlightPath(Color.green);
    }

    private void highlightPath(Color color) {
        Tile startingTile = GetCurrentTile();

        // keep track of every tile that we've updated
        List<Tile> toReset = new List<Tile>();

        // keep track of every tile that we need to look at
        Queue<Tile> q = new Queue<Tile>();
        q.Enqueue(startingTile);
        while (q.Count > 0) { // only stop when there are no tiles left
            Tile t = q.Dequeue();
            toReset.Add(t);

            // don't look at any tiles that are outside of the movement range
            if (t.visited || t.distance > _movementDistance)
            {
                t.visited = true;
                continue;
            }

            // we are marking this tile with the given colour
            t.SetColour(color);

            var nextNeighbours = t.GetNeighbours();
            foreach (var n in nextNeighbours) {
                if (!n.visited)
                {
                    // each tile is one further from the previous
                    n.distance = t.distance + 1;
                    n.parent = t;
                    q.Enqueue(n);
                }
            }
        }

        // reset every tile that was looked at in this search
        foreach (Tile t in toReset) {
            t.Reset();
        }
    }

    public List<Tile> getPath()
    {
        List<Tile> path = new List<Tile>();
        return path;
    }
}
