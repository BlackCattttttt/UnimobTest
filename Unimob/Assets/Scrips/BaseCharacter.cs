using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseCharacter : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Animator characterAnimator;

    protected static readonly int IsMove = Animator.StringToHash("IsMove");
    protected static readonly int IsCarryMove = Animator.StringToHash("IsCarryMove");
    protected static readonly int IsEmpty = Animator.StringToHash("IsEmpty");

    protected bool _isEmpty;
}
