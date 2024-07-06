using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private BaseCustomer customerPrefab;
    [SerializeField] private List <Material> materials;

    public BaseCustomer SpawnCustomer()
    {
        BaseCustomer _customer = SimplePool.Spawn(customerPrefab, Vector3.zero, Quaternion.identity);
        _customer.transform.SetParent(transform, false);
        _customer.transform.localPosition = Vector3.zero;
        _customer.SetMaterial(materials[Random.Range(0, materials.Count)]);

        return _customer;
    }
}
