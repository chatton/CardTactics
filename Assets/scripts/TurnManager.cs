using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TurnManager : MonoBehaviour
{
    // Use this for initialization
    private Queue<TacticsMove> _players;

    public void NextTurn() {
        _players.Enqueue(_players.Dequeue());      
    }

    public TacticsMove GetActivePlayer() {
        return _players.Peek();
    }

    public void RegisterPlayer(TacticsMove player)
    {
        if (_players == null) {
            _players = new Queue<TacticsMove>();
        }
        if (!_players.Contains(player)) {
            _players.Enqueue(player);
        }
    }

}
