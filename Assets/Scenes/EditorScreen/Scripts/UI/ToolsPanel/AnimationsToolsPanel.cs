using DG.Tweening;
using UnityEngine;

public class AnimationsToolsPanel : MonoBehaviour
{
    private void Start()
    {
        OpenPanel();
    }

    public void ClosePanel()
    {
        transform.DOMoveX(transform.position.x + 410, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        transform.DOMoveX(transform.position.x - 410, 0.5f).SetEase(Ease.OutBack);
    }

} 
