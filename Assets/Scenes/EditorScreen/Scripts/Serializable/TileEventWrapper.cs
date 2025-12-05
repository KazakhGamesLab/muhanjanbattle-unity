using System;

[Serializable]
public class TileEvent
{
    public string eventType; 
    public TileDataSerializable tile;
    public int x;
    public int y;
    public TileDataSerializable[] addedTiles;
    public TileCoord[] deletedTiles;
}

[Serializable]
public class TileCoord
{
    public int x;
    public int y;
}

[Serializable]
public class TileEventWrapper
{
    public string type; 
    public string data; 
}

[Serializable]
public class TileBulkEvent
{
    public int[] deleted_ids;
    public TileDataSerializable[] added_tiles; 
}