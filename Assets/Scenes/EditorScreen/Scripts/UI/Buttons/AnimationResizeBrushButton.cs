using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class AnimationResizeBrushButton : MonoBehaviour, IPointerEnterHandler
{
    private GameObject _canvasOwner;

    [SerializeField]
    private GameObject _sliderPrefab;

    [SerializeField]
    private Sprite _selectButtonSprite;

    [SerializeField]
    private Sprite _defaultButtonSprite;

    private bool _isSelected = false;

    private GameObject _slider;

    private void Awake()
    {
        _canvasOwner = GameObject.Find("CanvasUI");
        SwitchSprite();
    }

    public void SetSelect(bool isSelected)
    {
        _isSelected = isSelected;
        SwitchSprite();


        if (!_isSelected && _slider != null)
        {
            _slider.GetComponent<AnimationsSlider>().CloseSlider();
        }


        if (_canvasOwner != null)
        {
            CreateSlider();
        }
    }


    private void SwitchSprite()
    {
        Image image = GetComponent<Image>();

        if (_isSelected)
        {
            image.sprite = _selectButtonSprite;
        }

        if (!_isSelected)
        {
            image.sprite = _defaultButtonSprite;
        }
    }

    public void CreateSlider()
    {
        if (!_isSelected) { return; }

        if (_slider != null) { return; }

        _slider = Instantiate(_sliderPrefab, _canvasOwner.transform);
        _slider.GetComponent<Image>().raycastTarget = false;
        _slider.transform.position = transform.position;
        _slider.transform.localScale = new Vector2(0, 0);


        _slider.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack)
        .OnComplete(() =>
        {
            _slider.GetComponent<Image>().raycastTarget = true;
            _slider.GetComponent<CanvasGroup>().interactable = true;
            _slider.GetComponent<CanvasGroup>().blocksRaycasts = true;
        });
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        CreateSlider();
    }


    public GameObject GetSlider()
    {
        if (_slider != null) { return _slider; }

        return null;
    }
}
