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
    public Sprite SelectedSprite { get; private set; }
    private readonly List<GameObject> _previewTiles = new();

    private void OnEnable()
    {
        EventsManager.OnTileSelect += SelectTilePreview;
        EventsManager.OnBrushSizeChanged += OnBrushSizeChanged;
    }

    private void OnDisable()
    {
        EventsManager.OnTileSelect -= SelectTilePreview;
        EventsManager.OnBrushSizeChanged -= OnBrushSizeChanged;
        ClearPreview();
    }

    private void OnBrushSizeChanged(int newSize)
    {
        if (SelectedSprite == null || tilemap == null)
            return;

        ClearPreview();
        CreatePreviewTiles();

        UpdatePreviewTilesPosition();
    }

    public void SelectTilePreview(TileData data)
    {
        if (data == null)
        {
            SelectedSprite = null;
            ClearPreview();
            return;
        }

        if (SelectedSprite == data.sprite)
        {
            SelectedSprite = null;
            ClearPreview();
            return;
        }

        SelectedSprite = data.sprite;
        ClearPreview();
        CreatePreviewTiles();
    }

    private void CreatePreviewTiles()
    {
        if (SelectedSprite == null || BrushController.Instance == null)
            return;

        int required = BrushController.Instance.GetBrushTiles(Vector2Int.zero).Count;

        for (int i = 0; i < required; i++)
        {
            GameObject go = new GameObject($"PreviewTile_{i}");
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = SelectedSprite;
            sr.color = new Color(1f, 1f, 1f, previewAlpha);
            sr.sortingOrder = 100;
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
        if (!context.performed || SelectedSprite == null || tilemap == null || BrushController.Instance == null)
            return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0f;

        Vector3Int cellPos = tilemap.WorldToCell(mousePos);
        List<Vector2Int> brushTiles = BrushController.Instance.GetBrushTiles(new Vector2Int(cellPos.x, cellPos.y));

        if (brushTiles.Count != _previewTiles.Count)
        {
            ClearPreview();
            CreatePreviewTiles();
        }

        int limit = Mathf.Min(brushTiles.Count, _previewTiles.Count);
        for (int i = 0; i < limit; i++)
        {
            Vector3Int brushCell = new Vector3Int(brushTiles[i].x, brushTiles[i].y, 0);
            Vector3 worldPos = tilemap.GetCellCenterWorld(brushCell);
            _previewTiles[i].transform.position = worldPos;
        }
    }

    private void UpdatePreviewTilesPosition()
    {
        if (_previewTiles.Count == 0) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0f;

        Vector3Int cellPos = tilemap.WorldToCell(mousePos);
        List<Vector2Int> brushTiles = BrushController.Instance.GetBrushTiles(new Vector2Int(cellPos.x, cellPos.y));

        int limit = Mathf.Min(brushTiles.Count, _previewTiles.Count);
        for (int i = 0; i < limit; i++)
        {
            Vector3Int brushCell = new Vector3Int(brushTiles[i].x, brushTiles[i].y, 0);
            Vector3 worldPos = tilemap.GetCellCenterWorld(brushCell);
            _previewTiles[i].transform.position = worldPos;
        }
    }
}
