using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeShipMenuController : MonoBehaviour {
    GameController gameController;
    SoundManager soundManager;
    private ShipSelectionMenu shipSelectionMenu;
    [SerializeField]
    private GameObject itemsLifePlaceHolder, itemsSpeedPlaceHolder;
    [SerializeField]
    private TextMeshProUGUI textMaxLife, textMaxSpeed;

    private CanvasController canvasController;
    private PowerUpItem itemToBuy;
    private PlayerShip ship;
    private CanvasGroup strenghtCanvasGroup, speedCanvasGroup;

    // Start is called before the first frame update
    void Awake()
    {
        gameController = GameController.Instance;
        soundManager = SoundManager.Instance;
        shipSelectionMenu = GetComponentInParent<ShipSelectionMenu>();
        canvasController = GetComponentInParent<CanvasController>();
        strenghtCanvasGroup = itemsLifePlaceHolder.GetComponent<CanvasGroup>();
        speedCanvasGroup = itemsSpeedPlaceHolder.GetComponent<CanvasGroup>();
    }
    private void CheckItems()
    {
        ship = gameController.selectedPlayerShip;

        textMaxLife.gameObject.SetActive(ship.Life + ship.LifeImprove == ship.MaxLifeImprove);
        textMaxSpeed.gameObject.SetActive(ship.Speed + ship.SpeedImprove == ship.MaxSpeedImprove);


        bool activateStrengPlaceholder = false;

        RectTransform[] array = itemsLifePlaceHolder.GetComponentsInChildren<RectTransform>();
        for (int i = 0; i < array.Length; i++)
        {
            RectTransform item = array[i];
            PowerUpItemBoughtController powerUpItemBoughtController = item.GetComponent<PowerUpItemBoughtController>();
            if (item.gameObject != itemsLifePlaceHolder.gameObject && powerUpItemBoughtController)
            {
                powerUpItemBoughtController.Init();
                powerUpItemBoughtController.OnUsedItem += OnUsedItem;
                if (powerUpItemBoughtController.GetValuesQuantityOfPowerUp() > 0 && powerUpItemBoughtController.CanUpgradeFeatures())
                {
                    powerUpItemBoughtController.SetInteractable(true);
                    activateStrengPlaceholder = true;
                }
                else
                {
                    powerUpItemBoughtController.SetInteractable(false);
                }
            }
        }
        strenghtCanvasGroup.interactable = activateStrengPlaceholder;

        bool activateSpeedPlaceholder = false;
        RectTransform[] array1 = itemsSpeedPlaceHolder.GetComponentsInChildren<RectTransform>();
        for (int i = 0; i < array1.Length; i++)
        {
            RectTransform item = array1[i];
            PowerUpItemBoughtController powerUpItemBoughtController = item.GetComponent<PowerUpItemBoughtController>();
            if (item.gameObject != itemsSpeedPlaceHolder.gameObject && powerUpItemBoughtController)
            {
                powerUpItemBoughtController.OnUsedItem += OnUsedItem;
                if (powerUpItemBoughtController.GetValuesQuantityOfPowerUp() > 0 && powerUpItemBoughtController.CanUpgradeFeatures())
                {
                    powerUpItemBoughtController.SetInteractable(true);
                    activateSpeedPlaceholder = true;

                }
                else
                {
                    powerUpItemBoughtController.SetInteractable(false);
                }
            }

        }
        speedCanvasGroup.interactable = activateSpeedPlaceholder;
    }

    private void OnDestroy()
    {
        RectTransform[] array = itemsLifePlaceHolder.GetComponentsInChildren<RectTransform>();
        for (int i = 0; i < array.Length; i++)
        {
            RectTransform item = array[i];
            if (item.gameObject != itemsLifePlaceHolder.gameObject && item.GetComponent<PowerUpItemBoughtController>())
            {
                item.GetComponent<PowerUpItemBoughtController>().OnUsedItem -= OnUsedItem;
            }
        }

        RectTransform[] array1 = itemsSpeedPlaceHolder.GetComponentsInChildren<RectTransform>();
        for (int i = 0; i < array1.Length; i++)
        {
            RectTransform item = array1[i];
            if (item.gameObject != itemsSpeedPlaceHolder.gameObject && item.GetComponent<PowerUpItemBoughtController>())
            {
                item.GetComponent<PowerUpItemBoughtController>().OnUsedItem -= OnUsedItem;
            }
        }
    }

    private void OnUsedItem(PowerUpItem powerUp)
    {
        PlayerShip ship = gameController.selectedPlayerShip;

        switch (powerUp.Feature)
        {
            case FeatureToImprove.Life:
                {
                    if (ship.LifeImprove + ship.Life >= ship.MaxLifeImprove)
                    {
                        strenghtCanvasGroup.interactable = false;
                        textMaxLife.gameObject.SetActive(true);
                    }
                    else
                    {
                        textMaxLife.gameObject.SetActive(false);
                    }
                    break;
                }
            case FeatureToImprove.Speed:
                {
                    if (ship.SpeedImprove + ship.Speed >= ship.MaxSpeedImprove)
                    {
                        speedCanvasGroup.interactable = false;
                        textMaxSpeed.gameObject.SetActive(true);
                    }
                    else
                    {

                        textMaxSpeed.gameObject.SetActive(false);
                    }
                    break;
                }

            default:
                throw new Exception("Unexpected Case");
        }

        CheckItems();
    }


    public void CloseButton()
    {
        soundManager.PlayMusicClick();
        shipSelectionMenu.ViewShip(shipSelectionMenu.ShipShowing);
        canvasController.SetBlockRaycasts(true);
        shipSelectionMenu.ResetInfoShip();
        gameObject.SetActive(false);
    }

    internal void LoadShipValues()
    {
        CheckItems();
    }
}
