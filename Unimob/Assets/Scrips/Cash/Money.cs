using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public void MoveToLocalTarget(Vector3 target, float duration, Action onComplete = null)
    {
        transform.DOLocalMove(target, duration).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
}
