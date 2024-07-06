using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSlotWait : MonoBehaviour
{
    [SerializeField] private CashRegister cashRegister;
    [SerializeField] private CustomerSlotWait nextSlotWait;
    [SerializeField] private Transform lookAtTransform;
    [SerializeField] private bool firstSlot;
    [SerializeField] private SlotState slotState;

    public CashRegister CashRegister => cashRegister;
    public Transform LookAtTransform { get => lookAtTransform; }
    public CustomerSlotWait NextSlotWait { get => nextSlotWait; }
    public bool FirstSlot { get => firstSlot; }

    public SlotState GetSlotState() { return slotState; }
    public void SetSlotState(SlotState state) { slotState = state; }
}