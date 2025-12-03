using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject _camera;

    private float zoomSpeed = 1f;
    private float moveSpeed = 0.01f;
    private Vector3 targetPos;

    [SerializeField]
    private int minAssetsPPU;

    [SerializeField]
    private int maxAssetsPPU;

    public void ZoomHandler(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (Keyboard.current.leftCtrlKey.isPressed || Keyboard.current.rightCtrlKey.isPressed)
            return;

        PixelPerfectCamera _camera = this._camera.GetComponent<PixelPerfectCamera>();

        float zoomChange = context.ReadValue<float>() * zoomSpeed;

        int newPPU = Mathf.Clamp(Mathf.RoundToInt(_camera.assetsPPU + zoomChange), minAssetsPPU, maxAssetsPPU);

        _camera.assetsPPU = newPPU;
    }


    public void MoveHandler(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Vector2 delta = context.ReadValue<Vector2>();

        Camera _camera = this._camera.GetComponent<Camera>();

        _camera.transform.Translate(new Vector3(-delta.x * moveSpeed, -delta.y * moveSpeed, 0), Space.World);
    }

}
