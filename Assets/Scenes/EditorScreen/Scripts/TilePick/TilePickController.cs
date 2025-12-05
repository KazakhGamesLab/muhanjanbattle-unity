using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePickController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Tilemap tilemap;
    private Dictionary<string, TileBase> _tileCache = new();

    private void OnEnable()
    {
        // Подписываемся на НОВЫЕ события
        EventsManager.OnTileCreated += OnTileCreated;
        EventsManager.OnTileDeleted += OnTileDeleted;
        EventsManager.OnTilesCreatedBulk += OnTilesCreatedBulk;
        EventsManager.OnTilesDeletedBulk += OnTilesDeletedBulk;
        EventsManager.OnGetTilesJson += OnGetTilesJson;
    }

    private void OnDisable()
    {
        // Отписываемся
        EventsManager.OnTileCreated -= OnTileCreated;
        EventsManager.OnTileDeleted -= OnTileDeleted;
        EventsManager.OnTilesCreatedBulk -= OnTilesCreatedBulk;
        EventsManager.OnTilesDeletedBulk -= OnTilesDeletedBulk;
        EventsManager.OnGetTilesJson -= OnGetTilesJson;

    }

    private void Awake()
    {
        TileData[] tileDatas = Resources.LoadAll<TileData>("Shared/TilesData");
        _tileCache = new Dictionary<string, TileBase>();
        foreach (TileData td in tileDatas)
        {
            if (!string.IsNullOrEmpty(td.tileName) && td.sprite != null)
            {
                Tile tile = ScriptableObject.CreateInstance<Tile>();
                tile.sprite = td.sprite;
                tile.name = "RuntimeTile_" + td.tileName;
                _tileCache[td.tileName] = tile;
            }
        }
    }

    void OnTileCreated(TileDataSerializable tile) => SetTile(tile);
    void OnTilesCreatedBulk(TileDataSerializable[] tiles)
    {
        foreach (var t in tiles) SetTile(t);
    }

    void OnTileDeleted(int x, int y) => tilemap?.SetTile(new Vector3Int(x, y, 0), null);
    void OnTilesDeletedBulk(TileCoord[] coords)
    {
        foreach (var c in coords)
            tilemap?.SetTile(new Vector3Int(c.x, c.y, 0), null);
    }

    void SetTile(TileDataSerializable tile)
    {
        if (tilemap == null) return;
        if (_tileCache.TryGetValue(tile.tileName, out var tileBase))
        {
            tilemap.SetTile(new Vector3Int(tile.x, tile.y, 0), tileBase);
        }
        else
        {
            Debug.LogWarning($"Tile '{tile.tileName}' not found in cache!");
        }
    }


    private void OnGetTilesJson(string json)
    {
        try
        {
            var tile = JsonUtility.FromJson<TileDataSerializable>(json);
            if (tile != null) SetTile(tile);
        }
        catch (Exception ex)
        {
            Debug.LogError($"JSON parse failed: {ex.Message}");
        }
    }
}