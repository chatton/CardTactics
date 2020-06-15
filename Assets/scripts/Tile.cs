using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    [SerializeField] public bool visited = false;
    [SerializeField] public int distance = 0;
    [SerializeField] public Tile parent = null;
    public bool walkable;
    public bool inAttackRange;

}


public class Tile : MonoBehaviour
{
    private MeshRenderer _renderer;
    private TurnManager turnManager;

    [SerializeField] public Dictionary<TacticsMove, State> pathFindingState;


    private State GetState(TacticsMove tm) {

        if (!pathFindingState.ContainsKey(tm)) {
            pathFindingState.Add(tm, new State());
        }
        return pathFindingState[tm];
    }

    public void SetDistance(TacticsMove tm, int distance) {
        var state = GetState(tm);
        state.distance = distance;
    }

    public int GetDistance(TacticsMove tm) {
        var state = GetState(tm);
        return state.distance;
    }


    public void SetParent(TacticsMove tm, Tile parentTile)
    {
        var state = GetState(tm);
        state.parent = parentTile;
    }

    public Tile GetParent(TacticsMove tm)
    {
        var state = GetState(tm);
        return state.parent;
    }

    

    // Start is called before the first frame update
    void Start()
    {
        pathFindingState = new Dictionary<TacticsMove, State>();
        _renderer = GetComponent<MeshRenderer>();
        SetColour(Color.gray);
    }

    private void Awake()
    {
        turnManager = FindObjectOfType<TurnManager>();
    }

    private void Update()
    {
        UpdateColour();
    }

    private void UpdateColour()
    {

        TacticsMove currentPlayer = turnManager.GetActivePlayer();
        State state = GetState(currentPlayer);

       if (state.walkable) {
            SetColour(Color.green);
        }
        else if (state.inAttackRange)
        {
            SetColour(Color.red);
        }
        else
        {
            SetColour(Color.gray);
        }
    }

    internal void SetColour(Color color)
    {
        _renderer.material.color = color;
    }


    public void Reset(TacticsMove tacticsMove)
    {

        State state = GetState(tacticsMove);
        state.visited = false;
        state.parent = null;
        state.distance = 0;
        state.walkable = false;
        state.inAttackRange = false;
    }

    private Tile CheckTile(Vector3 direction)
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, new Vector3(0.25f, 0, 0.25f));
        foreach (Collider col in colliders)
        {   
            Tile tile = col.GetComponent<Tile>();
            if (tile != null)
            {
                return tile;
            }
        }
        return null;
    }

    public List<Tile> GetNeighbours()
    {
        return GetNeighbours(false);
    }

    public List<Tile> GetNeighbours(bool ignoreOccupiedTiles)
    {
        Vector3[] checkVectors = new[] {
            Vector3.forward,
            -Vector3.forward,
            Vector3.right,
            -Vector3.right,
        };

        var tiles = new List<Tile>();
        foreach (Vector3 v in checkVectors)
        {
            Tile t = CheckTile(v);
            if (t != null)
            {
                if (!ignoreOccupiedTiles)
                {
                    RaycastHit hit;
                    if (!Physics.Raycast(t.transform.position, Vector3.up, out hit, 1))
                    {
                        tiles.Add(t);
                    }
                }
                else {
                    tiles.Add(t);
                }
              

            }
        }
        return tiles;
    }

    internal bool IsReachableBy(TacticsMove tacticsMove)
    {
        var state = GetState(tacticsMove);
        return state.inAttackRange;
    }

    public TacticsMove GetUnitOnTile() {
        RaycastHit hit;
        bool hasUnit = Physics.Raycast(transform.position, Vector3.up, out hit, 1);
        if (hasUnit) {
            Player p = hit.transform.gameObject.GetComponent<Player>();
            if (p != null)
            {
                return p;
            }

            Enemy e = hit.transform.gameObject.GetComponent<Enemy>();
            if (e != null)
            {
                return e;
            }

        }
        return null;
    }

    public void SetInAttackRange(TacticsMove tacticsMove)
    {
        var state = GetState(tacticsMove);
        state.inAttackRange = true;
    }

    public void SetWalkable(TacticsMove tacticsMove)
    {
        var state = GetState(tacticsMove);
        state.walkable = true;
    }
}
