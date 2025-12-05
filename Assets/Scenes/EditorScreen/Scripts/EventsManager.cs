using System;
using UnityEngine;

public class EventData
{
    public string eventType;
    public string jsonData;
    public EventData(string et, string jd) { eventType = et; jsonData = jd; }
}

public class EventsManager : MonoBehaviour
{
    public static event Action<TileData> OnTileSelect;

    public static event Action<float> OnValueChangedSlider;

    public static  event Action<int> OnBrushSizeChanged;

    public static  event Action<TileDataSerializable> OnGetTileServer;
    public static  event Action<string> OnGetTilesJson;
    public static event Action<Vector2, string> OnMoveCoursour;


    public static event Action<TileDataSerializable> OnTileCreated;
    public static event Action<int, int> OnTileDeleted;
    public static event Action<TileDataSerializable[]> OnTilesCreatedBulk;
    public static event Action<TileCoord[]> OnTilesDeletedBulk;

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
        if (eventData?.jsonData == null || eventData.eventType != "tile_event") return;

        try
        {
            var ev = JsonUtility.FromJson<TileEvent>(eventData.jsonData);
            if (ev == null) return;

            switch (ev.eventType)
            {
                case "create":
                    OnTileCreated?.Invoke(ev.tile);
                    break;
                case "delete":
                    OnTileDeleted?.Invoke(ev.x, ev.y);
                    break;
                case "bulk":
                    if (ev.deletedTiles != null) OnTilesDeletedBulk?.Invoke(ev.deletedTiles);
                    if (ev.addedTiles != null) OnTilesCreatedBulk?.Invoke(ev.addedTiles);
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"SSE Parse Error: {ex}\n{eventData.jsonData}");
        }
    }

    public static void MoveCoursour(Vector2 value, string username)
    {
        OnMoveCoursour?.Invoke(value, username);
    }
}