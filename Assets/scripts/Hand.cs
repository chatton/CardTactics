using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

    private Card[] _cards;
    [Range(0, 1)]
    [SerializeField] float cardBufferDistance = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        _cards = FindObjectsOfType<Card>();
    }


    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _cards.Length; i++)
        {
            Card c = _cards[i];
            Vector3 screenPoint = Camera.main.ViewportToScreenPoint(new Vector3(0, 0, 3));
            Vector3 finalPos = Camera.main.ScreenToWorldPoint(screenPoint);
            finalPos.y += c.transform.lossyScale.y / 2;
            finalPos.x += c.transform.lossyScale.x / 2 * 0.75f + (i * 0.75f);

            c.transform.rotation = Camera.main.transform.rotation;
            c.transform.position = finalPos;
        }
    }
}
