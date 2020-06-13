using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExecuteCard
{
    public abstract void Execute();
}

public class Card : MonoBehaviour
{

    public CardItem cardItem;
    public bool attackSelect = false;

    [SerializeField] private Color _onMouseEnterColour;
    private Color _originalColour;
    private MeshRenderer _renderer;


    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _originalColour = _renderer.material.color;
        GetComponentInChildren<TextMesh>().text = (cardItem.name + "\n" + "Dmg: " + cardItem.dmg + "\n" + "Cost: " + cardItem.cost);
    }


    private void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = _originalColour;
    }

    private void OnMouseEnter()
    {
        _renderer.material.color = _onMouseEnterColour;
        Debug.Log("Name: " + cardItem.name);
    }

    private void OnMouseDown()
    {

    }

}