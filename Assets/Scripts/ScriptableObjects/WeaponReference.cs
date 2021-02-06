using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Match Attack/WeaponReference")]
public class WeaponReference : ItemReference
{

    public int damage;

    public bool requiresAmmo => ammo.Count > 0;

    public List<AmmoReference> ammo;

}
