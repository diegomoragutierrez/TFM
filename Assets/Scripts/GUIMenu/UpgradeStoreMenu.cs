using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class UpgradeStoreMenu : MonoBehaviour {
    private GameController gameController;
    private SoundManager soundManager;

    [SerializeField]
    private Material background;

    [SerializeField]
    private GameObject propmtWindowsPrefab, gameMenuPrefab, creditsMenuPrefab;


    [SerializeField]
    private List<GameObject> itemsStore;

    private PromptWindowController windowsToBuyUpgrade, WindowsToBuyCredits;
    private CreditsItemsMenuController creditsItemsMenu;

    private GameObject windowToBuyCredits;


    private Animator animator;
    private CanvasGroup canvasGroup;

    private CanvasController canvasController;

    private PowerUpItem itemToBuy;

    private void Awake()
    {
        soundManager = SoundManager.Instance;
        soundManager.PlayMusicStore();
        canvasController = GetComponentInParent<CanvasController>();
        gameController = GameController.Instance;
        PromptWindowController[] promptWindowControllers = canvasController.GetComponentsInChildren<PromptWindowController>(true);
        CanvasGroup[] canvasGroup = canvasController.GetComponentsInChildren<CanvasGroup>(true);


        if (!promptWindowControllers.Any(x => x.name == "WindowsToBuyUpgrade"))
        {
            windowsToBuyUpgrade = Instantiate(propmtWindowsPrefab.GetComponent<PromptWindowController>(), canvasController.gameObject.transform, false);
            windowsToBuyUpgrade.name = "WindowsToBuyUpgrade";
        }
        else
        {
            windowsToBuyUpgrade = promptWindowControllers.Single(x => x.name == "WindowsToBuyUpgrade");
        }

        if (!promptWindowControllers.Any(x => x.name == "WindowsToBuyCredits"))
        {
            WindowsToBuyCredits = Instantiate(propmtWindowsPrefab.GetComponent<PromptWindowController>(), canvasController.gameObject.transform, false);
            WindowsToBuyCredits.name = "WindowsToBuyCredits";
        }
        else
        {
            WindowsToBuyCredits = promptWindowControllers.Single(x => x.name == "WindowsToBuyCredits");
        }


        if (!canvasGroup.Any(x => x.name == "CreditsItemsMenu"))
        {
            creditsItemsMenu = Instantiate(creditsMenuPrefab.GetComponent<CreditsItemsMenuController>(), canvasController.gameObject.transform, false);

            creditsItemsMenu.name = "CreditsItemsMenu";
        }
        else
        {
            creditsItemsMenu = canvasGroup.Single(x => x.name == "CreditsItemsMenu").GetComponent<CreditsItemsMenuController>();
        }


        windowsToBuyUpgrade.transform.SetAsLastSibling();
        WindowsToBuyCredits.transform.SetAsLastSibling();
        creditsItemsMenu.transform.SetAsLastSibling();
        canvasController.SetBackground(background);
        windowsToBuyUpgrade.gameObject.SetActive(false);
        WindowsToBuyCredits.gameObject.SetActive(false);
        creditsItemsMenu.gameObject.SetActive(false);
    }


    // Start is called before the first frame update
    void Start()
    {
        windowsToBuyUpgrade.OnButtonYesClicked += OnBuyItem;
        windowsToBuyUpgrade.OnButtonNoClicked += OnClosePromptWindows;

        WindowsToBuyCredits.OnButtonYesClicked += ShowWindowToBuyCredits;
        WindowsToBuyCredits.OnButtonNoClicked += OnClosePromptWindows;

        foreach (GameObject item in itemsStore)
        {
            item.GetComponent<StoreItemController>().OnBuyItem += OnInitBuyItem;
        }
    }

    private void OnInitBuyItem(StoreItem obj)
    {
        itemToBuy = (PowerUpItem)obj;
        if (gameController.PersistentData.Credits >= itemToBuy.CreditCost)
        {
            //Show ventana confirmacion
            int quantity = gameController.PersistentData.GetQuantityUpgrade(itemToBuy);
            string textQuantity = String.Format("You have {0} units of this item.", quantity);
            windowsToBuyUpgrade.SetText("Do you want to buy this upgrade?\n \n" + textQuantity);
            windowsToBuyUpgrade.gameObject.SetActive(true);
        }
        else
        {
            WindowsToBuyCredits.SetText("You haven't enough MCs credits to buy this upgrade, Do you want to buy some MCs credits?");
            WindowsToBuyCredits.gameObject.SetActive(true);
        }
        canvasController.SetBlockRaycasts(false);
    }

    private void ShowWindowToBuyCredits()
    {
        WindowsToBuyCredits.gameObject.SetActive(false);
        creditsItemsMenu.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        if (windowsToBuyUpgrade)
        {
            windowsToBuyUpgrade.OnButtonYesClicked -= OnBuyItem;
            windowsToBuyUpgrade.OnButtonYesClicked -= OnClosePromptWindows;
            //Destroy(windowsToBuyUpgrade);
        }
        if (WindowsToBuyCredits)
        {
            WindowsToBuyCredits.OnButtonYesClicked -= OnBuyItem;
            WindowsToBuyCredits.OnButtonYesClicked -= OnClosePromptWindows;
            //Destroy(WindowsToBuyCredits, 1.0f);
        }

        if (itemsStore != null)
        {
            foreach (GameObject item in itemsStore)
            {
                item.GetComponent<StoreItemController>().OnBuyItem -= OnInitBuyItem;
            }
        }
    }

    private void OnBuyItem()
    {
        //Todo Restar creditos a creditos totales
        gameController.PersistentData.Credits -= itemToBuy.CreditCost;
        canvasController.UpdateCredits();
        //Almacenar upgrade
        gameController.PersistentData.AddUpgradeToUser(itemToBuy);
        gameController.SaveInformation();
        ShowThanksWindow();

        canvasController.SetBlockRaycasts(true);
        windowsToBuyUpgrade.gameObject.SetActive(false);

    }

    private void ShowThanksWindow()
    {
        //TODO SHowThanklsWindow

        //throw new NotImplementedException();
    }

    private void OnClosePromptWindows()
    {
        canvasController.SetBlockRaycasts(true);
        itemToBuy = null;
    }

    //Used in GUI
    public void ButtonBack()
    {
        soundManager.PlayMusicClick();
        soundManager.PlayStopMusicStore();

        //Debug.Log("continue");
        UtilsCanvas.ChangeCanvas(gameMenuPrefab, gameObject);
    }

    private void Hide()
    {
        animator.SetBool("Shown", false);
        soundManager.StopGameMenuMusic();

        canvasGroup.interactable = false;
    }

}
