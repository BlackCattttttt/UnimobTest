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
    }
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private BehaviourTreeOwner behaviourTree;
    [SerializeField] private CustomerUI customerUI;
    [SerializeField] private CustomerCarry customerCarry;
    public float damping = 6;

    public Transform Target
    {
        get
        {
            if (_currentSlot != null)
                return _currentSlot.transform;
            return transform;
        }
    }
    private FoodOrderData _orderData;
    private BaseStore _store;
    private BaseSlotCustomer _currentSlot;
    private bool lookAtTarget;

    private void Update()
    {
        if (customerCarry.CurrentProductsCarry > 0)
        {
            _isEmpty = false;
        }
        else
        {
            _isEmpty = true;
        }

        characterAnimator.SetBool(IsEmpty, _isEmpty);
        if (lookAtTarget)
        {
            // Look at and dampen the rotation
            var rotation = Quaternion.LookRotation(_currentSlot.LookAtTransform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        }
    }
    public void SetTargetSlot(BaseSlotCustomer slot, FoodOrderData orderData, BaseStore store)
    {
        _currentSlot = slot;
        _orderData = orderData;
        _store = store;
        _isEmpty = true;

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
    public void OnGotoSlotStall()
    {
        if (_currentSlot.Stall != null)
        {
            _currentSlot.Stall.AddCustomer(this);
        }

        lookAtTarget = true;
        characterAnimator.SetBool(_isEmpty ? IsMove : IsCarryMove, false);
    }
    public bool CheckHasOrder()
    {
        if (_orderData.quantity > 0)
        {
            return true;
        }
        return false;
    }
    public void ReceiveOrder(ProductItem product)
    {
        _orderData.quantity--;
        customerCarry.AddProduct(product);  
        customerUI.UpdateData(_orderData.quantity);
    }
}
