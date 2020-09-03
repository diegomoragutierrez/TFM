using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpItemBoughtController : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField]
    private StoreItem storeItem;
    SoundManager soundManager;

    [SerializeField]
    private TextMeshProUGUI quantityOfFearuresText, improveValueOfFeatureText;

    private GameController gameController;

    public StoreItem StoreItem { get => storeItem; }

    private PowerUpItem powerUp;

    public event Action<PowerUpItem> OnUsedItem = delegate { };

    public void Init()
    {
        Awake();
    }
    private void Awake()
    {
        gameController = GameController.Instance;
        powerUp = (PowerUpItem)StoreItem;

        improveValueOfFeatureText.text = string.Format("+{0}", powerUp.QuantityOfUpgrade.ToString());
        soundManager = SoundManager.Instance;
        //CheckIfInteractive();
        //GetValuesQuantityOfPowerUp();
    }

    internal bool CanUpgradeFeatures()
    {
        PlayerShip ship = gameController.selectedPlayerShip;
        bool interactive = true;
        switch (powerUp.Feature)
        {
            case FeatureToImprove.Life:
                {
                    if (powerUp.QuantityOfUpgrade + ship.LifeImprove + ship.Life > ship.MaxLifeImprove)
                    {
                        interactive = false;
                    }
                    break;
                }
            case FeatureToImprove.Speed:
                {
                    if (powerUp.QuantityOfUpgrade + ship.SpeedImprove + ship.Speed > ship.MaxSpeedImprove)
                    {
                        interactive = false;
                    }
                    break;
                }

            default:
                throw new Exception("Unexpected Case");
        }
        GetComponent<Button>().interactable = interactive;
        return interactive;
    }

    internal int GetValuesQuantityOfPowerUp()
    {
        int quantity = gameController.PersistentData.GetQuantityUpgrade(powerUp);

        quantityOfFearuresText.SetText("{0} uds", quantity);
        return quantity;
    }

    internal void SetInteractable(bool status)
    {
        GetComponent<Button>().interactable = status;
    }

    //Used in GUI
    public void AddPowerUp()
    {
        PlayerShip playerShip = gameController.selectedPlayerShip;
        soundManager.PlayMusicClick();

        //Add pair ship upgrade
        gameController.PersistentData.AddUpgradeToShip(playerShip, powerUp);

        gameController.PersistentData.RemoveUpgradeFromUser(powerUp);
        playerShip.AddPowerUp(powerUp.Feature);
      
        //Save in PersistentData
        gameController.SaveInformation();
        OnUsedItem(powerUp);
    }
}
