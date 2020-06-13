using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// this class is the base class for Player and AI movement
public abstract class TacticsMove : MonoBehaviour {


    [SerializeField] private int _movementDistance = 5;
    [SerializeField] private bool _selected;
    private Color _originalColour;
    private Renderer _renderer;
    [SerializeField] private Tile _destinationTile;

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _originalColour = _renderer.material.color;
    }

    private void Update()
    {

        if (_selected) {
            highlightPath(true);
        }

        if (_destinationTile != null && _destinationTile.distance <= _movementDistance)
        {
            Move();
        }
        else {
            CheckMouse();
        }
        
    }

    private void Move()
    {
       

        List<Tile> path = BuildPathFromTile(_destinationTile);
        highlightPath(false);

        if (path.Count == 0) {
            _destinationTile = null;
            return;
        }

        _destinationTile = path[0];
        var destinationPos = _destinationTile.transform.position;
        transform.position = new Vector3(destinationPos.x, transform.position.y, destinationPos.z);
        _destinationTile = null;

        foreach (Tile t in path) {
            t.Reset();
        }
    }

    private List<Tile> BuildPathFromTile(Tile destinationTile)
    {
        List<Tile> path = new List<Tile>();
        Tile nextTile = destinationTile;
        
        while (nextTile.parent != null) {
            path.Add(destinationTile);
            nextTile = nextTile.parent;
        }

        path.Reverse();

        return path;
    }

    private void OnMouseOver()
    {
        // display the movable tiles in green
        highlightPath(true);
    }

    private void OnMouseExit()
    {
        // reset the tiles to hide the generated path
        if (!_selected) {
            highlightPath(false);
        }
        
    }

    private void CheckMouse() {
        if (Input.GetMouseButtonUp(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                Tile t = hit.collider.GetComponent<Tile>();
                if (t == null) {
                    return;
                }

                if (t.parent == null) {
                    return;
                }

                _destinationTile = t;
            }
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
        highlightPath(true);
    }


    private List<Tile> GetTilesInRange() {

        HashSet<Tile> visited = new HashSet<Tile>();
        Tile startingTile = GetCurrentTile();

        // keep track of every tile that we need to look at
        Queue<Tile> q = new Queue<Tile>();
        q.Enqueue(startingTile);
       
        while (q.Count > 0)
        {
            // only stop when there are no tiles left
            Tile t = q.Dequeue();
            

            // don't look at any tiles that are outside of the movement range
            if (visited.Contains(t) || t.distance > _movementDistance)
            {
                continue;
            }

            visited.Add(t);

            var nextNeighbours = t.GetNeighbours();
            foreach (var n in nextNeighbours)
            {
                if (!visited.Contains(n))
                {
                    // each tile is one further from the previous
                    n.distance = t.distance + 1;
                    n.parent = t;
                    q.Enqueue(n);
                }
            }
        }

        return new List<Tile>(visited);
    }

    private void highlightPath(bool walkable)
    {
        var allTilesInRange = GetTilesInRange();
        foreach (Tile t in allTilesInRange) {
            if (!walkable) {
                t.Reset();
            } else {
                t.walkable = true;
            }
            
        }
    }
}
