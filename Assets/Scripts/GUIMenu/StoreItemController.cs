using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreItemController : MonoBehaviour {
    SoundManager soundManager;

    [SerializeField]
    private StoreItem storeItem;

    [SerializeField]
    private TextMeshProUGUI quantityText, priceText;

    public StoreItem StoreItem { get => storeItem; }

    public event Action<StoreItem> OnBuyItem = delegate { };
    private void Start()
    {
        soundManager = SoundManager.Instance;

        if (StoreItem.Type == StoreItemType.PowerUp)
        {
            PowerUpItem powerUp = (PowerUpItem)StoreItem;
            quantityText.text = string.Format("+{0}", powerUp.QuantityOfUpgrade.ToString());
            priceText.text = string.Format("{0} pts", powerUp.CreditCost.ToString());
        }
        else if (StoreItem.Type == StoreItemType.Credit)
        {
            CreditItem credit = (CreditItem)StoreItem;
            quantityText.text = string.Format("+{0}", credit.CreditsToBuy.ToString());
            priceText.text = string.Format("{0} €", credit.RealMoneyCost.ToString());
        }
    }

    //Used in GUI
    public void Purchase()
    {
        soundManager.PlayMusicClick();

        OnBuyItem(storeItem);
    }
}

