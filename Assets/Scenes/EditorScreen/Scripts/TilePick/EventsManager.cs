using UnityEngine;
using System;

public class EventsManager : MonoBehaviour
{
    public static event Action<TileData> OnTileSelect;

    public static void TileSelect(TileData data)
    {
        OnTileSelect?.Invoke(data);
    }
}