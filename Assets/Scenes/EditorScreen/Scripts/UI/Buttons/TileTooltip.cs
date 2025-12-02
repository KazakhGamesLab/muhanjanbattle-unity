using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileInfoDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject _tileInfoPrefab;

    private GameObject _canvasOwner;

    private GameObject _tileInfoObject;

    private void Awake()
    {
        _canvasOwner = GameObject.Find("CanvasUI");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_tileInfoObject != null) return;

        _tileInfoObject = Instantiate(_tileInfoPrefab, _canvasOwner.transform);
        _tileInfoObject.GetComponentInChildren<TextMeshProUGUI>().text = gameObject.name;
        _tileInfoObject.transform.position = transform.position;
        _tileInfoObject.transform.localScale = new Vector2(0,0);


        _tileInfoObject.transform.DOScale(1.3f, 0.3f).SetEase(Ease.OutBack);
        _tileInfoObject.transform.DOMove(new Vector2(
            _tileInfoObject.transform.position.x + 50,
            _tileInfoObject.transform.position.y + 50
        ), 0.4f).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_tileInfoObject == null) return;
        Destroy(_tileInfoObject);
    }

}

