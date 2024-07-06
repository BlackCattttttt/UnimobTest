using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CashRegister : MonoBehaviour
{
    [SerializeField] private List<CustomerSlotWait> customerSlotWaits = new List<CustomerSlotWait>();
    [SerializeField] private Transform boxPoint;
    [SerializeField] private Box boxPrefab;

    private Box _currentBox;
    private BaseCustomer _currentCustomer;
    public List<CustomerSlotWait> CustomerSlotWaits { get { return customerSlotWaits; } set { customerSlotWaits = value; } }

    private bool _havePlayer;

    public bool HavePlayer => _havePlayer;

    public void SpawnBox(BaseCustomer currentCustomer)
    {
        _currentCustomer = currentCustomer;
        _currentBox = SimplePool.Spawn(boxPrefab, Vector3.zero, Quaternion.identity);
        _currentBox.transform.SetParent(boxPoint, false);
        _currentBox.Init(this);
    }
    public void AddProduct(ProductItem product, bool last)
    {
        if (_currentBox != null)
        {
            _currentBox.AddBox(product, last);
        }
    }
    public void RemoveBox()
    {
        if (_currentCustomer != null)
        {
            _currentCustomer.AddBox(_currentBox);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _havePlayer = true;
            var _playerController = other.transform.parent.GetComponent<PlayerController>();
            if (_playerController != null)
            {
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _havePlayer = false;
            var _playerController = other.transform.parent.GetComponent<PlayerController>();
            if (_playerController != null)
            {
            }
        }
    }
}
