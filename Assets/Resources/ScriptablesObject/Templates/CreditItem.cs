using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CreditItemInstance", menuName = "CreditItem")]
public class CreditItem : StoreItem {

    [SerializeField]
    private int creditsToBuy;

    [SerializeField]
    private float realMoneyCost;

    public float RealMoneyCost { get => realMoneyCost; set => realMoneyCost = value; }
    public int CreditsToBuy { get => creditsToBuy; set => creditsToBuy = value; }

    public void Init()
    {
        Type = StoreItemType.Credit;
    }
}
