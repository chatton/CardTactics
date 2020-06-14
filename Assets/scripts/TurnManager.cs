using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TurnManager : MonoBehaviour
{
    // Use this for initialization
    private Queue<string> _teams;

    public void NextTurn() {
        _teams.Enqueue(_teams.Dequeue());      
    }

    public string GetActiveTeam() {
        return _teams.Peek();
    }

    public void RegisterTeam(string tag)
    {
        if (_teams == null) {
            _teams = new Queue<string>();
        }
        if (!_teams.Contains(tag)) {
            _teams.Enqueue(tag);
        }
    }
}
