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
    private bool _mouseOver;
    [SerializeField] public Stack<Tile> _path;
    Vector3 heading;
    Vector3 velocity;
    [SerializeField] float moveSpeed = 4f;
    private HashSet<Tile> _visited;
    private List<Tile> _tilesInRange;
    private Queue<Tile> _tileQueue;


    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _originalColour = _renderer.material.color;
        _path = new Stack<Tile>();
        _visited = new HashSet<Tile>();
        _tilesInRange = new List<Tile>();
        _tileQueue = new Queue<Tile>();
    }

    private void Update()
    {
        UpdateColour();
        HighlightTilesInRange();

        if (HasValidTarget())
        {
            Move();

        }
        else {
            CheckMouseForTarget();
        }

    }

    private void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    private void SetHorizontalVelocity(){
        velocity = heading * moveSpeed;
    }

    private void Move()
    {
        ResetTiles();
        if (_path.Count > 0)
        {
            Tile t = _path.Peek();
            Vector3 target = new Vector3(t.transform.position.x, transform.position.y, t.transform.position.z);
            if (Vector3.Distance(transform.position, target) >= 0.5f)
            {
                CalculateHeading(target); 
                SetHorizontalVelocity();
                transform.forward = heading; // update player to look towards target
                transform.position += velocity * Time.deltaTime; // adjust position based on current velocity
            }
            else
            {
                if (_path.Count == 0) {
                    transform.position = target;
                }
                
                _path.Pop(); // remove the target from our path, we will reach the next one
            }

        }
    }


    private bool HasValidTarget() {
        return _path != null && _path.Count > 0;
    }

    private void HighlightTilesInRange()
    {
        if (_mouseOver || _selected) {
            MarkWalkable();
            return;
        }

        // any tiles in range should be reset if there is no player interaction
        // happening
        ResetTiles();
    }

    private void UpdateColour()
    {
        if (_selected)
        {
            _renderer.material.color = Color.blue;
        }
        else
        {
            _renderer.material.color = _originalColour;
        }
    }


    private void BuildPathFromTile(Tile destinationTile)
    {
        _path.Clear();
        Tile nextTile = destinationTile;
        
        while (nextTile.parent != null) {
            _path.Push(nextTile);
            nextTile = nextTile.parent;
        }

    }

    private void OnMouseOver()
    {
        _mouseOver = true;
    }

    private void OnMouseExit()
    {
        _mouseOver = false;
    }

    private void CheckMouseForTarget() {
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

                BuildPathFromTile(t);
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

    // Toggle selected on mouse click which will prompt
    // filling of the tiles as walkable.
    private void OnMouseDown()
    {
        _selected = !_selected;
    }


    // GetTilesInRange returns a list of all the tiles within _movementDistance
    // tiles of the character.
    private List<Tile> GetTilesInRange() {
        _visited.Clear();
        _tilesInRange.Clear();
        _tileQueue.Clear();

        Tile startingTile = GetCurrentTile();

        // keep track of every tile that we need to look at
        _tileQueue.Enqueue(startingTile);
       
        while (_tileQueue.Count > 0)
        {
            // only stop when there are no tiles left
            Tile t = _tileQueue.Dequeue();
            

            // don't look at any tiles that are outside of the movement range
            if (_visited.Contains(t) || t.distance > _movementDistance)
            {
                continue;
            }

            _visited.Add(t);

            var nextNeighbours = t.GetNeighbours();
            foreach (var n in nextNeighbours)
            {
                if (!_visited.Contains(n))
                {
                    // each tile is one further from the previous
                    n.distance = t.distance + 1;
                    n.parent = t;
                    _tileQueue.Enqueue(n);
                }
            }
        }



        _tilesInRange.AddRange(_visited);
        return _tilesInRange;
    }

    private void ResetTiles() {
        var allTilesInRange = GetTilesInRange();
        foreach (Tile t in allTilesInRange)
        {
            t.Reset();
        }
    }

    private void MarkWalkable() {
        var allTilesInRange = GetTilesInRange();
        foreach (Tile t in allTilesInRange)
        {
            t.walkable = true;
        }
    }

}
