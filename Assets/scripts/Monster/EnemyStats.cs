using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy/New Enemy")]

public class EnemyStats : ScriptableObject {
    
        new public string name = "New Card";
        public Sprite icon = null;
        public bool isDefaultItem = false;
        public int hitPoint;
        public int dmg;
        public int _movementDistance;
        public float moveSpeed;
}