using System;
using System.Collections.Generic;
using System.Linq;
public class Deck
{
    
    private readonly Stack<Card> _discard;
    private readonly Stack<Card> _cards;

    public Deck(Card[] cards)
    {
        _cards = new Stack<Card>();
        _discard = new Stack<Card>();
        foreach (Card c in cards) {
            _cards.Push(c);
        }
    }


    public List<Card> drawHand(int handSize) {
        var list = new List<Card>();
        for(int i = 0; i < handSize; i++)
        {
            if (_cards.Count == 0) {
                shuffleDiscard();
            }
            Card c = _cards.Pop();
            _discard.Push(c);
            list.Add(c);
        }
        return list;
    }

    // shuffle all of the discard pile into the cards
    private void shuffleDiscard()
    {
        var values = _discard.ToArray();
        _discard.Clear();
        _cards.Clear();
        var rnd = new Random();
        foreach (var value in values.OrderBy(x => rnd.Next())){
            _cards.Push(value);
        }
    }
}
