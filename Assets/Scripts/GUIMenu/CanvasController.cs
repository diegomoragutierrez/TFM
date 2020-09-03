using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {

    [SerializeField]
    private GameObject optionsMenu, gearButton, backgroundObject;

    private SoundManager soundManager;

    private CanvasGroup canvasGroup;

    private GameController gameController;

    [SerializeField]
    private TextMeshProUGUI creditsQuantity;

    private void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Screen.orientation = ScreenOrientation.Landscape;
        }
        soundManager = SoundManager.Instance;
        gameController = GameController.Instance;
        gameController.Initialize();
        canvasGroup = GetComponent<CanvasGroup>();
        UpdateCredits();
    }

    // Start is called before the first frame update
    void Start()
    {
        optionsMenu.GetComponent<OptionsMenuController>().OnCloseMenu += HideMenuOptions;
    }

    internal void SetCreditsTextState(bool v)
    {
        creditsQuantity.gameObject.SetActive(v);
    }

    private void OnDestroy()
    {
        if (optionsMenu)
        {
            optionsMenu.GetComponent<OptionsMenuController>().OnCloseMenu -= HideMenuOptions;
        }
    }

    internal void SetBackground(Material background)
    {
        backgroundObject.GetComponent<Image>().material = background;
    }

    //Used in GUI
    public void ShowOptionsMenu()
    {
        soundManager.PlayMusicClick();
        SetOptionsMenu(true);

        optionsMenu.transform.SetAsLastSibling();
        ShipSelectionMenu shipSelectionMenu = gameObject.GetComponentInChildren<ShipSelectionMenu>();
        if (shipSelectionMenu)
        {
            shipSelectionMenu.ViewShip(false);
        }
        SetBlockRaycasts(false);
    }

    public void HideMenuOptions()
    {
        SetOptionsMenu(false);
        ShipSelectionMenu shipSelectionMenu = gameObject.GetComponentInChildren<ShipSelectionMenu>();

        if (shipSelectionMenu && shipSelectionMenu.ShipShowing)
        {
            shipSelectionMenu.ViewShip(true);
        }
        SetBlockRaycasts(true);
    }



    internal void SetBlockRaycasts(bool status)
    {
        canvasGroup.blocksRaycasts = status;
    }

    internal void SetOptionsMenu(bool status)
    {
        optionsMenu.SetActive(status);
    }

    internal void UpdateCredits()
    {
        creditsQuantity.text = String.Format("Credits: {0} MCs", gameController.PersistentData.Credits);
    }

    public void AddCredits()
    {
        gameController.PersistentData.Credits += 9999;
        gameController.SaveInformation();
        UpdateCredits();
    }


}
