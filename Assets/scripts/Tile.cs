using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public bool visited = false;
    private MeshRenderer _renderer;

    public bool walkable;
    public int distance;
    public Tile parent;
    public bool partOfPath;

    // Start is called before the first frame update
    void Start()
    {
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
        else if (partOfPath) {
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


    public void Reset()
    {
        visited = false;
        walkable = false;
        parent = null;
        partOfPath = false;
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
                RaycastHit hit;
                if (!Physics.Raycast(t.transform.position, Vector3.up, out hit, 1))
                {
                    tiles.Add(t);
                }
               
            }
        }
        return tiles;
    }
}
