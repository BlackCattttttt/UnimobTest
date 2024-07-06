using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerCarry : MonoBehaviour
{
    private List<ProductItem> products = new List<ProductItem>();
    private int _currentProductsCarry = 0;

    public int CurrentProductsCarry => _currentProductsCarry;

    public void AddProduct(ProductItem productItem)
    {
        productItem.transform.SetParent(transform, true);
        productItem.MoveToLocalTarget(Vector3.up * _currentProductsCarry * 0.335f, 0.3f);
        _currentProductsCarry++;

        products.Add(productItem);
    }
    public ProductItem RemoveProduct(out bool last)
    {
        if (_currentProductsCarry == 0)
        {
            last = false;
            return null;
        }
        var _product = products[_currentProductsCarry - 1];
        _currentProductsCarry--;
        products.Remove(_product);
        last = _currentProductsCarry == 0 ? true : false;

        return _product;
    }
    public void AddBox(Box box, Action onComplete = null)
    {
        box.transform.SetParent(transform, true);
        box.MoveToLocalTarget(Vector3.up * 0, 0.3f, () =>
        {
            onComplete?.Invoke();
        });
    }
    public bool CheckEmpty()
    {
        return transform.childCount > 0 ? false : true;
    }
    public void Clear()
    {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
    }
}
