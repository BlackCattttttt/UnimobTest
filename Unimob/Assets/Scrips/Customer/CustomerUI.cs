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
    [SerializeField] private Image cashierIcon;
    [SerializeField] private Image emojiIcon;

    private int _quantity;

    public void SetData(Sprite icon, int quantity)
    {
        orderPanel.gameObject.SetActive(true);
        cashierIcon.gameObject.SetActive(false);
        emojiIcon.gameObject.SetActive(false);
        iconImage.sprite = icon;
        _quantity = quantity;
        quantityText.text = "0/" + quantity.ToString();
    }
    public void UpdateData(int current)
    {
        quantityText.text = (current).ToString() + "/" + _quantity.ToString();
    }

    public void SetCashierActive()
    {
        orderPanel.gameObject.SetActive(false);
        emojiIcon.gameObject.SetActive(false);
        cashierIcon.gameObject.SetActive(true);
    }
    public void SetEmojiActive()
    {
        orderPanel.gameObject.SetActive(false);
        emojiIcon.gameObject.SetActive(true);
        cashierIcon.gameObject.SetActive(false);
    }
}
