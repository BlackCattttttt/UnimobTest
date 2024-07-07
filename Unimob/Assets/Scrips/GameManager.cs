using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private TMP_Text moneyText;

    public int Money;

    private void Start()
    {
       // Money = 0;
        moneyText.text = Money.ToString();
    }

    public void AddMoney(int money)
    {
        Money += money;
        moneyText.text = Money.ToString();
    }

    public void MinusMoney(int money, Action onComplete = null) 
    {
        if (Money >= money)
        {
            Money -= money;
            moneyText.text = Money.ToString();

            onComplete?.Invoke();
        }
    }
}
