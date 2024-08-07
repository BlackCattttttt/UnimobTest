using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using static BaseCustomer;

public class BaseStore : Singleton<BaseStore>
{
    [SerializeField] private ProductDatabase productDatabase;
    [SerializeField] private int maxCustomer = 4;
    [SerializeField] private CustomerSpawner customerSpawner;
    [SerializeField] private List<BaseStall> baseStalls = new List<BaseStall>();
    [SerializeField] private List<CashRegister> cashRegisters = new List<CashRegister>();

    protected Dictionary<ProductType, List<BaseSlotCustomer>> slots = new();
    protected List<ProductType> listProductUnlock = new List<ProductType>();
    protected int currentCustomer = 0;
    protected float delaySpawnCustomer = 1f;
    protected float _delaySpawnCustomer;

    private void Start()
    {
        _delaySpawnCustomer = delaySpawnCustomer;
        UpdateSlotsStall();
        UpdateSupplyUnlock();
    }
    private void Update()
    {
        _delaySpawnCustomer -= Time.deltaTime;
        if (currentCustomer < maxCustomer && _delaySpawnCustomer < 0)
        {
            var _targetSlot = GetBaseSlotCustomerFree();
            if (_targetSlot != null)
            {
                var _customer = customerSpawner.SpawnCustomer();
                _customer.SetTargetSlot(_targetSlot, GenerateOrderData(_targetSlot.Stall.SupplyType), this);
                currentCustomer++;
                _targetSlot.SetSlotState(SlotState.Close);

                _delaySpawnCustomer = delaySpawnCustomer;
            }
        }
    }
    public void AddStall(BaseStall stall)
    {
       // navMeshSurface.BuildNavMesh();
        baseStalls.Add(stall);
        UpdateSlotsStall();
        UpdateSupplyUnlock();
    }
    public void UpdateSlotsStall()
    {
        slots.Clear();
        for (int i = 0; i < baseStalls.Count; i++)
        {
            if (slots.ContainsKey(baseStalls[i].SupplyType))
            {
                slots[baseStalls[i].SupplyType].AddRange(baseStalls[i].GetBaseSlotCustomers());
            }
            else
            {
                slots.Add(baseStalls[i].SupplyType, baseStalls[i].GetBaseSlotCustomers());
            }
        }
    }
    public void UpdateSupplyUnlock()
    {
        listProductUnlock.Clear();
        for (int i = 0; i < baseStalls.Count; i++)
        {
            if (!listProductUnlock.Contains(baseStalls[i].SupplyType))
            {
                listProductUnlock.Add(baseStalls[i].SupplyType);
            }
        }
    }
    public FoodOrderData GenerateOrderData(ProductType _type)
    {
        var productItemData = productDatabase.GetProductItemData(_type);
        int quantity = Random.Range(1, 5);
        return new FoodOrderData
        {
            productType = _type,
            productIcon = productItemData.productIcon,
            quantity = quantity,
            count = 0,
            earn = quantity * productItemData.earn
        };
    }
    public BaseSlotCustomer GetBaseSlotCustomerFree()
    {
        foreach (var item in slots.Values)
        {
            for (int i = 0; i < item.Count; i++)
            {
                if (item[i].GetSlotState() == SlotState.Free)
                    return item[i];
            }
        }
        return null;
    }

    public CustomerSlotWait GetCustomerSlotWaitCash()
    {
        for (int i = 0;i< cashRegisters.Count; i++)
        {
            var _customerSlotWaits = cashRegisters[i].CustomerSlotWaits;
            for (int j = 0; j < _customerSlotWaits.Count; j++)
            {
                if (_customerSlotWaits[j].GetSlotState() == SlotState.Free)
                    return _customerSlotWaits[j];
            }
        }
        return null;
    }
    public void CustomerGotoDespawn(BaseCustomer customer)
    {
        customer.Target = customerSpawner.transform;
    }
    public void DeSpawn()
    {
        currentCustomer--;
    }
}
