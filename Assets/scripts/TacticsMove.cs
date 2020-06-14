﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// this class is the base class for Player and AI movement
[RequireComponent(typeof(TurnManager))]
public abstract class TacticsMove : MonoBehaviour {


    public TurnManager _turnManager;

    [SerializeField] protected int _movementDistance = 5;
    [SerializeField] protected bool _selected;
    protected Color _originalColour;
    protected Renderer _renderer;
    protected bool _mouseOver;
    [SerializeField] protected Stack<Tile> _path;
    protected Vector3 heading;
    protected Vector3 velocity;
    [SerializeField] protected float moveSpeed = 4f;
    protected HashSet<Tile> _visited;
    protected List<Tile> _tilesInRange;
    protected Queue<Tile> _tileQueue;
    [SerializeField] private bool _moving;

    [SerializeField] protected List<Tile> _selectableTiles;


    public void Awake()
    {
        _turnManager = FindObjectOfType<TurnManager>();
        _turnManager.RegisterTeam(gameObject.tag);
    }

    public void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _originalColour = _renderer.material.color;
        _path = new Stack<Tile>();
        _visited = new HashSet<Tile>();
        _tilesInRange = new List<Tile>();
        _tileQueue = new Queue<Tile>();
    }

    public void Update()
    {
        UpdateColour();

        if (!_moving)
        {

            string currentTeam = _turnManager.GetActiveTeam();
            if (gameObject.tag != currentTeam)
            {
                return;
            }

            FindSelectableTiles();
            CheckMouse();
        }
        else {
            Move();
        }

    }


    private void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    private void SetHorizontalVelocity() {
        velocity = heading * moveSpeed;
    }

    private void Move()
    {
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
                 transform.position = target;
                _path.Pop(); // remove the target from our path, we will reach the next one
            }
        }
        else {
            _moving = false;
            RemoveSelectableTiles();
            _turnManager.NextTurn();
            
        }
    }

    private void RemoveSelectableTiles() {
        foreach (var t in _selectableTiles) {
            t.Reset();
        }
        _selectableTiles.Clear();
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

    private void OnMouseOver()
    {
        _mouseOver = true;
    }

    private void OnMouseExit()
    {
        _mouseOver = false;
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
        string currentTeam = _turnManager.GetActiveTeam();
        if (gameObject.tag != currentTeam)
        {
            return;
        }
        _selected = !_selected;
    }

    public void CheckMouse()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Tile t = hit.collider.GetComponent<Tile>();
                if (t != null) {
                    MoveToTile(t);
                }
            }
        }
    }

    private void MoveToTile(Tile t)
    {
        _path.Clear();
        _moving = true;
        Tile nextTile = t;
        while (nextTile.parent != null)
        {
            _path.Push(nextTile);
            nextTile = nextTile.parent;
        }
    }

    // GetTilesInRange returns a list of all the tiles within _movementDistance
    // tiles of the character.
    private void FindSelectableTiles() {
        _visited.Clear();
        _tileQueue.Clear();
        _selectableTiles.Clear();

        Tile startingTile = GetCurrentTile();

        // keep track of every tile that we need to look at
        _tileQueue.Enqueue(startingTile);
       
        while (_tileQueue.Count > 0)
        {
            // only stop when there are no tiles left
            Tile t = _tileQueue.Dequeue();

            // don't look at any tiles that are outside of the movement range
            if (_visited.Contains(t))
            {
                continue;
            }

            _visited.Add(t);

            var nextNeighbours = t.GetNeighbours();
            foreach (var n in nextNeighbours)
            {
                if (!_visited.Contains(n) && t.distance <= _movementDistance)
                {
                    // each tile is one further from the previous
                    n.distance = t.distance + 1;
                    n.parent = t;
                    n.walkable = true;
                    _tileQueue.Enqueue(n);
                }
            }
        }

        _selectableTiles.AddRange(_visited);
    }

}
