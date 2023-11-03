using System;
using UnityEngine;

[Serializable]
public struct Tile
{

    public string name;

    public Sprite sprite;

    public TileType type;

    public int damage;

    public bool requiresAmmo;

    public bool requiresSpecialAmmo;

    public override bool Equals(object obj)
    {

        if (!(obj is Tile))
        {
            return false;
        }

        var otherTile = (Tile)obj;

        return otherTile.sprite == sprite && otherTile.name == name;

    }

    public bool Equals(Tile other)
    {
        return name == other.name && Equals(sprite, other.sprite) && type == other.type && damage == other.damage &&
               requiresAmmo == other.requiresAmmo && requiresSpecialAmmo == other.requiresSpecialAmmo;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(name, sprite, (int)type, damage, requiresAmmo, requiresSpecialAmmo);
    }

}

[Serializable]
public enum TileType
{

    Weapon,

    Ammo,

    SpecialAmmo,

    Nothing

}
