using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class AnimationResizeBrushButton : MonoBehaviour, IPointerEnterHandler
{
    [Header("Prefabs & Sprites")]
    [SerializeField] private GameObject sliderPrefab;
    [SerializeField] private Sprite selectButtonSprite;
    [SerializeField] private Sprite defaultButtonSprite;

    [Header("Runtime")]
    [SerializeField] private string canvasName = "CanvasUI";

    private GameObject _canvasOwner;
    private GameObject _sliderInstance;
    private bool _isSelected = false;

    private int _sizeBrush = 0;


    private void OnEnable()
    {
        EventsManager.OnValueChangedSlider += SliderHandler;
    }

    private void OnDisable()
    {
        EventsManager.OnValueChangedSlider -= SliderHandler;
    }

    private void Awake()
    {
        _canvasOwner = GameObject.Find(canvasName);
        SwitchSprite();
    }

    public void SetSelect(bool isSelected)
    {
        _isSelected = isSelected;
        SwitchSprite();

        if (!_isSelected)
        {
            if (_sliderInstance != null)
            {
                var anim = _sliderInstance.GetComponent<AnimationsSlider>();
                if (anim != null)
                {
                    anim.CloseSlider();
                }
                else
                {
                    Destroy(_sliderInstance);
                }

                _sliderInstance = null;
            }

            return;
        }

        CreateSlider();
    }

    private void SwitchSprite()
    {
        var image = GetComponent<Image>();
        if (image == null) return;
        image.sprite = _isSelected ? selectButtonSprite : defaultButtonSprite;
    }

    public void CreateSlider()
    {
        if (!_isSelected || sliderPrefab == null || _canvasOwner == null) return;
        if (_sliderInstance != null) return;

        _sliderInstance = Instantiate(sliderPrefab, _canvasOwner.transform);
        _sliderInstance.name = $"{name}_Slider";
        _sliderInstance.GetComponentInChildren<Slider>()
            .SetValueWithoutNotify(_sizeBrush);


        var img = _sliderInstance.GetComponent<Image>();
        var cg = _sliderInstance.GetComponent<CanvasGroup>();
        if (img != null) img.raycastTarget = false;
        if (cg != null)
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }

        _sliderInstance.transform.position = transform.position;
        _sliderInstance.transform.localScale = Vector3.zero;

        _sliderInstance.transform.DOScale(1f, 0.28f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            if (img != null) img.raycastTarget = true;
            if (cg != null)
            {
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isSelected)
            CreateSlider();
    }

    public GameObject GetSlider()
    {
        return _sliderInstance;
    }

    private void SliderHandler(float value)
    {
        _sizeBrush = (int)value;
    }
}
