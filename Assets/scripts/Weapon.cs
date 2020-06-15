using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon/New Weapon")]
public class Weapon : ScriptableObject
{
    private static readonly int INFINITE_AMMO = -1;

    [SerializeField] public int range;
    [SerializeField] public int damage;
    [SerializeField] public string name;
    [SerializeField] public int ammo;


    public bool IsLoaded() {
        if (ammo == INFINITE_AMMO) {
            return true;
        }
        return ammo > 0;
    }

    // Attack deals damage to a HealthBar, this function
    // returns a boolean indicating if the target was killed with the attack
    public bool Attack(HealthBar hb) {
        hb.TakeDamage(damage);
        if (ammo != INFINITE_AMMO) {
            ammo--;
        }
        return hb.currentHealth <= 0;
    }

}
