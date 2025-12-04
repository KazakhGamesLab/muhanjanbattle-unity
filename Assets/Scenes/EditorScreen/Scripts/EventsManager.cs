using UnityEngine;
using System;

public class EventsManager : MonoBehaviour
{
    public static event Action<TileData> OnTileSelect;

    public static event Action<float> OnValueChangedSlider;

    public static  event Action<int> OnBrushSizeChanged;

    public static  event Action<TileDataSerializable> OnGetTileServer;

    public static void TileSelect(TileData data)
    {
        OnTileSelect?.Invoke(data);
    }

    public static void ValueChangedSlider(float value)
    {
        OnValueChangedSlider?.Invoke(value);
    }

    public static void BrushSizeChanged(int value)
    {
        OnBrushSizeChanged?.Invoke(value);
    }

    public static void GetTileServer(TileDataSerializable value)
    {
        OnGetTileServer?.Invoke(value);
    }
}