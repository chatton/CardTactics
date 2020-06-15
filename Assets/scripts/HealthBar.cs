using UnityEngine;

public class HealthBar : MonoBehaviour
{

    public int MaxHealth;
    public int currentHealth;
    public RectTransform healthbar;
    // Update is called once per frame

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            currentHealth = 0;
        }
    }

    void Update()
    {
        healthbar.sizeDelta = new Vector2(currentHealth * 2, healthbar.sizeDelta.y);
    }
}
