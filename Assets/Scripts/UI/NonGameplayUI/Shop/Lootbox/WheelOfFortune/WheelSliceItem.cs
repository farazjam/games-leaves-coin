using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelSliceItem : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Button button;
    
    private void Start()
    {
        AssertUtil.IsNotNull(image);
        AssertUtil.IsNotNull(button);
        image.color = defaultColor;
        image.alphaHitTestMinimumThreshold = 0.1f;
        button.interactable = true;
    }

    public void Init(Action buttonClickCallback)
    {
        button.onClick.AddListener(() => buttonClickCallback?.Invoke());
        Select(false);
    }

    public void Select(bool isSelected)
    {
        image.color = isSelected ? selectedColor : defaultColor;
    }
}
