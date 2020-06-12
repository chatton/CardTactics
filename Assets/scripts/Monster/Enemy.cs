using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy/New Enemy")]

public class Enemy : ScriptableObject {
    
        new public string name = "New Card";
        public Sprite icon = null;
        public bool isDefaultItem = false;
        public int hitPoint;
        public int dmg;
        public int range;
        public float speed;
}