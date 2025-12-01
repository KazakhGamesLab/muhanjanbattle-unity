using UnityEngine;


[CreateAssetMenu(fileName = "NewTileData", menuName = "2D/Tiles/TileData")]
public class TileData : ScriptableObject
{
    public string tileName;
    public Sprite sprite;
    public TileEnum tileEnum;
}
