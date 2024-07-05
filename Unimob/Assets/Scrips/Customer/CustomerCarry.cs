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
}
