using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnlockBuilding : MonoBehaviour
{
    [SerializeField] private int cost;
    [SerializeField] private BaseBuilding building;
    [SerializeField] private Transform body;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private ParticleSystem hitParticle;

    private float _delayPay = 0;
    private int _amount = 0;   
    private PlayerController _playerController;
    private bool _havePlayer;

    private void Start()
    {
        _amount = 0;
        costText.text = cost.ToString();
    }
    private void Update()
    {
        if (_havePlayer && _playerController != null)
        {
            _delayPay -= Time.deltaTime;
            if (_amount < cost && _delayPay < 0)
            {
                _delayPay = 0.1f;
                GameManager.Instance.MinusMoney(1, () =>
                {
                    var _money = _playerController.SpendMoney();
                    _money.transform.SetParent(transform, true);
                    _money.MoveToLocalTarget(Vector3.zero, 0.2f, () =>
                    {
                        SimplePool.Despawn(_money.gameObject);
                    });
                    _amount++;
                    costText.text = (cost - _amount).ToString();
                });
            }
        }
        if (_amount >= cost)
        {
            if (hitParticle != null)
            {
                SimplePool.Spawn(hitParticle, transform.position, Quaternion.identity);
            }
            building.ActiveBuiling();
            gameObject.SetActive(false);
        }
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
