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
    [SerializeField] private Tile _destinationTile;
    [SerializeField] List<Tile> path;

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _originalColour = _renderer.material.color;
    }

    private void Update()
    {
        UpdateColour();
        HighlightTilesInRange();

        if (HasValidTarget())
        {
            SetDestinationTile();
            Move();

        }
        else {
            CheckMouseForTarget();
        }
        
    }

    private void SetDestinationTile()
    {

        path = BuildPathFromTile(_destinationTile);
        ResetTiles();

        if (path.Count == 0)
        {
            _destinationTile = null;
            return;
        }

        // TODO: teleporting to last tile for now
        _destinationTile = path[path.Count - 1]; 
    }



    private void Move()
    {
        var destinationPos = _destinationTile.transform.position;
        transform.position = new Vector3(destinationPos.x, transform.position.y, destinationPos.z);
    }


    private bool HasValidTarget() {
        return _destinationTile != null && _destinationTile.distance <= _movementDistance;
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



    //public IEnumerator MoveOverSpeed(GameObject objectToMove, Vector3 end, float speed)
    //{
    //    // speed should be 1 unit per second
    //    while (objectToMove.transform.position != end)
    //    {
    //        objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, end, speed * Time.deltaTime);
    //        yield return new WaitForEndOfFrame();
    //    }
    //}
    //public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    //{
    //    float elapsedTime = 0;
    //    Vector3 startingPos = objectToMove.transform.position;
    //    while (elapsedTime < seconds)
    //    {
    //        objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
    //        elapsedTime += Time.deltaTime;
    //        yield return new WaitForEndOfFrame();
    //    }
    //    objectToMove.transform.position = end;
    //}

    private List<Tile> BuildPathFromTile(Tile destinationTile)
    {
        List<Tile> path = new List<Tile>();
        Tile nextTile = destinationTile;
        
        while (nextTile.parent != null) {
            path.Add(nextTile);
            nextTile = nextTile.parent;
        }

        path.Reverse();

        return path;
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

    // Toggle selected on mouse click which will prompt
    // filling of the tiles as walkable.
    private void OnMouseDown()
    {
        _selected = !_selected;
    }


    // GetTilesInRange returns a list of all the tiles within _movementDistance
    // tiles of the character.
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
