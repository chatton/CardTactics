using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{



    [SerializeField] private Color _onMouseEnterColour;
    private Color originalColor;
    private MeshRenderer _renderer;


    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        originalColor = _renderer.material.color;
    }


    private void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = originalColor;
    }
    private void OnMouseEnter()
    {
        _renderer.material.color = _onMouseEnterColour;
    }

}
