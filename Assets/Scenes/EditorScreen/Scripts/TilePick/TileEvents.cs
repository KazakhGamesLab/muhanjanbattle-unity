using UnityEngine;
using System;

public class TileEvents : MonoBehaviour
{
    public static event Action<TileData> OnTileSelect;

    public static void TileSelect(TileData data)
    {
        OnTileSelect?.Invoke(data);
    }
}