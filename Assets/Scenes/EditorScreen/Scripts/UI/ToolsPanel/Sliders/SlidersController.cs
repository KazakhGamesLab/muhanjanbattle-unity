using UnityEngine;
using UnityEngine.UI;

public class SlidersController : MonoBehaviour
{

    [SerializeField]
    private AnimationResizeBrushButton _selectedSliderButton;

    private GameObject _selectedSlider;

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
        _selectedSliderButton.SetSelect(true);
        _selectedSlider = _selectedSliderButton.GetSlider();
    }

    public void SelectSlider(AnimationResizeBrushButton slider)
    {
        _selectedSliderButton.SetSelect(false);

        _selectedSliderButton = slider;

        _selectedSliderButton.SetSelect(true);
        _selectedSlider = _selectedSliderButton.GetSlider();
        _selectedSlider.GetComponentInChildren<Slider>().value = _sizeBrush;
    }

    private void SliderHandler(float value)
    {
        _sizeBrush = (int)value;
    }

}
