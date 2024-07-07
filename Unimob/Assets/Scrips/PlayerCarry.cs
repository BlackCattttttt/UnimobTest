using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarry : MonoBehaviour
{
    [SerializeField] private int maxProductsCarry;
    [SerializeField] private RectTransform canvas;

    private List<ProductItem> products = new List<ProductItem>();
    private int _currentProductsCarry = 0;

    public int CurrentProductsCarry => _currentProductsCarry;

    private void Start()
    {
        _currentProductsCarry = 0;
    }
    public bool CheckProduct(ProductType productItem)
    {
        if (_currentProductsCarry == 0)
            return true;
        return products[0].SupplyType == productItem;
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
            int n = productItems.Count - 1;
            for (int i = 0; i < temp; i++)
            {
                var _product = productItems[n - i];
                _product.transform.SetParent(transform, true);
                _product.MoveToLocalTarget(Vector3.up * _currentProductsCarry * 0.335f, 0.3f);
                _currentProductsCarry++;

                products.Add(_product);
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
                _product.transform.SetParent(transform, true);
                _product.MoveToLocalTarget(Vector3.up * _currentProductsCarry * 0.335f, 0.3f);
                _currentProductsCarry++;

                products.Add(_product);
                productItems.Remove(_product);
            }
            //_currentProductsCarry = newProduct;
            redundant = 0;
        }

        canvas.gameObject.SetActive(_currentProductsCarry == maxProductsCarry);
    }
    public void RemoveProductsToStall(int capacity, out List<ProductItem> productItems)
    {
        if (_currentProductsCarry <= capacity)
        {
            productItems = new List<ProductItem>();
            productItems.AddRange(products);

            products.Clear();
            _currentProductsCarry = 0;
        }
        else
        {
            productItems = new List<ProductItem>();
            int n = products.Count - 1;
            for (int i = 0;i< capacity;i++)
            {
                var _product = products[n - i];
                productItems.Add(_product);
                products.Remove(_product);
            }
            _currentProductsCarry -= capacity;
        }
        canvas.gameObject.SetActive(_currentProductsCarry == maxProductsCarry);
    }
}
