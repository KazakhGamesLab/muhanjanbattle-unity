using System.Collections.Generic;
using UnityEngine;

public enum BrushMode
{
    Square,
    Circle
}

public class BrushController : MonoBehaviour
{
    public static BrushController Instance { get; private set; }

    public int Size { get; private set; } = 1;
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
        EventsManager.OnValueChangedSlider += SetSize;
    }

    private void OnDisable()
    {
        EventsManager.OnValueChangedSlider -= SetSize;
    }

    public void SetSize(float value)
    {
        int newSize = Mathf.Max(1, (int)value);
        if (newSize == Size) return;

        Size = newSize;
        EventsManager.BrushSizeChanged(Size);
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
