using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum BrushMode
{
    Square,
    Circle
}

public class BrushController : MonoBehaviour
{
    public static BrushController Instance { get; private set; }

    public int Size { get; private set; } = 1;

    private const int MinSize = 1;
    private const int MaxSize = 8;
    public BrushMode Mode { get; private set; } = BrushMode.Square;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        EventsManager.OnValueChangedSlider += OnSliderChanged;
    }

    private void OnDisable()
    {
        EventsManager.OnValueChangedSlider -= OnSliderChanged;
    }

    public void SetSizeInt(int newSize)
    {
        newSize = Mathf.Clamp(newSize, MinSize, MaxSize);
        if (Size == newSize) return;

        Size = newSize;
        EventsManager.BrushSizeChanged(Size);
    }

    public void OnSliderChanged(float value)
    {
        SetSizeInt((int)value);
    }


    public void OnScroll(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (!Keyboard.current.leftCtrlKey.isPressed)
            return;

        float scroll = context.ReadValue<float>();
        int delta = scroll > 0 ? 1 : (scroll < 0 ? -1 : 0);
        if (delta == 0) return;

        SetSizeInt(Size + delta);
    }

    public void SetSquareMode() => Mode = BrushMode.Square;
    public void SetCircleMode() => Mode = BrushMode.Circle;

    public List<Vector2Int> GetBrushTiles(Vector2Int center)
    {
        return Mode == BrushMode.Square ? GetSquare(center, Size) : GetCircle(center, Size);
    }

    private List<Vector2Int> GetSquare(Vector2Int center, int size)
    {
        var result = new List<Vector2Int>();
        int radius = size - 1;

        for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
                result.Add(new Vector2Int(center.x + x, center.y + y));

        return result;
    }

    private List<Vector2Int> GetCircle(Vector2Int center, int size)
    {
        var result = new List<Vector2Int>();
        int radius = size - 1;
        int r2 = radius * radius;

        for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
                if (x * x + y * y <= r2)
                    result.Add(new Vector2Int(center.x + x, center.y + y));

        return result;
    }
}
