using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    // Use this for initialization

    private Queue<Team> _teams;

    void Start()
    {
        _teams = new Queue<Team>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
