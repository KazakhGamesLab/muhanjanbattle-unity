using DG.Tweening;
using TMPro;
using UnityEngine;

public class AnimationNamePanel : MonoBehaviour
{

    [SerializeField] GameObject _inputField;
    [SerializeField] CoursoursController _coursoursController;
    void Start()
    {
        transform.DOScale(new Vector3(1, 1, 1), 0.3f).SetEase(Ease.InBack);

    }

    public void InitPlayer()
    {
        _coursoursController.SetLocalUsername(_inputField.GetComponent<TMP_InputField>().text);
        transform.DOScale(new Vector3(0.5f, 0.5f, 1), 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
