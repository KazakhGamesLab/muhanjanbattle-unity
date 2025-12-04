using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class TilePickController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Dictionary<string, TileBase> _tileCache = new Dictionary<string, TileBase>();

    private void OnDisable()
    {
        EventsManager.OnGetTileServer -= OnGetTileServer;
    }

    private void OnEnable()
    {
        EventsManager.OnGetTileServer += OnGetTileServer;
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

    private void OnGetTileServer(TileDataSerializable tile)
    {
        if (tilemap == null)
        {
            Debug.LogError("Tilemap is not assigned!");
            return;
        }

        if (_tileCache.TryGetValue(tile.tileName, out TileBase tileBase))
        {
            Vector3Int position = new Vector3Int(tile.x, tile.y, 0);
            tilemap.SetTile(position, tileBase);
        }
        else
        {
            Debug.LogWarning($"No TileData found for tileName: '{tile.tileName}'. Available: {string.Join(", ", _tileCache.Keys)}");
        }
    }

}
