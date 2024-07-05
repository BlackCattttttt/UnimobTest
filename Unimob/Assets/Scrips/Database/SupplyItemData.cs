using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ProductType
{
    Tomato
}

[CreateAssetMenu(menuName = "ScriptableObject/SupplyItemData", fileName = "SupplyItemData.asset")]
public class SupplyItemData : ScriptableObject
{
    public string supplyName;
    public ProductType type;
    public ProductItem itemPrefab;
}
