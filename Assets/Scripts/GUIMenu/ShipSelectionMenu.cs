using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipSelectionMenu : MonoBehaviour {
    [SerializeField]
    private GameObject infoPlaceHolder, shipPlaceHolder, candado, buttonContinue, buttonUpgrade;

    [SerializeField]
    private GameObject startMenuPrefab, levelSelectionMenuPrefab, loadingPrefab, levelGameMenuPrefab, windowPromptHowToPlay, windowGuidePlay, upgradeMenuPrefab;

    [SerializeField]
    private TextMeshProUGUI textShipName, textLife, textSpeed, textNeutralizer, TextExpansive, textInvencible;


    [SerializeField]
    private Material background;

    private GameController gameController;
    private SoundManager soundManager;

    private GameObject shipInstance, buttonInfo;

    private List<GameObject> shipsInstances = new List<GameObject>();

    private List<PlayerShip> playerShips = new List<PlayerShip>();

    private GUIPlayerShip playerShipUIInfo;

    private Animator animator;
    private CanvasGroup canvasGroupShipMenu, canvasGroupParent;

    private CanvasController canvasController;

    private PromptWindowController promptWindows;

    private GuideHowToPlayMenuController guideHowToPlayController;

    private bool shipShowing;

    private UpgradeShipMenuController upgradeShipMenuController;

    public bool ShipShowing { get => shipShowing; set => shipShowing = value; }

    private void Awake()
    {
        gameController = GameController.Instance;
        soundManager = SoundManager.Instance;

        canvasController = GetComponentInParent<CanvasController>();

        canvasController.SetBackground(background);
        upgradeShipMenuController = upgradeMenuPrefab.GetComponent<UpgradeShipMenuController>();
    }

    internal void StatusButtonContinue(bool status)
    {
        buttonContinue.gameObject.SetActive(status);
    }

    // Start is called before the first frame update
    void Start()
    {
        ShipShowing = true;
        playerShipUIInfo = gameController.PlayerShipUIInfo;
        playerShips = playerShipUIInfo.PlayerShips;
        LoadShips(playerShips);
        animator = GetComponent<Animator>();
        canvasGroupShipMenu = GetComponent<CanvasGroup>();
        infoPlaceHolder.gameObject.SetActive(false);
        soundManager.PlaySelectionShipMusicGame();


        windowPromptHowToPlay = Instantiate(windowPromptHowToPlay, canvasController.transform, false);
        promptWindows = windowPromptHowToPlay.GetComponent<PromptWindowController>();
        promptWindows.SetText("You haven't read the How to Play Guide, Do you want to read it now?");
        windowPromptHowToPlay.gameObject.SetActive(false);
        windowPromptHowToPlay.transform.SetAsLastSibling();

        windowGuidePlay = Instantiate(windowGuidePlay, canvasController.transform, false);
        windowGuidePlay.gameObject.SetActive(false);
        windowGuidePlay.transform.SetAsLastSibling();
        guideHowToPlayController = windowGuidePlay.GetComponent<GuideHowToPlayMenuController>();


        if (gameController.PlayerShipUIInfo.selectedShip)
        {
            ChangeShipSelected(playerShipUIInfo.selectedShip);
        }
        else
        {
            ChangeShipSelected(playerShipUIInfo.PlayerShips[0]);
        }

        promptWindows.OnButtonNoClicked += StartGame;
        promptWindows.OnButtonYesClicked += ShowGuideWindow;

        guideHowToPlayController.OnButtonOkClicked += StartGame;
    }



    internal void ViewShip(bool v)
    {
        canvasController.SetBlockRaycasts(v);
        shipInstance.gameObject.SetActive(v);
    }

    private void Update()
    {
        if (shipInstance)
        {
            shipInstance.transform.Rotate(Vector3.up * 3.0f);
        }
    }

    private void OnDestroy()
    {
        if (promptWindows)
        {
            promptWindows.OnButtonNoClicked -= StartGame;
            promptWindows.OnButtonYesClicked -= ShowGuideWindow;
        }
        if (guideHowToPlayController)
        {
            guideHowToPlayController.OnButtonOkClicked -= StartGame;
        }
        Destroy(windowPromptHowToPlay);
        Destroy(windowGuidePlay);
    }

    private void ShowGuideWindow()
    {
        gameController.ReadHowToPlay = true;
        windowPromptHowToPlay.gameObject.SetActive(false);
        windowGuidePlay.gameObject.SetActive(true);
    }

    private void ChangeShipSelected(PlayerShip ship)
    {
        //upgradeShipMenuController.LoadUpgradesShip();
        //soundManager.PlayMusicClick();
        playerShipUIInfo.selectedShip = ship;
        //if (gameController.GetAvatarsUnlocked().Contains(ship))
        //{
        //textContinue.GetComponent<Button>().interactable = true;
        //candado.SetActive(false);
        ChangeTitleName(ship);
        CreateTextInfoShip(ship);
        gameController.selectedPlayerShip = ship;
        if (shipInstance)
        {
            shipInstance.gameObject.SetActive(false);
        }
        shipInstance = shipsInstances.Find(x => x.gameObject.name == ship.name);

        if (ShipShowing)
        {
            infoPlaceHolder.gameObject.SetActive(false);
            shipInstance.gameObject.SetActive(true);
        }
        else
        {
            infoPlaceHolder.gameObject.SetActive(true);
            shipInstance.gameObject.SetActive(false);
        }

        //}
        //else
        //{
        //    if (avatarInstance)
        //    {
        //        avatarInstance.gameObject.SetActive(false);
        //    }
        //    candado.SetActive(true);
        //    textContinue.GetComponent<Button>().interactable = false;
        //}
    }

    private void LoadShips(List<PlayerShip> shipsToShow)
    {

        //Instanciamos los gameobjects de los players
        for (int i = 0; i < playerShips.Count; i++)
        {
            PlayerShip item = playerShips[i];
            //Load the improves of the ship
            item.Initialize();
            GameObject instanceShip = Instantiate(item.Prefab);
            instanceShip.gameObject.SetActive(false);
            instanceShip.transform.localScale = new Vector3(150.0f, 150.0f, 150.0f);

            instanceShip.GetComponent<PlayerController>().enabled = false;
            instanceShip.GetComponent<KeyboardInputHandler>().enabled = false;
            instanceShip.transform.SetParent(shipPlaceHolder.transform, false);
            instanceShip.name = playerShips[i].name;

            if (!shipsInstances.Contains(instanceShip))
            {
                shipsInstances.Add(instanceShip);
            }
        }
    }

    //Used in GUI
    public void ClickRightArrow()
    {
        soundManager.PlayMusicClick();

        int newId = playerShipUIInfo.selectedShip.IdShip + 1;
        if (newId >= playerShips.Count)
        {
            newId = 0;
        }
        PlayerShip newShip = playerShips[newId];
        ChangeShipSelected(newShip);
    }

    private void ChangeTitleName(PlayerShip playerShip)
    {
        textShipName.text = playerShip.ShipName;
    }

    //Used in GUI
    public void ClickLeftArrow()
    {
        soundManager.PlayMusicClick();

        int newId = playerShipUIInfo.selectedShip.IdShip - 1;
        if (newId < 0)
        {
            newId = playerShips.Count - 1;
        }
        PlayerShip newShip = playerShips[newId];
        ChangeShipSelected(newShip);
    }

    //Used in GUI
    public void ButtonStartClicked()
    {
        soundManager.PlayMusicClick();
        if (gameController.ReadHowToPlay)
        {
            StartGame();
        }
        else
        {
            shipInstance.gameObject.SetActive(false);
            ShowPromptWindowsToRead();
        }
    }

    private void StartGame()
    {
        soundManager.StopSelectionShipMusicGame();
        canvasController.SetBlockRaycasts(true);
        Hide();
        UtilsCanvas.ChangeCanvas(loadingPrefab, gameObject);
    }

    private void SetBlockRaycasts(bool status)
    {
        canvasGroupShipMenu.blocksRaycasts = status;
    }


    private void ShowPromptWindowsToRead()
    {
        canvasController.SetBlockRaycasts(false);
        SetBlockRaycasts(false);

        windowPromptHowToPlay.SetActive(true);
    }

    //Used in GUI
    public void Back()
    {
        soundManager.PlayMusicClick();
        soundManager.StopSelectionShipMusicGame();
        //buttonContinue.SetActive(false);
        //buttonUpgrade.SetActive(false);
        //Debug.Log("continue");
        if (buttonContinue.gameObject.activeInHierarchy)
        {
            if (gameController.GameMode == GameMode.ArcadeMode)
            {
                UtilsCanvas.ChangeCanvas(levelSelectionMenuPrefab, gameObject);
            }
            else
            {
                UtilsCanvas.ChangeCanvas(levelGameMenuPrefab, gameObject);
            }
        }
        else
        {
            UtilsCanvas.ChangeCanvas(levelGameMenuPrefab, gameObject);
        }
    }

    private void Hide()
    {
        animator.SetBool("Shown", false);

        canvasGroupShipMenu.interactable = false;
    }

    //Used in GUI
    public void ShowInfo()
    {
        soundManager.PlayMusicClick();

        SetPlaceHolderStatus();
        ShipShowing = !ShipShowing;
    }

    private void SetPlaceHolderStatus()
    {
        if (ShipShowing)
        {
            infoPlaceHolder.gameObject.SetActive(true);
            shipInstance.gameObject.SetActive(false);
        }
        else
        {
            shipInstance.gameObject.SetActive(true);
            infoPlaceHolder.gameObject.SetActive(false);
        }
    }

    private void CreateTextInfoShip(PlayerShip ship)
    {
        SetInfoText(textLife, FeatureToImprove.Life.ToString(), ship.Life.ToString(), ship.LifeImprove);
        SetInfoText(textSpeed, FeatureToImprove.Speed.ToString(), ship.Speed.ToString(), ship.SpeedImprove);
        SetInfoText(TextExpansive, "Cadence Expansive ", ship.WaitTimeExpansiveWave + "s", 0);
        SetInfoText(textNeutralizer, "Cadence Neutralize", ship.WaitTimeNeutralizer + "s", 0);
        SetInfoText(textInvencible, "Invencible time", ship.WaitTimeUntilNextHit + "s", 0);
    }

    private void SetInfoText(TextMeshProUGUI newText, string title, string value, float upgradeValue)
    {
        newText.text = String.Format("{0} {1} +{2}", title, value, upgradeValue);
    }

    public void UpgradeButton()
    {
        soundManager.PlayMusicClick();

        //Show
        ViewShip(false);
        canvasController.SetBlockRaycasts(false);
        upgradeMenuPrefab.gameObject.SetActive(true);
        upgradeShipMenuController.LoadShipValues();

    }

    internal void ResetInfoShip()
    {
        CreateTextInfoShip(gameController.selectedPlayerShip);
    }
}
