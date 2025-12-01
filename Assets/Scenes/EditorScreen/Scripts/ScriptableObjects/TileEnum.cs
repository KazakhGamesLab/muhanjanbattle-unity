using UnityEngine;

[SerializeField]
public enum TileGroup
{
    Terrain,
    Waterform,
}

[SerializeField]
public enum TileSubGroup
{
    Earth,
    Water
}

[CreateAssetMenu(fileName = "NewTileEnum", menuName = "2D/Tiles/TileEnum")]
public class TileEnum : ScriptableObject
{
    public string groupName;
    public string subGroupName;
    public TileGroup tileGroup;
    public TileSubGroup tileSubGroup;
}