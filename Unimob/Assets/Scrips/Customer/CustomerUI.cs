using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomerUI : MonoBehaviour
{
    [SerializeField] private RectTransform orderPanel;
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text quantityText;

    private int _quantity;

    public void SetData(Sprite icon, int quantity)
    {
        orderPanel.gameObject.SetActive(true);
        iconImage.sprite = icon;
        _quantity = quantity;
        quantityText.text = "0/" + quantity.ToString();
    }
    public void UpdateData(int current)
    {
        quantityText.text = (_quantity - current).ToString() + "/" + _quantity.ToString();
    }
}
