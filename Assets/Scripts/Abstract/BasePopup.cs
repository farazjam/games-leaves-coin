using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class BasePopup : MonoBehaviour
{
    [SerializeField] private Transform parentTransform;
    [SerializeField] private Button closePopupButton;

    public bool IsActive => parentTransform.gameObject.activeInHierarchy;

    void Awake()
    {
        Assert.IsNotNull(parentTransform);

        if (closePopupButton != null)
        {
            closePopupButton.onClick.AddListener(OnCloseButtonPressed);
        }
        SetActive(false);
        Init();
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    public void Show() => SetActive(true);

    public void Hide() => SetActive(false);

    protected void SetActive(bool value) => parentTransform.gameObject.SetActive(value);

    protected virtual void Init() { }
    protected virtual void Subscribe() { }
    protected virtual void Unsubscribe() { }

    public virtual void OnCloseButtonPressed() 
    {
        Hide();
    }
}
