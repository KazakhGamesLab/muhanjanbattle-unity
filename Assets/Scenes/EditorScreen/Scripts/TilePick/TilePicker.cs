using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitTilePicker : MonoBehaviour
{
    [SerializeField] private GameObject parentContainer;
    [SerializeField] private GameObject prefabTileGroup;
    [SerializeField] private GameObject prefabTilePickButton;

    private GameObject _selectedTilePickButton;
    private readonly List<(TileGroup group, GameObject container)> _groups = new();
    private TileData[] _tilesData;

    private void Awake()
    {
        _tilesData = Resources.LoadAll<TileData>("Shared/TilesData");
        InitTilesUI();
    }

    private void InitTilesUI()
    {
        if (_tilesData == null || prefabTileGroup == null || prefabTilePickButton == null || parentContainer == null)
        {
            Debug.LogWarning("InitTilePicker: one of the references is null.");
            return;
        }

        foreach (TileData tileData in _tilesData)
        {
            if (tileData == null) continue;

            GameObject container = GetOrCreateGroup(tileData.tileEnum.tileGroup, tileData.tileEnum.groupName);

            var tilesGrid = container.transform.Find("TilesGrid");
            Transform parent = tilesGrid != null ? tilesGrid : container.transform;

            GameObject btn = Instantiate(prefabTilePickButton, parent);
            btn.name = tileData.tileName;

            var img = btn.GetComponent<Image>();
            if (img != null) img.sprite = tileData.sprite;

            var button = btn.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() =>
                {
                    EventsManager.TileSelect(tileData);
                    HighlightSelected(btn);
                });
            }

            btn.transform.DOScale(1.2f, 0.5f).SetEase(Ease.InBack).SetLoops(2, LoopType.Yoyo);
        }
    }

    private GameObject GetOrCreateGroup(TileGroup group, string groupName)
    {
        foreach (var g in _groups)
            if (g.group == group)
                return g.container;

        GameObject newContainer = Instantiate(prefabTileGroup, parentContainer.transform);
        var nameTf = newContainer.transform.Find("TileGroupName");
        if (nameTf != null)
        {
            var tmp = nameTf.GetComponent<TMPro.TextMeshProUGUI>();
            if (tmp != null) tmp.text = groupName;
        }

        _groups.Add((group, newContainer));
        return newContainer;
    }

    private void HighlightSelected(GameObject tile)
    {
        if (_selectedTilePickButton == tile)
        {
            SetBorder(tile, false);
            _selectedTilePickButton = null;
            return;
        }

        if (_selectedTilePickButton != null)
            SetBorder(_selectedTilePickButton, false);

        _selectedTilePickButton = tile;
        SetBorder(_selectedTilePickButton, true);
    }

    private void SetBorder(GameObject tile, bool selected)
    {
        var border = tile.transform.Find("Border");
        var borderSelected = tile.transform.Find("BorderSelected");

        if (border != null) border.gameObject.SetActive(!selected);
        if (borderSelected != null) borderSelected.gameObject.SetActive(selected);
    }
}
