using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private Animator boxAnimator;
    [SerializeField] private List<Vector3> points;

    private int _currentProduct;
    private CashRegister _cashRegister;

    protected static readonly int CloseTrigger = Animator.StringToHash("Close");

    public void Init(CashRegister cashRegister)
    {
        _cashRegister = cashRegister;   
        _currentProduct = 0;
    }
    public void PlayCloseAnimation()
    {
        boxAnimator.SetTrigger(CloseTrigger);
    }
    public void Close()
    {
        _cashRegister.RemoveBox();
    }

    internal void AddBox(ProductItem productItem, bool last)
    {
        productItem.transform.SetParent(transform, true);
        productItem.MoveToLocalTarget(points[_currentProduct], 0.3f, () =>
        {
            if (last)
            {
                PlayCloseAnimation();
            }
        });
        _currentProduct++;
    }
    public void MoveToLocalTarget(Vector3 target, float duration, Action onComplete = null)
    {
        transform.DOLocalMove(target, duration).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
}
