using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TilePrewiew : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Tilemap tilemap;

    [Header("Preview settings")]
    [Tooltip("Alpha for preview sprites")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float previewAlpha = 0.6f;

    private Sprite _selectedSprite;
    private readonly List<GameObject> _previewTiles = new();

    private void OnEnable()
    {
        EventsManager.OnTileSelect += SelectTilePreview;
    }

    private void OnDisable()
    {
        EventsManager.OnTileSelect -= SelectTilePreview;
        ClearPreview();
    }

    public void SelectTilePreview(TileData data)
    {
        // Toggle same tile off
        if (data == null)
        {
            _selectedSprite = null;
            ClearPreview();
            return;
        }

        if (_selectedSprite == data.sprite)
        {
            _selectedSprite = null;
            ClearPreview();
            return;
        }

        _selectedSprite = data.sprite;
        ClearPreview();
        CreatePreviewTiles();
    }

    private void CreatePreviewTiles()
    {
        if (_selectedSprite == null || BrushController.Instance == null)
            return;

        int required = BrushController.Instance.GetBrushTiles(Vector2Int.zero).Count;

        for (int i = 0; i < required; i++)
        {
            GameObject go = new GameObject($"PreviewTile_{i}");
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = _selectedSprite;
            sr.color = new Color(1f, 1f, 1f, previewAlpha);
            sr.sortingOrder = 100;
            // Optional: parent to this for cleanliness
            go.transform.SetParent(transform, true);

            _previewTiles.Add(go);
        }
    }

    private void ClearPreview()
    {
        for (int i = 0; i < _previewTiles.Count; i++)
        {
            var go = _previewTiles[i];
            if (go != null)
                Destroy(go);
        }

        _previewTiles.Clear();
    }


    public void TilePreview(InputAction.CallbackContext context)
    {
        if (!context.performed || _selectedSprite == null || tilemap == null || BrushController.Instance == null)
            return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0f;

        Vector3Int cellPos = tilemap.WorldToCell(mousePos);
        List<Vector2Int> brushTiles = BrushController.Instance.GetBrushTiles(new Vector2Int(cellPos.x, cellPos.y));

        // если количество превью не соответствует кисти — пересоздать
        if (brushTiles.Count != _previewTiles.Count)
        {
            ClearPreview();
            CreatePreviewTiles();
        }

        // обновляем позиции превью
        int limit = Mathf.Min(brushTiles.Count, _previewTiles.Count);
        for (int i = 0; i < limit; i++)
        {
            Vector3Int brushCell = new Vector3Int(brushTiles[i].x, brushTiles[i].y, 0);
            Vector3 worldPos = tilemap.GetCellCenterWorld(brushCell);
            _previewTiles[i].transform.position = worldPos;
        }
    }
}
