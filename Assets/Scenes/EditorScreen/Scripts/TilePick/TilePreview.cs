using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TilewPrewiew : MonoBehaviour
{

    public Tilemap tilemap;     
    public Sprite selectedSprite;

    private Tile previewTile;
    private GameObject previewGO;

    private void OnEnable()
    {
        TileEvents.OnTileSelect += SelectTilePreview;
    }

    private void OnDisable()
    {
        TileEvents.OnTileSelect -= SelectTilePreview;
    }

    public void SelectTilePreview(TileData data)
    {
        if (previewGO != null)
            Destroy(previewGO);


        if (selectedSprite == data.sprite)
        {
            selectedSprite = null;
            Destroy(previewGO);
            return;
        }

        selectedSprite = data.sprite;

        previewTile = ScriptableObject.CreateInstance<Tile>();
        previewTile.sprite = selectedSprite;

        

        previewGO = new GameObject("TilePreview");
        SpriteRenderer sr = previewGO.AddComponent<SpriteRenderer>();
        sr.sprite = selectedSprite;
        sr.sortingOrder = 100;
    }


    public void TilePreview(InputAction.CallbackContext context)
    {
        if (previewGO == null || !context.performed)
            return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPos.z = 0;

        Vector3Int cellPos = tilemap.WorldToCell(mouseWorldPos);
        Vector3 cellCenter = tilemap.GetCellCenterWorld(cellPos);
        previewGO.transform.position = cellCenter;
    }
}
