using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileSet", menuName = "Match Attack/TileSetReference")]
public class TileSetReference : ScriptableObject
{

    public bool isLastLevel;

    public int timer;

    public float enemyMinDamagePercentage;

    public Sprite backBackground;

    public Sprite backItem;

    public Sprite frontBackground;

    public Sprite frontAttackBackground;

    public List<Tile> tiles;

}
