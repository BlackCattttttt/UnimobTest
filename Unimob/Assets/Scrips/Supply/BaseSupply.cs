using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSupply : BaseBuilding
{
    [SerializeField] protected SupplyItemData supplyItemData;
    [SerializeField] protected int maxProductSpawn;
    [SerializeField] protected float delayProductSpawn;
    [SerializeField] protected DOTweenAnimation tweenAnimation;
    [SerializeField] protected List<Transform> spawnPoints = new List<Transform>();

    protected float _currentDelay = 0;
    protected int _currentProduct = 0;
    protected PlayerController _playerController;
    protected int redundant;
    protected List<ProductItem> listProducts = new List<ProductItem>();

    private void Start()
    {
        _currentDelay = 0;
        _currentProduct = 0;
        redundant = 0;
    }

    private void Update()
    {
        if (_currentProduct < maxProductSpawn)
        {
            if (!tweenAnimation.tween.IsPlaying())
                tweenAnimation.tween.Play();
            _currentDelay += Time.deltaTime;
            if (_currentDelay >= delayProductSpawn )
            {
                _currentDelay = 0;
//_currentProduct++;
                SpawnProduct();
            }
        }
        else
        {
            tweenAnimation.tween.Pause();
        }
    }
    public void SpawnProduct()
    {
        var _productItem = SimplePool.Spawn(supplyItemData.itemPrefab, Vector3.zero, Quaternion.identity);
        _productItem.transform.SetParent(spawnPoints[_currentProduct], false);
        _productItem.transform.localPosition = Vector3.up * -2f;
        _productItem.MoveToTarget(spawnPoints[_currentProduct].position,0.3f, () =>
        {
            _currentProduct++;
            listProducts.Add(_productItem);
        });
    }
    //public void SetActiveProducts()
    //{
    //    if (_currentProduct > listProducts.Count)
    //    {

    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerController = other.transform.parent.GetComponent<PlayerController>();
            if (_playerController != null && _playerController.CheckProduct(supplyItemData.type))
            {               
                _playerController.AddProducts(_currentProduct,listProducts, out redundant);

                _currentProduct = redundant;
                //SetActiveProducts();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_playerController == null )
            {
                _playerController = other.transform.parent.GetComponent<PlayerController>();
            }
            if (_playerController.CheckProduct(supplyItemData.type))
            {
                _playerController.AddProducts(_currentProduct, listProducts, out redundant);

                _currentProduct = redundant;
               // SetActiveProducts();
            }
        }
    }

    public override void ActiveBuiling()
    {
        gameObject.SetActive(true); 
    }
}
