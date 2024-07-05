using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductItem : MonoBehaviour
{
    [SerializeField] private ProductType supplyType;

    public ProductType SupplyType { get => supplyType; }

    public void MoveToTarget(Vector3 target,float duration, Action onComplete = null)
    {
        transform.DOMove(target, duration).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
    public void MoveToLocalTarget(Vector3 target, float duration, Action onComplete = null)
    {
        transform.DOLocalMove(target, duration).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
}
