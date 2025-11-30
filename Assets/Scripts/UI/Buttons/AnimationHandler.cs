using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimationHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 _originalScale;
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _enterClip;
    [SerializeField]
    private AudioClip _clickClip;

    private bool _hasPlayedHoverSound = false;

    private void Start()
    {
        _originalScale = transform.localScale;

        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(new Vector3(_originalScale.x * 1.1f, _originalScale.y * 1.1f, _originalScale.z), 0.2f).SetEase(Ease.OutBack);

        if (!_hasPlayedHoverSound && _enterClip != null)
        {
            _audioSource.PlayOneShot(_enterClip);
            _hasPlayedHoverSound = true;
        }

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(_originalScale, 0.2f).SetEase(Ease.InBack);
        _hasPlayedHoverSound = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.DOKill();

        if (_clickClip != null)
        {
            _audioSource.PlayOneShot(_clickClip);
        }

        transform.DOScale(new Vector3(_originalScale.x * 0.9f, _originalScale.y * 0.9f, _originalScale.z), 0.2f).SetEase(Ease.OutBack).SetLoops(1, LoopType.Yoyo);
    }
}
