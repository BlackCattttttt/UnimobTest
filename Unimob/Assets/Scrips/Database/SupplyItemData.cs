using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SupplyType
{
    Tomato
}

[CreateAssetMenu(menuName = "ScriptableObject/SupplyItemData", fileName = "SupplyItemData.asset")]
public class SupplyItemData : ScriptableObject
{
    public string supplyName;
    public SupplyType type;
    public int earn;
    public ProductItem itemPrefab;
}
