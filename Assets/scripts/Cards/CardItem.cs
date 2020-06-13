using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/New Card")]
public class CardItem : ScriptableObject {
    
        new public string name = "New Card";
        public Sprite icon = null;
        public bool isDefaultItem = false;
        public int dmg;
        public int cost;
        public int range;
        public string text;
        public int text2;
}