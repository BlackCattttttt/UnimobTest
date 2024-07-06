using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseCustomer;

public class BaseStore : MonoBehaviour
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
       // ProductType _type = listProductUnlock[Random.Range(0, listProductUnlock.Count)];
        return new FoodOrderData
        {
            productType = _type,
            productIcon = productDatabase.GetProductIcon(_type),
            quantity = Random.Range(1, 5),
            count = 0
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
}
