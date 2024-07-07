using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStall : BaseBuilding
{
    [SerializeField] private ProductType supplyType;
    [SerializeField] private int row;
    [SerializeField] private int column;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private List<BaseSlotCustomer> baseSlotCustomers = new List<BaseSlotCustomer>();

    public ProductType SupplyType => supplyType;
    public int MaxProduct => row * column;
    protected Vector3 spawnPosition => spawnPoint.localPosition;
    protected int _currentProduct = 0;
    protected List<ProductItem> productItems = new List<ProductItem>();
    protected List<BaseCustomer> customers = new List<BaseCustomer>();

    private void Start()
    {
        _currentProduct = 0;
        productItems.Clear();
    }

    private void Update()
    {
        for (int i = 0; i < customers.Count; i++)
        {
            if (_currentProduct > 0 && customers[i].CheckHasOrder())
            {
                var _product = productItems[_currentProduct - 1];
                customers[i].ReceiveOrder(_product);
                productItems.Remove(_product);
                _currentProduct--;
            }
        }
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
                    int index = i;
                    products[i].transform.SetParent(spawnPoint, true);
                    products[i].MoveToLocalTarget(GetProductPosition(temp + i), 0.3f, () =>
                    {
                        productItems.Add(products[index]);
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

    public BaseSlotCustomer GetBaseSlotCustomerFree()
    {
        for (int i = 0; i < baseSlotCustomers.Count; i++)
        {
            if (baseSlotCustomers[i].GetSlotState() == SlotState.Free)
                return baseSlotCustomers[i];
        }
        return null;
    }
    public List<BaseSlotCustomer> GetBaseSlotCustomers()
    {
        return baseSlotCustomers;
    }
    public void AddCustomer(BaseCustomer customer)
    {
        customers.Add(customer);
    }
    public void RemoveCustomer(BaseCustomer customer)
    {
        customers.Remove(customer);
    }
    public override void ActiveBuiling()
    {
        gameObject.SetActive(true);
        BaseStore.Instance.AddStall(this);
    }
}
