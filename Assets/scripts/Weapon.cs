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

    //public Weapon(string name, int damage, int range, int ammo) {
    //    this.name = name;
    //    this.damage = damage;
    //    this.range = range;
    //    this.ammo = ammo;
    //}


    //public void Attack(TacticsMove target) {

    //}
}
