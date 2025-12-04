using UnityEngine;
using UnityEngine.Rendering.Universal; // для PixelPerfectCamera

public class CursorView : MonoBehaviour
{
    public bool IsLocalPlayer { get; set; }
    public string Username { get; set; }

    [SerializeField] private Camera _camera;
    private PixelPerfectCamera _pixelPerfectCamera;

    private SpriteRenderer _sprite;

    private void OnEnable()
    {
        _sprite = GetComponent<SpriteRenderer>();
        if (_camera == null)
            _camera = Camera.main;

        _pixelPerfectCamera = _camera.GetComponent<PixelPerfectCamera>();

        if (IsLocalPlayer)
            Cursor.visible = false;

        EventsManager.OnMoveCoursour += OnMoveCoursour;
    }

    private void OnDisable()
    {
        EventsManager.OnMoveCoursour -= OnMoveCoursour;

        if (IsLocalPlayer)
            Cursor.visible = true;
    }

    private void OnMoveCoursour(Vector2 screenMousePos, string username)
    {
        if (Username != username) return;
        if (_camera == null || _sprite?.sprite == null) return;

        // Позиция
        Vector3 screenPos = new Vector3(screenMousePos.x, screenMousePos.y, -_camera.transform.position.z);
        Vector3 worldPos = _camera.ScreenToWorldPoint(screenPos);

        // МАСШТАБИРОВАНИЕ: чтобы курсор был ОДНОГО размера на экране
        if (_pixelPerfectCamera != null)
        {
            // Ключевое: масштаб ОБРАТНО пропорционален assetsPPU
            float scale = 1f / _pixelPerfectCamera.assetsPPU;
            // Но чтобы не было слишком маленького значения — нормализуем к базовому PPU
            // Например, если при PPU=100 курсор был "нормальным", то:
            const float BASE_PPU = 100f;
            scale *= BASE_PPU;

            transform.localScale = Vector3.one * scale;
        }

        // Смещение
        float w = _sprite.sprite.rect.width / _sprite.sprite.pixelsPerUnit;
        float h = _sprite.sprite.rect.height / _sprite.sprite.pixelsPerUnit;
        Vector2 offset = new Vector2(w * 0.25f, h * 0.5f);

        transform.position = new Vector3(
            worldPos.x - offset.x,
            worldPos.y - offset.y,
            0f
        );
    }

    public void SetUsername(string username)
    {
        Username = username;
        gameObject.name = "Cursor_" + username;
    }
}