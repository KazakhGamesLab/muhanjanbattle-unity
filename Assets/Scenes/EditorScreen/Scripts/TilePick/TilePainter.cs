using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[Serializable]
public class BrushTileInfo
{
    public string tileName;
    public int brushSize;
    public string brushMode;
    public List<Vector2Int> positions;
}

public class TilePainter : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap; 

    public void OnPaint(InputAction.CallbackContext context)
    {
        if (!context.performed) return; 

        CaptureBrushInfo();
    }

    private async void CaptureBrushInfo()
    {
        if (BrushController.Instance == null || tilemap == null)
            return;

        Sprite selectedSprite = GetComponent<TilePrewiew>().SelectedSprite;
        if (selectedSprite == null)
            return;

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorld.z = 0f;

        Vector3Int cellPos = tilemap.WorldToCell(mouseWorld);
        List<Vector2Int> brushTiles = BrushController.Instance.GetBrushTiles(new Vector2Int(cellPos.x, cellPos.y));

        if (brushTiles.Count == 0)
            return;

        List<TilePosition> positions = new List<TilePosition>();
        foreach (var p in brushTiles)
            positions.Add(new TilePosition(p.x, p.y));

        TileBulkData info = new TileBulkData(
            selectedSprite.name,
            BrushController.Instance.Size,
            BrushController.Instance.Mode.ToString(),
            positions
        );

        string json = JsonUtility.ToJson(info, true);
        Debug.Log(json);

        await ApiClient.SendTilesAsync(info);
    }
}
