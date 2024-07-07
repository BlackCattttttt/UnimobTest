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
    [SerializeField] private Transform moneyPoint; 
    [SerializeField] private float delayPay = 0.3f;

    private Box _currentBox;
    private BaseCustomer _currentCustomer;
    private int _currentMoney = 0;
    private List<Money> _moneyList = new List<Money>();
    private float _delayPay = 0;
    private PlayerController _playerController;
    public List<CustomerSlotWait> CustomerSlotWaits { get { return customerSlotWaits; } set { customerSlotWaits = value; } }

    private bool _havePlayer;
    public bool HavePlayer => _havePlayer && (_currentBox != null && _currentBox.boxFinish);

    private void Update()
    {
        if (_havePlayer && _playerController != null)
        {
            _delayPay -= Time.deltaTime;
            if (_delayPay < 0 && _moneyList.Count > 0)
            {
                _delayPay = delayPay;
                var _money = _moneyList[_moneyList.Count - 1];
                _playerController.AddMoney(_money);

                _moneyList.Remove(_money);
                _currentMoney--;
            }
        }
    }
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
            _currentBox = null;
        }
    }

    public void AddMoney(Money money)
    {
        money.transform.SetParent(moneyPoint, true);
        money.transform.localEulerAngles = Vector3.zero;
        money.MoveToLocalTarget(GetMoneyPosition(_currentMoney), 0.3f, () =>
        {
            _moneyList.Add(money);
        });
        _currentMoney++;
    }
    public Vector3 GetMoneyPosition(int index)
    {
        Vector3Int pos3Int = new Vector3Int(index % 10 / 5, index % 10 % 5, index / 10);
        return new Vector3(moneyPoint.position.x * -pos3Int.y * 0.04f, moneyPoint.position.y * pos3Int.z * 0.1f, moneyPoint.position.z * -pos3Int.x * 0.35f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _havePlayer = true;
             _playerController = other.transform.parent.GetComponent<PlayerController>();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _havePlayer = false;
            _playerController = null;
        }
    }
}
