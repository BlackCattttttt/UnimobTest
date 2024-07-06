using NodeCanvas.BehaviourTrees;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCustomer : BaseCharacter
{
    public class FoodOrderData
    {
        public ProductType productType;
        public Sprite productIcon;
        public int quantity;
        public int earn;
        public int count;
    }
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private BehaviourTreeOwner behaviourTree;
    [SerializeField] private CustomerUI customerUI;
    [SerializeField] private CustomerCarry customerCarry;
    [SerializeField] private float damping = 6;
    [SerializeField] private float delayPay = 0.3f;

    public Transform Target { get; protected set; }
    private Transform lookAtTargetTrans;
    private FoodOrderData _orderData;
    private BaseStore _store;
    private BaseSlotCustomer _currentSlot;
    private CustomerSlotWait _currentSlotWait;
    private CashRegister _cashRegister;
    private float _delayPay = 0;
    private bool lookAtTarget;
    private bool canPay = false;

    private void OnEnable()
    {
        canPay = false;
    }

    private void Update()
    {
        if (customerCarry.CheckEmpty())
        {
            _isEmpty = true;
        }
        else
        {
            _isEmpty = false;
        }

        characterAnimator.SetBool(IsEmpty, _isEmpty);
        if (lookAtTarget)
        {
            // Look at and dampen the rotation
            var rotation = Quaternion.LookRotation(lookAtTargetTrans.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        }

        if (canPay && _cashRegister.HavePlayer)
        {
            _delayPay -= Time.deltaTime;
            if (_delayPay < 0)
            {
                _delayPay = delayPay;
                var _product = customerCarry.RemoveProduct(out bool last);
                if (_product != null)
                {
                    _cashRegister.AddProduct(_product, last);
                }
            }
        }
    }
    public void SetTargetSlot(BaseSlotCustomer slot, FoodOrderData orderData, BaseStore store)
    {
        _currentSlot = slot;
        _orderData = orderData;
        _store = store;
        _isEmpty = true;

        Target = _currentSlot.transform;

        customerUI.SetData(_orderData.productIcon, _orderData.quantity);

        behaviourTree.StartBehaviour();
    }
    public void SetMaterial(Material material)
    {
        skinnedMeshRenderer.material = material;
    }
    public void PlayMove()
    {
        lookAtTarget = false;
        characterAnimator.SetBool(_isEmpty ? IsMove : IsCarryMove, true);
    }
    public void PlayIdle()
    {
        lookAtTarget = true;
        characterAnimator.SetBool(_isEmpty ? IsMove : IsCarryMove, false);
    }
    public void OnGotoSlotStall()
    {
        if (_currentSlot.Stall != null)
        {
            _currentSlot.Stall.AddCustomer(this);
        }

        lookAtTargetTrans = _currentSlot.LookAtTransform;
    }
    public bool CheckHasOrder()
    {
        if (_orderData.count < _orderData.quantity)
        {
            return true;
        }
        return false;
    }
    public bool CheckFinishPay()
    {
        if (customerCarry.CurrentProductsCarry == 0)
            return true;
        return false;
    }
    public void ReceiveOrder(ProductItem product)
    {
        _orderData.count++;
        customerCarry.AddProduct(product);
        customerUI.UpdateData(_orderData.count);
    }
    public bool CheckHaveSlot()
    {
        _currentSlotWait = _store.GetCustomerSlotWaitCash();
        if (_currentSlotWait != null)
        {
            lookAtTarget = false;
            Target = _currentSlotWait.transform;
            _cashRegister = _currentSlotWait.CashRegister;
            _currentSlotWait.SetSlotState(SlotState.Close);
            customerUI.SetCashierActive();
            return true;
        }
        return false;
    }
    public bool CheckNextSlot()
    {
        if (_currentSlotWait != null)
        {
            if (_currentSlotWait.FirstSlot)
                return true;
            bool res = (_currentSlotWait.NextSlotWait.GetSlotState() == SlotState.Free);
            if (res)
            {
                Target = _currentSlotWait.NextSlotWait.transform;
                _currentSlotWait.SetSlotState(SlotState.Free);
                _currentSlotWait.NextSlotWait.SetSlotState(SlotState.Close);
            }
            return res;
        }
        return false;
    }
    public bool CheckFirstSlot()
    {
        if (_currentSlotWait != null)
            return _currentSlotWait.FirstSlot;
        return false;
    }
    public void GoToNextWaitSlot()
    {
        lookAtTargetTrans = _currentSlotWait.LookAtTransform;
        if (!_currentSlotWait.FirstSlot)
            _currentSlotWait = _currentSlotWait.NextSlotWait;
    }
    public void GotoWaitSlot()
    {
        lookAtTargetTrans = _currentSlotWait.LookAtTransform;
    }
    public void OnGotoFirstSlot()
    {
        canPay = true;
        if (_cashRegister != null)
        {
            _cashRegister.SpawnBox(this);
        }
    }
    public void AddBox(Box box)
    {
        customerCarry.AddBox(box, () =>
        {

        });
    }
}
