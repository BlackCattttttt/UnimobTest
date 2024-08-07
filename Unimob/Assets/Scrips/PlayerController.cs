using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseCharacter
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private DynamicJoystick joystick;
    [SerializeField] protected PlayerCarry playerCarry;
    [SerializeField] private Money moneyPrefab;

    private float _turnSmoothVelocity;
    private Transform _cameraTransform;

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

        characterAnimator.SetBool(IsEmpty, _isEmpty);
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
  
    }
    public bool CheckProduct(ProductType productItem)
    {
        return playerCarry.CheckProduct(productItem);
    }
    public void AddProducts(int quantity, List<ProductItem> productItems, out int redundant)
    {
        playerCarry.AddProduct(quantity, productItems, out redundant);
    }
    public void RemoveProductsToStall(int capacity, out List<ProductItem> productItems)
    {
        playerCarry.RemoveProductsToStall(capacity, out productItems);
    }
    public void AddMoney(Money money)
    {
        money.transform.SetParent(playerCarry.transform, true);
        money.MoveToLocalTarget(Vector3.zero, 0.3f, () =>
        {
            GameManager.Instance.AddMoney(1);
            Destroy(money.gameObject);
        });
    }
    public Money SpendMoney()
    {
        var _money = SimplePool.Spawn(moneyPrefab, Vector3.zero, Quaternion.identity);
        _money.transform.SetParent(playerCarry.transform, false);

        return _money;
    }
}
