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

}

[Serializable]
public enum TileType
{

    Weapon,

    Ammo,

    SpecialAmmo,

    Nothing

}
