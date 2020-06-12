using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    private List<Tile> _neighbours;
    private Tile _parent;
    public bool visited = false;
    private MeshRenderer _renderer;

    public bool walkable;
    public int distance;

    // Start is called before the first frame update
    void Start()
    {
        _neighbours = buildNeighboursList();
        _renderer = GetComponent<MeshRenderer>();
        SetColour(Color.gray);
    }

    // Update is called once per frame
    void Update()
    {
        updateColour();    
    }

    void updateColour() {
        //if (debug) {
        //    _renderer.material.color = Color.cyan;
        //}

        //else if (walkable) {

        //    _renderer.material.color = Color.green;
        //}
        //else
        //{
        //    _renderer.material.color = Color.grey;

        //}
    }

    internal void SetColour(Color color)
    {
        _renderer.material.color = color;
    }


    public void Reset()
    {
        visited = false;
        walkable = false;
        distance = 0;
    }

    private Tile checkTile(Vector3 direction)
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

    public List<Tile> buildNeighboursList()
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
            Tile t = checkTile(v);
            if (t != null)
            {
                //t.debug = true
                tiles.Add(t);
            }
        }
        return tiles;
    }
}
