using UnityEngine;
using System.Collections.Generic;
using System;

// this class is the base class for Player and AI movement
[RequireComponent(typeof(TurnManager))]
public abstract class TacticsMove : MonoBehaviour
{


    private TurnManager _turnManager;

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
    protected bool foundTiles = false;

    [SerializeField] protected List<Tile> _selectableTiles;
    [SerializeField] protected List<Weapon> weapons;
    protected Weapon selectedWeapon;
    protected bool hasMoved;
    [SerializeField] List<TacticsMove> targets;


    public void Awake()
    {
        _turnManager = FindObjectOfType<TurnManager>();
        _turnManager.RegisterPlayer(this);
    }

    public void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _originalColour = _renderer.material.color;
        _path = new Stack<Tile>();
        _visited = new HashSet<Tile>();
        _tilesInRange = new List<Tile>();
        _tileQueue = new Queue<Tile>();
        selectedWeapon = weapons[0];
    }

    public void EndTurn()
    {
        RemoveSelectableTiles();
        _turnManager.NextTurn();
        _renderer.material.color = Color.grey;
        _moving = false;
        hasMoved = false;
    }


    public void Update()
    {
        // nothing to do unless it is our turn
        if (_turnManager.GetActivePlayer() != this)
        {
            return;
        }


        UpdateColour();
        UpdateMovement();
        UpdateAction();
    }

    private void UpdateMovement() {

        // we can no longer move if we've already done it once this turn
        if (hasMoved) {
            HighlightAttackRange();
            return;
        }


        if (!_moving)
        {
            if (!foundTiles)
            {
                FindSelectableTiles();
                foundTiles = true;
            }

            if (CheckMouse())
            {
                // need to find selectable tiles again
                foundTiles = false;
            }
        }
        else
        {
            Move();
        }
    }


    private void UpdateAction()
    {
        if (hasMoved)
        {
            if (PerformAction())
            {
                EndTurn();
            }

        }

    }


    public bool PerformAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                HealthBar hb = hit.transform.GetComponent<HealthBar>();
                if (hb == null)
                {
                    return false;
                }

                // we've clicked on ourselves, potentially no valid moves, end turn.
                if (hb == GetComponent<HealthBar>()) {
                    return true;
                }

                // the Tile that the unit that was just clicked on is standing on
                Tile targetTile = GetTileAtPosition(hit.transform.position);
               
                // if the target is within our attack range, and we have ammo, we can attack
                if (targetTile.IsReachableBy(this) && selectedWeapon.IsLoaded()) {
                    selectedWeapon.Attack(hb);
                    print("HIT: " + targetTile.name +". HP left: " + hb.currentHealth +"/" + hb.MaxHealth);
                    return true;
                }
                return false;
                
            }
        }
        return false;
    }


    private void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    private void SetHorizontalVelocity()
    {
        velocity = heading * moveSpeed;
    }

    private void Move()
    {

        TacticsMove currentPlayer = _turnManager.GetActivePlayer();
        if (currentPlayer != this)
        {
            return;
        }

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
        else
        {
            _moving = false;
            hasMoved = true;
            RemoveSelectableTiles();
            
        }
    }

    private void RemoveSelectableTiles()
    {
        foreach (var t in _selectableTiles)
        {
            t.Reset(this);
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

    private Tile GetCurrentTile()
    {
        return GetTileAtPosition(transform.position);
    }

    private Tile GetTileAtPosition(Vector3 pos) {
        // shoot a ray vertically downwards to check for current tile
        RaycastHit hit;
        if (Physics.Raycast(pos, Vector3.down, out hit))
        {

            return hit.transform.gameObject.GetComponent<Tile>();
        }
        return null;
    }

    // Toggle selected on mouse click which will prompt
    // filling of the tiles as walkable.
    private void OnMouseDown()
    {
        TacticsMove currentPlayer = _turnManager.GetActivePlayer();
        if (currentPlayer != this)
        {
            return;
        }
        _selected = !_selected;
    }

    public bool CheckMouse()
    {

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Tile t = hit.collider.GetComponent<Tile>();
                if (t != null)
                {
                    if (t.GetDistance(this) > _movementDistance || t.GetDistance(this) == 0)
                    {
                        return false;
                    }

                    MoveToTile(t);
                    return true;
                }
            }
        }
        return false;
    }

    private void MoveToTile(Tile t)
    {
        _path.Clear();
        _moving = true;
        Tile nextTile = t;
        while (nextTile.GetParent(this) != null)
        {
            _path.Push(nextTile);
            nextTile = nextTile.GetParent(this);
        }
    }


    private void HighlightAttackRange()
    {
        var visited = new HashSet<Tile>();
        var tileQueue = new Queue<Tile>();

        Tile startingTile = GetCurrentTile();

        // keep track of every tile that we need to look at
        tileQueue.Enqueue(startingTile);

        while (tileQueue.Count > 0)
        {
            // only stop when there are no tiles left
            Tile t = tileQueue.Dequeue();
            visited.Add(t);

            var nextNeighbours = t.GetNeighbours(true);
            foreach (var n in nextNeighbours)
            {
                if (visited.Contains(n))
                {
                    continue;
                }

                var dist = t.GetDistance(this);
                if (dist > selectedWeapon.range)
                {
                    continue;
                }

                // it's possible to shoot here
                if (dist < selectedWeapon.range)
                {
                    n.SetInAttackRange(this);
                }

                // each tile is one further from the previous
                n.SetParent(this, t);
                n.SetDistance(this, t.GetDistance(this) + 1);
                tileQueue.Enqueue(n);
            }
        }

    }


    private void FindSelectableTiles()
    {
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
            _visited.Add(t);

            var nextNeighbours = t.GetNeighbours();
            foreach (var n in nextNeighbours)
            {
                if (_visited.Contains(n))
                {
                    continue;
                }

                var dist = t.GetDistance(this);

                // the tile is out of range of movement and as a possible weapon target
                //if (dist > _movementDistance + selectedWeapon.range) {
                //    continue;
                //}

                // it's possible to walk here
                if (dist < _movementDistance)
                {
                    n.SetWalkable(this);
                }

                // it's possible to shoot here
                //if (dist < _movementDistance + selectedWeapon.range)
                //{
                //    n.SetInAttackRange(this);
                //}

                // each tile is one further from the previous
                n.SetParent(this, t);
                n.SetDistance(this, t.GetDistance(this) + 1);
                _tileQueue.Enqueue(n);
            }
        }


        _selectableTiles.AddRange(_visited);
    }
}
