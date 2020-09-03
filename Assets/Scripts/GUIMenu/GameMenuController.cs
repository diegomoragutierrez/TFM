using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour {
    [SerializeField]
    private GameObject startMenuPrefab, mapMenuPrefab, selectionShipPrefab, storeMenuPrefab, historyModeMenu, mainMenu;

    private SoundManager soundManager;


    private Animator animator;
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Material background;

    private CanvasController canvasController;

    private GameController gameController;

    private ShipSelectionMenu shipSelectionMenu;

    private void Awake()
    {
        soundManager = SoundManager.Instance;
        canvasController = GetComponentInParent<CanvasController>();
        gameController = GameController.Instance;
        canvasController.SetBackground(background);
        historyModeMenu.GetComponent<HistoryMenuController>().OnCloseMenu += OnCloseHistoryMenu;
        historyModeMenu.GetComponent<HistoryMenuController>().OnStartGameHistoryMode += OnStartGameHistoryMode;
        shipSelectionMenu = selectionShipPrefab.GetComponent<ShipSelectionMenu>();
    }

    private void OnCloseHistoryMenu()
    {
        mainMenu.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        canvasController.SetCreditsTextState(true);
        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
        animator.SetBool("Shown", true);
        mainMenu.gameObject.SetActive(true);
        historyModeMenu.gameObject.SetActive(false);
        soundManager.PlayGameMenuMusic();
    }

    //Used in GUI
    public void SeeVehicles()
    {
        soundManager.PlayMusicClick();
        soundManager.StopGameMenuMusic();
        shipSelectionMenu.StatusButtonContinue(false);
        UtilsCanvas.ChangeCanvas(selectionShipPrefab, gameObject);
        Hide();
    }

    //Used in GUI
    public void ButtonStore()
    {
        soundManager.PlayMusicClick();
        soundManager.StopGameMenuMusic();
        UtilsCanvas.ChangeCanvas(storeMenuPrefab, gameObject);
        Hide();
    }


    //Used in GUI
    public void HistoryModeButton()
    {
        soundManager.PlayMusicClick();

        mainMenu.gameObject.SetActive(false);
        shipSelectionMenu.StatusButtonContinue(true);

        historyModeMenu.SetActive(true);
        gameController.GameMode = GameMode.HistoryMode;
    }

    //Used in GUI
    public void ArcadeModeButton()
    {
        soundManager.PlayMusicClick();
        soundManager.StopGameMenuMusic();
        shipSelectionMenu.StatusButtonContinue(true);
        gameController.GameMode = GameMode.ArcadeMode;

        UtilsCanvas.ChangeCanvas(mapMenuPrefab, gameObject);
        Hide();
    }



    internal void Hide()
    {
        animator.SetBool("Shown", false);
        soundManager.StopGameMenuMusic();

        canvasGroup.interactable = false;
    }

    //Used in GUI
    public void Back()
    {
        soundManager.PlayMusicClick();
        soundManager.StopGameMenuMusic();
        canvasController.SetCreditsTextState(false);

        //Debug.Log("continue");
        UtilsCanvas.ChangeCanvas(startMenuPrefab, gameObject);

    }

    private void OnDestroy()
    {
        if (historyModeMenu)
        {
            historyModeMenu.GetComponent<HistoryMenuController>().OnCloseMenu += OnCloseHistoryMenu;
            historyModeMenu.GetComponent<HistoryMenuController>().OnStartGameHistoryMode -= OnStartGameHistoryMode;
        }
    }

    private void OnStartGameHistoryMode()
    {
        soundManager.StopGameMenuMusic();

        UtilsCanvas.ChangeCanvas(selectionShipPrefab, gameObject);
        shipSelectionMenu.StatusButtonContinue(true);
    }
}
