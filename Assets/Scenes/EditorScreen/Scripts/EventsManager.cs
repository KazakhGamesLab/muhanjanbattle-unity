using UnityEngine;
using System;

public class EventData
{
    public string eventType;
    public string jsonData;

    public EventData(string eventType, string jsonData)
    {
        this.eventType = eventType;
        this.jsonData = jsonData;
    }
}

public class EventsManager : MonoBehaviour
{
    public static event Action<TileData> OnTileSelect;

    public static event Action<float> OnValueChangedSlider;

    public static  event Action<int> OnBrushSizeChanged;

    public static  event Action<TileDataSerializable> OnGetTileServer;
    public static  event Action<string> OnGetTilesJson;


    public static event Action<Vector2, string> OnMoveCoursour;

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

    public static void GetTilesJson(string value)
    {
        OnGetTilesJson?.Invoke(value);
    }

    public static void SSEEventHandler(EventData eventData)
    {
        if (eventData.eventType == "tile_update")
        {
            OnGetTilesJson(eventData.jsonData);
        }
    }

    public static void MoveCoursour(Vector2 value, string username)
    {
        OnMoveCoursour?.Invoke(value, username);
    }
}