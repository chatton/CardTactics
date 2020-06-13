using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    private List<Tile> _neighbours;
    public bool visited = false;
    private MeshRenderer _renderer;

    public bool walkable;
    public int distance;
    public Tile parent;

    // Start is called before the first frame update
    void Start()
    {
        _neighbours = BuildNeighboursList();
        _renderer = GetComponent<MeshRenderer>();
        SetColour(Color.gray);
    }


    private void Update()
    {
        UpdateColour();
    }

    private void UpdateColour()
    {
        if (walkable)
        {
            SetColour(Color.green);
        }
        else {
            SetColour(Color.gray);
        }
    }

    internal void SetColour(Color color)
    {
        _renderer.material.color = color;
    }


    public void Reset()
    {
        visited = false;
        walkable = false;
        parent = null;
        distance = 0;
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
        return _neighbours;
    }

    public List<Tile> BuildNeighboursList()
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
                //t.debug = true
                tiles.Add(t);
            }
        }
        return tiles;
    }
}
