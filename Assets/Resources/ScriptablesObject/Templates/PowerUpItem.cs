using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FeatureToImprove {
    Life,
    Speed,
}

[CreateAssetMenu(fileName = "PowerUpsInstance", menuName = "PowerUp")]
public class PowerUpItem : StoreItem {

    [SerializeField]
    private FeatureToImprove feature;

    [SerializeField]
    private float quantityOfUpgrade;

    [SerializeField]
    private int pointsToBuy;

    public float QuantityOfUpgrade { get => quantityOfUpgrade; set => quantityOfUpgrade = value; }
    public FeatureToImprove Feature { get => feature; set => feature = value; }
    public int CreditCost { get => pointsToBuy; set => pointsToBuy = value; }

    public void Init()
    {
        Type = StoreItemType.PowerUp;
    }
}
