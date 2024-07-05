using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarry : MonoBehaviour
{
    [SerializeField] private int maxProductsCarry;

    private List<GameObject> products = new List<GameObject>();
    private int _currentProductsCarry = 0;

    public int CurrentProductsCarry => _currentProductsCarry;

    private void Start()
    {
        _currentProductsCarry = 0;
    }

    public void AddProduct(int quantity, List<ProductItem> productItems, out int redundant)
    {
        if (_currentProductsCarry > maxProductsCarry)
        {
            redundant = quantity;
            return;
        }
        if (_currentProductsCarry + quantity > maxProductsCarry)
        {
            redundant = _currentProductsCarry + quantity - maxProductsCarry;
            int temp = maxProductsCarry - _currentProductsCarry;
            for (int i = 0; i < temp; i++)
            {
                var _product = productItems[productItems.Count - 1 - i];
                _product.transform.SetParent(transform, false);
                _product.MoveToLocalTarget(Vector3.up * _currentProductsCarry * 0.335f, 0.3f);
                _currentProductsCarry++;
                productItems.Remove(_product);
            }
          //  _currentProductsCarry = maxProductsCarry;
        }
        else
        {
            //int newProduct = _currentProductsCarry + quantity;
            for (int i = 0; i < productItems.Count; i++)
            {
                var _product = productItems[i];
                _product.transform.SetParent(transform, false);
                _product.MoveToLocalTarget(Vector3.up * _currentProductsCarry * 0.335f, 0.3f);
                _currentProductsCarry++;
                productItems.Remove(_product);
            }
            //_currentProductsCarry = newProduct;
            redundant = 0;
        }
    }
}
