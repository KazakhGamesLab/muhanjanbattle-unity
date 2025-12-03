using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class AnimationsSlider : MonoBehaviour, IPointerExitHandler
{
    private void OnEnable()
    {
        gameObject.GetComponentInChildren<Slider>()
            .onValueChanged.AddListener((float value) => EventsManager.ValueChangedSlider(value));
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
}
