using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStall : MonoBehaviour
{
    [SerializeField] private int row;
    [SerializeField] private int column;
    [SerializeField] private Transform spawnPoint;

    public int MaxProduct => row * column;
    protected Vector3 spawnPosition => spawnPoint.localPosition;
    protected int _currentProduct = 0;

    private void Start()
    {
        _currentProduct = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var _playerController = other.transform.parent.GetComponent<PlayerController>();
            if (_playerController != null)
            {
                List<ProductItem> products = new List<ProductItem>();
                _playerController.RemoveProductsToStall(MaxProduct - _currentProduct, out products);
                int temp = _currentProduct;
                for (int i = 0; i < products.Count; i++)
                {
                    products[i].transform.SetParent(spawnPoint, true);
                    products[i].MoveToLocalTarget(GetProductPosition(temp + i), 0.3f, () =>
                    {
                        _currentProduct++;
                    });
                }
            }
        }
    }

    public Vector3 GetProductPosition(int index)
    {
        Vector2Int pos2Int = new Vector2Int(index / column, index % column);
        return new Vector3(spawnPosition.x * -pos2Int.y * 0.65f, spawnPosition.y, spawnPosition.z * -pos2Int.x * 1.8f);
    }
}
