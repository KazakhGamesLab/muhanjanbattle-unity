using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InitTilePicker : MonoBehaviour
{
    [SerializeField]
    private GameObject _parentContainer;
    [SerializeField]
    private GameObject _prefabTileGroup;
    [SerializeField]
    private GameObject _prefabTilePickButton;

    private struct GroupTiles
    {
        public  TileGroup group;
        public  GameObject groupContainer;

        public GroupTiles(TileGroup group, GameObject groupContainer)
        {
            this.group = group;
            this.groupContainer = groupContainer;
        }
    }

    private TileData[] _tilesData;

    private List<GroupTiles> _groupsTiles = new List<GroupTiles>();

    private void Awake()
    {
        _tilesData = Resources.LoadAll<TileData>("Shared/TilesData");
        InitTilesPick();
    }

    private void InitTilesPick()
    {

        foreach (TileData tileData in _tilesData)
        {
            GroupTiles? foundGroup = FindGroup(tileData.tileEnum.tileGroup);

            GameObject groupContainer = null;

            if (foundGroup == null)
            {
                groupContainer = Instantiate(_prefabTileGroup, _parentContainer.transform);
                GameObject TileGroupName = 
                    groupContainer.transform.Find("TileGroupName").gameObject;

                var tmpUGUI = TileGroupName.GetComponent<TMPro.TextMeshProUGUI>();
                tmpUGUI.text = tileData.tileEnum.groupName;

                GroupTiles newGroup = new GroupTiles(tileData.tileEnum.tileGroup, groupContainer);
                _groupsTiles.Add(newGroup);
            }
            else
            {
                groupContainer = foundGroup.Value.groupContainer;
            }


            GameObject buttonTilePick = 
                Instantiate(
                    _prefabTilePickButton, 
                    groupContainer.transform.Find("TilesGrid").transform
                );

            buttonTilePick.GetComponent<Image>().sprite = tileData.sprite;
            buttonTilePick.GetComponent<Button>().onClick.AddListener(() => TileEvents.TileSelect(tileData));

            

            buttonTilePick.transform.DOScale(1.2f, 0.5f).SetEase(Ease.InBack).SetLoops(2, LoopType.Yoyo);

        }
    }

    private GroupTiles? FindGroup(TileGroup group)
    {
        foreach (var g in _groupsTiles)
        {
            if (g.group == group)
                return g;
        }

        return null;
    }
}
