using UnityEngine;
using UnityEngine.UI;

public class SlidersController : MonoBehaviour
{

    [SerializeField]
    private AnimationResizeBrushButton _selectedSliderButton;

    private GameObject _selectedSlider;

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
    }

}
