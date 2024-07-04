using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSupply : MonoBehaviour
{
    [SerializeField] protected int maxProductSpawn;
    [SerializeField] protected float delayProductSpawn;
    [SerializeField] protected List<GameObject> listProducts = new List<GameObject>();
    [SerializeField] protected DOTweenAnimation tweenAnimation;

    protected float _currentDelay = 0;
    protected int _currentProduct = 0;
    protected PlayerController _playerController;
    protected int redundant;

    private void Start()
    {
        _currentDelay = 0;
        _currentProduct = 0;
        redundant = 0;
        SetActiveProducts();
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
                _currentProduct++;
                SetActiveProducts();
            }
        }
        else
        {
            tweenAnimation.tween.Pause();
        }
    }

    public void SetActiveProducts()
    {
        for ( int i = 0; i < listProducts.Count; i++ )
        {
            listProducts[i].gameObject.SetActive(i + 1 > _currentProduct ? false : true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerController = other.transform.parent.GetComponent<PlayerController>();
            if (_playerController != null)
            {
                
                _playerController.AddProducts(_currentProduct, out redundant);

                _currentProduct = redundant;
                SetActiveProducts();
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
            if (_playerController != null)
            {
                _playerController.AddProducts(_currentProduct, out redundant);

                _currentProduct = redundant;
                SetActiveProducts();
            }
        }
    }
}
