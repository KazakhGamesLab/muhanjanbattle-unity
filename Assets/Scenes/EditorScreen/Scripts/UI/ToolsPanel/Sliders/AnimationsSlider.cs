using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
public class AnimationsSlider : MonoBehaviour, IPointerExitHandler
{
    private void OnDisable()
    {
        EventsManager.OnBrushSizeChanged -= OnBrushSizeChanged;
    }

    private void OnEnable()
    {
        gameObject.GetComponentInChildren<Slider>()
            .onValueChanged.AddListener((float value) => EventsManager.ValueChangedSlider(value));

        EventsManager.OnBrushSizeChanged += OnBrushSizeChanged;
    }

    public void CloseSlider()
    {
        transform.DOScale(0f, 0.1f).SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CloseSlider();
    }

    private void OnBrushSizeChanged(int newSize)
    {
        gameObject.GetComponentInChildren<Slider>()
            .SetValueWithoutNotify(newSize);
    }
}
