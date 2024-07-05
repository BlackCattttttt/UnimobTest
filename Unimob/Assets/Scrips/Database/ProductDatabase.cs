using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObject/ProductDatabase", fileName = "ProductDatabase.asset")]
public class ProductDatabase : ScriptableObject
{
    public List<ProductItemData> datas = new List<ProductItemData>();

    public Sprite GetProductIcon(ProductType productType)
    {
        return datas.Find(x => x.productType == productType).productIcon;
    }

    [Serializable]
    public class ProductItemData 
    {
        public ProductType productType;
        public Sprite productIcon;
        public int earn;
    }
}
