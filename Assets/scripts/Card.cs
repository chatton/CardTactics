using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

    public CardItem cardItem;

    [SerializeField] private Color _onMouseEnterColour;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
    private Color _originalColor;
=======
    private Color originalColor;
>>>>>>> Stashed changes
=======
    private Color originalColor;
>>>>>>> Stashed changes
    private MeshRenderer _renderer;


    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        _originalColor = _renderer.material.color;
        GetComponentInChildren<TextMesh>().text=(cardItem.name + "\n" + "Dmg: " + cardItem.dmg + "\n" + "Cost: " + cardItem.cost);
=======
        originalColor = _renderer.material.color;
>>>>>>> Stashed changes
=======
        originalColor = _renderer.material.color;
>>>>>>> Stashed changes
    }


    private void OnMouseExit()
    {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        GetComponent<Renderer>().material.color = _originalColor;
=======
        GetComponent<Renderer>().material.color = originalColor;
>>>>>>> Stashed changes
=======
        GetComponent<Renderer>().material.color = originalColor;
>>>>>>> Stashed changes
    }
    private void OnMouseEnter()
    {
        _renderer.material.color = _onMouseEnterColour;
        Debug.Log("Name: " + cardItem.name);
    }

}
