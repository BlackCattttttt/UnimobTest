using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlotState
{
    Free,
    Close
}
public class BaseSlotCustomer : MonoBehaviour
{
    [SerializeField] private SlotState slotState;
    [SerializeField] private Transform lookAtTransform;
    [SerializeField] private BaseStall stall;

    public Transform LookAtTransform { get => lookAtTransform; }
    public BaseStall Stall { get => stall; }

    public SlotState GetSlotState() { return slotState; }
    public void SetSlotState(SlotState state) { slotState = state; }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
