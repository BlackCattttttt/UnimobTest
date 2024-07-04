using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarry : MonoBehaviour
{
    [SerializeField] private int maxProductsCarry;
    [SerializeField] private GameObject productPrefab;

    private List<GameObject> products = new List<GameObject>();
    private int _currentProductsCarry = 0;

    public int CurrentProductsCarry => _currentProductsCarry;

    private void Start()
    {
        _currentProductsCarry = 0;
    }

    public void AddProduct(int quantity, out int redundant)
    {
        if (_currentProductsCarry + quantity > maxProductsCarry)
        {
            for (int i = _currentProductsCarry; i < maxProductsCarry; i++)
            {
                var _product = SimplePool.Spawn(productPrefab, Vector3.zero, Quaternion.identity);
                _product.transform.SetParent(transform, false);
                _product.transform.localPosition = Vector3.up * i * 0.335f;
            }
            redundant = _currentProductsCarry + quantity - maxProductsCarry;
            _currentProductsCarry = maxProductsCarry;
        }
        else
        {
            int newProduct = _currentProductsCarry + quantity;
            for (int i = _currentProductsCarry; i < newProduct; i++)
            {
                var _product = SimplePool.Spawn(productPrefab, Vector3.zero, Quaternion.identity);
                _product.transform.SetParent(transform, false);
                _product.transform.localPosition = Vector3.up * i * 0.335f;
            }
            _currentProductsCarry = newProduct;
            redundant = 0;
        }
    }
}
