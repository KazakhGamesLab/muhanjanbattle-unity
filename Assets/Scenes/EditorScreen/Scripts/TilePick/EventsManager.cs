using UnityEngine;
using System;

public class EventsManager : MonoBehaviour
{
    public static event Action<TileData> OnTileSelect;

    public static event Action<float> OnValueChangedSlider;


    public static void TileSelect(TileData data)
    {
        OnTileSelect?.Invoke(data);
    }

    public static void ValueChangedSlider(float value)
    {
        OnValueChangedSlider?.Invoke(value);
    }
}