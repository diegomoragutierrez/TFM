using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsItemsMenuController : MonoBehaviour {
    private CanvasController canvasController;

    private PromptWindowController windowsToBuyCredits;

    private CanvasGroup canvasGroup;

    private GameController gameController;

    SoundManager soundManager;


    [SerializeField]
    private GameObject propmtWindowsPrefab;

    [SerializeField]
    private List<GameObject> itemsStore;

    private CreditItem itemToBuy;
    // Start is called before the first frame update

    private void Awake()
    {
        canvasController = GetComponentInParent<CanvasController>();
        
    }
    void Start()
    {
        windowsToBuyCredits = Instantiate(propmtWindowsPrefab.GetComponent<PromptWindowController>(), canvasController.gameObject.transform, false);
        windowsToBuyCredits.gameObject.SetActive(false);
        gameController = GameController.Instance;
        canvasGroup = GetComponent<CanvasGroup>();
        soundManager = SoundManager.Instance;

        canvasController.SetBlockRaycasts(false);
        foreach (GameObject item in itemsStore)
        {
            item.GetComponent<StoreItemController>().OnBuyItem += OnBuyItem;
        }
    }

    private void OnBuyItem(StoreItem itemBought)
    {
        gameController.PersistentData.Credits += ((CreditItem)itemBought).CreditsToBuy;
        gameController.SaveInformation();
        canvasController.UpdateCredits();
    }


    private void OnDestroy()
    {
        if (itemsStore != null)
        {
            foreach (GameObject item in itemsStore)
            {
                item.GetComponent<StoreItemController>().OnBuyItem += OnBuyItem;
            }
        }
    }
    private void OnDisable()
    {
        canvasController.SetBlockRaycasts(true);
    }

    // Update is called once per frame



    public void OnCloseButton()
    {
        soundManager.PlayMusicClick();

        gameObject.SetActive(false);
    }
}
