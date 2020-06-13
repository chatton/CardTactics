using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterClass 
{

    private string _name;
    private List<Card> _defaultDeck;
    public CharacterClass(string name, List<Card> defaultDeck) {
        _name = name;
        _defaultDeck = new List<Card>(defaultDeck);
    }
}
