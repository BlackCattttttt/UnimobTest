using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private DynamicJoystick joystick;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private PlayerCarry playerCarry;

    private static readonly int IsMove = Animator.StringToHash("IsMove");
    private static readonly int IsCarryMove = Animator.StringToHash("IsCarryMove");
    private static readonly int IsEmpty = Animator.StringToHash("IsEmpty");

    private float _turnSmoothVelocity;
    private Transform _cameraTransform;
    private bool _isEmpty;

    private void Awake()
    {
        _cameraTransform = Camera.main.transform;
    }
    private void Update()
    {
        if (playerCarry.CurrentProductsCarry > 0)
        {
            _isEmpty = false;
        }
        else
        {
            _isEmpty = true;
        }

        var direction = new Vector3(joystick.Horizontal, 0f, joystick.Vertical).normalized;
        var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;

        if (direction.sqrMagnitude >= 0.01f)
        {
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            agent.Move(moveDirection.normalized * speed * Time.deltaTime);
            characterAnimator.SetBool(_isEmpty ? IsMove : IsCarryMove, true);
        }
        else
        {
            characterAnimator.SetBool(_isEmpty ? IsMove : IsCarryMove, false);
        }

        characterAnimator.SetBool(IsEmpty, _isEmpty);      
    }
    public void AddProducts(int quantity, out int redundant)
    {
        playerCarry.AddProduct(quantity, out redundant);
    }
}
